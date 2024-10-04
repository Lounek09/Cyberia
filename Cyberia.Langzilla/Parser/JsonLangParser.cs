using Cyberia.Langzilla.Models;

using System.Buffers;
using System.Text;

namespace Cyberia.Langzilla.Parser;

/// <summary>
/// Parses lang data into JSON.
/// </summary>
public sealed class JsonLangParser : IDisposable
{
    internal static readonly SearchValues<char> UpperCaseLettersSearch = SearchValues.Create("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
    internal static readonly SearchValues<char> LowerCaseLettersSearch = SearchValues.Create("abcdefghijklmnopqrstuvwxyz");
    internal static readonly SearchValues<char> DigitsSearch = SearchValues.Create("0123456789");

    private const string c_lineSeparator = " = ";
    private static readonly IReadOnlyList<string> s_ignoredEndingLines = ["new Object();", "new Array();"];

    private readonly FileStream _fileStream;
    private readonly StreamReader _streamReader;
    private readonly StringBuilder _builder;
    private readonly Dictionary<string, JsonLangPartBuilder> _partBuilders;
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonLangParser"/> class.
    /// </summary>
    /// <param name="filePath">The path to the lang file to parse.</param>
    private JsonLangParser(string filePath)
    {
        _fileStream = new(filePath, FileMode.Open, FileAccess.Read);
        _streamReader = new(_fileStream);
        _builder = new();
        _partBuilders = [];
    }

    /// <summary>
    /// Creates a new <see cref="JsonLangParser"/> instance and parses the lang data.
    /// </summary>
    /// <param name="lang">The lang to parse.</param>
    /// <returns>A new instance of <see cref="JsonLangParser"/></returns>
    /// <exception cref="FileNotFoundException">Thrown when the lang file has never been decompiled.</exception>
    public static JsonLangParser Create(Lang lang)
    {
        var filePath = lang.CurrentDecompiledFilePath;
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"The {lang.Name} lang has never been decompiled.");
        }

        JsonLangParser langParser = new(filePath);
        langParser.Parse();

        return langParser;
    }

    /// <summary>
    /// Returns the string from the internal <see cref="StringBuilder"/> that represents the lang data parsed to JSON.
    /// </summary>
    /// <returns>The JSON representation of the lang data.</returns>
    public override string ToString()
    {
        return _builder.ToString();
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _fileStream.Dispose();
            _streamReader.Dispose();
            _disposed = true;
        }
    }

    /// <summary>
    /// Parses the lang data.
    /// </summary>
    private void Parse()
    {
        var currentPartName = ReadOnlySpan<char>.Empty;
        JsonLangPartBuilder? currentBuilder = null;

        while (!_streamReader.EndOfStream)
        {
            var currentLine = _streamReader.ReadLine().AsSpan();
            if (IsLineIgnored(currentLine))
            {
                continue;
            }

            var indexOfSeparator = currentLine.IndexOf(c_lineSeparator);
            if (indexOfSeparator == -1)
            {
                continue;
            }

            var keySegment = currentLine[..indexOfSeparator];
            var valueSegment = currentLine[(indexOfSeparator + c_lineSeparator.Length)..];

            var newPartName = FindName(keySegment);
            if (currentPartName.IsEmpty || !currentPartName.Equals(newPartName, StringComparison.Ordinal))
            {
                currentPartName = newPartName;
                currentBuilder = GetOrCreateLangPartBuilder(currentPartName.ToString(), keySegment);
            }

            currentBuilder?.Append(keySegment, valueSegment);
        }

        FinalizeParsing();
    }

    /// <summary>
    /// Checks if a line should be ignored.
    /// </summary>
    /// <param name="line">The line to check.</param>
    /// <returns>True if the line should be ignored.</returns>
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

    /// <summary>
    /// Finds the name of a key segment.
    /// </summary>
    /// <param name="keySegment">The key segment to find the name of.</param>
    /// <returns>The name found.</returns>
    private static ReadOnlySpan<char> FindName(ReadOnlySpan<char> keySegment)
    {
        var indexOfOpenBracket = keySegment.IndexOf('[');
        if (indexOfOpenBracket != -1)
        {
            return keySegment[..indexOfOpenBracket];
        }

        var indexOfDot = keySegment.IndexOf('.');
        if (indexOfDot == -1 || indexOfDot + 1 == keySegment.Length)
        {
            return keySegment;
        }

        var afterDot = keySegment[(indexOfDot + 1)..];
        if (afterDot.ContainsAny(LowerCaseLettersSearch))
        {
            return keySegment;
        }

        return keySegment[..indexOfDot];
    }

    /// <summary>
    /// Gets or creates a builder that represents a part of the lang.
    /// </summary>
    /// <param name="name">The name of the builder.</param>
    /// <param name="keySegment">The key segment used to determine the value kind of the created part.</param>
    /// <returns>The builder.</returns>
    private JsonLangPartBuilder GetOrCreateLangPartBuilder(string name, ReadOnlySpan<char> keySegment)
    {
        if (!_partBuilders.TryGetValue(name, out var builder))
        {
            builder = JsonLangPartBuilder.Create(name, keySegment);
            _partBuilders.Add(name, builder);
        }

        return builder;
    }

    /// <summary>
    /// Finalizes the parsing.
    /// </summary>
    private void FinalizeParsing()
    {
        List<StringBuilder> builtParts = [];
        var capacity = 1;

        foreach (var partBuilder in _partBuilders.Values)
        {
            var builtPart = partBuilder.Build();
            builtParts.Add(builtPart);
            capacity += builtPart.Length + 1;
        }

        _builder.Capacity = capacity;

        _builder.Append('{');

        foreach (var builtPart in builtParts)
        {
            _builder.Append(builtPart);
            _builder.Append(',');
        }

        if (_builder.Length > 1)
        {
            _builder[^1] = '}';
        }
        else
        {
            _builder.Append('}');
        }
    }
}
