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
    private static readonly string[] s_ignoredEndingLines = ["new Object();", "new Array();"];

    private readonly FileStream _fileStream;
    private readonly StreamReader _streamReader;
    private readonly StringBuilder _builder;
    private readonly Dictionary<string, JsonLangPartBuilder> _partBuilders;
    private readonly Dictionary<string, JsonLangPartBuilder>.AlternateLookup<ReadOnlySpan<char>> _alternatePartBuilders;
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
        _alternatePartBuilders = _partBuilders.GetAlternateLookup<ReadOnlySpan<char>>();
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

        return new JsonLangParser(filePath);
    }

    /// <summary>
    /// Parses the lang data.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown when the parser has been disposed.</exception>
    public void Parse()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        JsonLangPartBuilder currentBuilder;

        // Create the parts
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
            var keySegmentName = FindKeySegmentName(keySegment);
            var truncatedKeySegment = keySegment[keySegmentName.Length..];
            var valueSegment = currentLine[(indexOfSeparator + c_lineSeparator.Length)..];

            currentBuilder = GetOrCreateLangPartBuilder(keySegmentName, truncatedKeySegment);
            currentBuilder.Append(truncatedKeySegment, valueSegment);
        }

        // Concatenate the parts
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

        var ignoredEndingLines = s_ignoredEndingLines.AsSpan();
        var length = ignoredEndingLines.Length;
        for (var i = 0; i < length; i++)
        {
            if (line.EndsWith(ignoredEndingLines[i], StringComparison.Ordinal))
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
    private static ReadOnlySpan<char> FindKeySegmentName(ReadOnlySpan<char> keySegment)
    {
        var indexOfOpenBracket = keySegment.IndexOf('[');
        if (indexOfOpenBracket != -1)
        {
            return keySegment[..indexOfOpenBracket];
        }

        var indexOfDot = keySegment.IndexOf('.');
        if (indexOfDot == -1)
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
    /// <param name="truncatedKeySegment">The key segment truncated by the name used to determine the value kind of the created part.</param>
    /// <returns>The builder.</returns>
    private JsonLangPartBuilder GetOrCreateLangPartBuilder(ReadOnlySpan<char> name, ReadOnlySpan<char> truncatedKeySegment)
    {
        if (!_alternatePartBuilders.TryGetValue(name, out var builder))
        {
            builder = JsonLangPartBuilder.Create(name, truncatedKeySegment);
            _alternatePartBuilders[name] = builder;
        }

        return builder;
    }
}
