using System.Buffers;
using System.Text;
using System.Text.Json;

namespace Cyberia.Api.Parser;

public sealed class LangParserBetter : IDisposable
{
    private const string c_keyBodySegmentSeparator = " = ";
    private const char c_keySeparator = '.';
    private const char c_keyOpenId = '[';
    private const char c_keyCloseId = ']';

    private static readonly IReadOnlyList<string> s_ignoredEndingLines = ["new Object();", "new Array();"];
    private static readonly SearchValues<char> s_lowerCaseLettersSearch = SearchValues.Create("abcdefghijklmnopqrstuvwxyz");

    private readonly FileStream _fileStream;
    private readonly StreamReader _streamReader;
    private readonly Dictionary<string, StringBuilder> _builder;
    private bool _disposed;

    public LangParserBetter(string filePath)
    {
        if (Path.GetExtension(filePath) != ".as")
        {
            throw new ArgumentException("The file must be an ActionScript file", nameof(filePath));
        }

        _fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        _streamReader = new StreamReader(_fileStream);
        _builder = [];
        _disposed = false;
    }

    public void Parse()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        Span<Range> separatorRanges = stackalloc Range[2];
        while (!_streamReader.EndOfStream)
        {
            var currentLine = _streamReader.ReadLine().AsSpan();
            if (IsLineIgnored(currentLine))
            {
                continue;
            }

            var splitCount = currentLine.Split(separatorRanges, c_keyBodySegmentSeparator, StringSplitOptions.RemoveEmptyEntries);
            if (splitCount != 2)
            {
                continue;
            }

            var keySegment = currentLine[separatorRanges[0]];
            var valueSegment = currentLine[separatorRanges[1]];

            ParseKeySegment(keySegment, out var currentBuilder);
            if (currentBuilder is null)
            {
                continue;
            }

            ParseBodySegment(currentBuilder, valueSegment);
        }
    }

    private static bool IsLineIgnored(ReadOnlySpan<char> line)
    {
        if (line.IsEmpty || line.IsWhiteSpace())
        {
            return true;
        }

        foreach (var ignoredEndingLine in s_ignoredEndingLines)
        {
            if (line.EndsWith(ignoredEndingLine, StringComparison.Ordinal))
            {
                return true;
            }
        }

        return false;
    }

    private void ParseKeySegment(ReadOnlySpan<char> input, out StringBuilder? currentBuilder)
    {
        currentBuilder = null;

        var indexOfOpenId = input.IndexOf(c_keyOpenId);
        if (indexOfOpenId == -1)
        {
            var indexOfSeparator = input.IndexOf(c_keySeparator);
            if (indexOfSeparator == -1 ||
                input.ContainsAny(s_lowerCaseLettersSearch))
            {
                currentBuilder = GetOrCreateBuilder(input.ToString());
                return;
            }

            currentBuilder = GetOrCreateBuilder(input[..indexOfSeparator].ToString());
            currentBuilder.Append('{');
            currentBuilder.AppendJsonProperty("id", input[(indexOfSeparator + 1)..], JsonValueKind.String);
            return;
        }
       
        var indexOfCloseId = input.IndexOf(c_keyCloseId);
        if (indexOfCloseId == -1)
        {
            return;
        }

        currentBuilder = GetOrCreateBuilder(input[..indexOfOpenId].ToString());

        var idName = ReadOnlySpan<char>.Empty;
        var count = 1;
        while (indexOfOpenId != -1)
        {
            currentBuilder.AppendJsonProperty($"id{idName}", input[(indexOfOpenId + 1)..indexOfCloseId], JsonValueKind.Number);
            if (indexOfCloseId == input.Length - 1)
            {
                return;
            }

            count++;
            input = input[(indexOfCloseId + 1)..];

            indexOfOpenId = input.IndexOf(c_keyOpenId);

            var indexOfDot = input.IndexOf(c_keySeparator);
            if (indexOfDot == -1)
            {
                idName = count.ToString();
                continue;
            }

            idName = input[(indexOfDot + 1)..indexOfOpenId];
            input = input[(indexOfOpenId + 1)..];
        }
    }

    private void ParseBodySegment(StringBuilder currentBuilder, ReadOnlySpan<char> input)
    {
        currentBuilder.Append('0');
    }

    public override string ToString()
    {
        var capacity = _builder.Sum(x => x.Key.Length + x.Value.Length + 1) + 1;
        StringBuilder builder = new(capacity);

        builder.Append('{');

        foreach (var (key, value) in _builder)
        {
            builder.Append(value);
            builder.Append(',');
        }

        if (builder.Length > 1)
        {
            builder[^1] = '}';
        }
        else
        {
            builder.Append('}');
        }

        return builder.ToString();
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _fileStream.Dispose();
            _streamReader.Dispose();
            GC.SuppressFinalize(this);

            _disposed = true;
        }
    }

    private StringBuilder GetOrCreateBuilder(string key)
    {
        if (!_builder.TryGetValue(key, out var builder))
        {
            builder = new StringBuilder();
            builder.Append('"');
            builder.Append(key);
            builder.Append("\":");
            _builder.Add(key, builder);
        }

        return builder;
    }

    private void Append(string key, char input)
    {
        var builder = GetOrCreateBuilder(key);
        builder.Append(input);
    }

    private void Append(string key, ReadOnlySpan<char> input)
    {
        var builder = GetOrCreateBuilder(key);
        builder.Append(input);
    }

    private void AppendJsonProperty(string key, ReadOnlySpan<char> name, ReadOnlySpan<char> value, JsonValueKind kind)
    {
        var builder = GetOrCreateBuilder(key);

        builder.Append('"');
        builder.Append(name);
        builder.Append("\":");

        if (kind == JsonValueKind.String)
        {
            builder.Append('"');
        }

        builder.Append(value);

        if (kind == JsonValueKind.String)
        {
            builder.Append('"');
        }
    }
}

