using System.Buffers;
using System.Text;
using System.Text.Json;

namespace Cyberia.Langzilla.Parser;

/// <summary>
/// Builds lang parts for parsing purposes.
/// </summary>
internal sealed class JsonLangPartBuilder
{
    private static readonly ArrayPool<char> s_arrayPool = ArrayPool<char>.Shared;

    private readonly int _nameLength;
    private readonly JsonValueKind _valueKind;
    private readonly StringBuilder _builder;

    /// <summary>
    /// Initializes a new instance of the LangPartBuilder class.
    /// </summary>
    /// <param name="name">The name of the part to be built.</param>
    /// <param name="valueKind">Indicates whether the part to build should be formatted as an array.</param>
    private JsonLangPartBuilder(ReadOnlySpan<char> name, JsonValueKind valueKind)
    {
        _nameLength = name.Length;
        _valueKind = valueKind;
        _builder = new();

        _builder.Append('"').Append(name).Append('"').Append(':');
        _builder.AppendStartJsonToken(valueKind);
    }

    /// <summary>
    /// Creates a new <see cref="JsonLangPartBuilder"/> instance.
    /// </summary>
    /// <param name="name">The name of the part.</param>
    /// <param name="truncatedKeySegment">The key segment truncated from the name used to determine the value kind of the created part.</param>
    /// <returns>A new instance of <see cref="JsonLangPartBuilder"/>.</returns>
    public static JsonLangPartBuilder Create(ReadOnlySpan<char> name, ReadOnlySpan<char> truncatedKeySegment)
    {
        if (truncatedKeySegment.IsEmpty)
        {
            return new JsonLangPartBuilder(name, JsonValueKind.Undefined);
        }

        var firstChar = truncatedKeySegment[0];
        var valueKind = firstChar switch
        {
            '[' => JsonValueKind.Array,
            '.' => JsonValueKind.Object,
            _ => JsonValueKind.Undefined
        };

        return new JsonLangPartBuilder(name, valueKind);
    }

    /// <summary>
    /// Appends the specified segment to the part.
    /// </summary>
    /// <param name="truncatedKeySegment">The key segment of the line.</param>
    /// <param name="valueSegment">The value segment of the line.</param>
    /// <returns>A reference to this instance after the append operation has completed.</returns>
    public JsonLangPartBuilder Append(ReadOnlySpan<char> truncatedKeySegment, ReadOnlySpan<char> valueSegment)
    {
        var sanitizedValueSegment = SanitizeValueSegment(valueSegment);

        if (truncatedKeySegment.IsEmpty)
        {
            _builder.Append(sanitizedValueSegment);
        }
        else
        {
            switch (_valueKind)
            {
                case JsonValueKind.Undefined:
                    _builder.Append(sanitizedValueSegment);
                    break;
                case JsonValueKind.Object:
                    _builder.AppendJsonProperty(truncatedKeySegment[1..], sanitizedValueSegment);
                    break;
                case JsonValueKind.Array:
                    _builder.Append('{');

                    var indexOfOpenBracket = 0;
                    for (var i = 1; indexOfOpenBracket != -1; i++)
                    {
                        var indexOfEndBracket = truncatedKeySegment.IndexOf(']');
                        var name = "id" + (i == 1 ? string.Empty : i);
                        var value = truncatedKeySegment[1..indexOfEndBracket];
                        truncatedKeySegment = truncatedKeySegment[(indexOfEndBracket + 1)..];
                        _builder.AppendJsonProperty(name, value);
                        _builder.Append(',');
                        indexOfOpenBracket = truncatedKeySegment.IndexOf('[');
                    }

                    if (sanitizedValueSegment[0] == '{')
                    {
                        _builder.Append(sanitizedValueSegment[1..]);
                    }
                    else
                    {
                        _builder.AppendJsonProperty("v", sanitizedValueSegment);
                        _builder.Append('}');
                    }
                    break;
            }
        }

        _builder.Append(',');
        return this;
    }

    /// <summary>
    /// Finalizes the part building.
    /// </summary>
    /// <returns>A reference to the insternal <see cref="StringBuilder"/> instance.</returns>
    public StringBuilder Build()
    {
        _builder.Remove(_builder.Length - 1, 1);
        _builder.AppendEndJsonToken(_valueKind);

        return _builder;
    }

    /// <summary>
    /// Sanitizes the value segment to ensure valid JSON output.
    /// </summary>
    /// <param name="valueSegment">The value segment to sanitize.</param>
    /// <returns>A <see cref="ReadOnlySpan{T}"/> of <see cref="char"/> containing the sanitized value.</returns>
    private static ReadOnlySpan<char> SanitizeValueSegment(ReadOnlySpan<char> valueSegment)
    {
        var firstChar = valueSegment[0];
        if (firstChar is not ('[' or '{' or '\''))
        {
            return valueSegment[..^1];
        }

        var buffer = s_arrayPool.Rent(valueSegment.Length * 2);
        var bufferSpan = buffer.AsSpan();
        var bufferIndex = 0;

        try
        {
            var inString = firstChar == '\'';
            bufferSpan[bufferIndex++] = inString ? '"' : firstChar;

            var length = valueSegment.Length - 1;
            for (var i = 1; i < length; i++)
            {
                var previousChar = valueSegment[i - 1];
                var currentChar = valueSegment[i];
                var nextChar = valueSegment[i + 1];

                if (inString)
                {
                    switch (currentChar)
                    {
                        case '\\' when nextChar == '\'':
                            bufferSpan[bufferIndex++] = '\'';
                            i++;
                            break;
                        case '\'':
                            inString = false;
                            bufferSpan[bufferIndex++] = '"';
                            break;
                        case '"':
                            bufferSpan[bufferIndex++] = '\\';
                            bufferSpan[bufferIndex++] = '"';
                            break;
                        case < (char)32:
                            bufferSpan.AppendEscapedChar(currentChar, ref bufferIndex);
                            break;
                        default:
                            bufferSpan[bufferIndex++] = currentChar;
                            break;
                    }
                }
                else
                {
                    switch (currentChar)
                    {
                        case ' ' when previousChar == '\'' && nextChar == '+':
                            inString = true;
                            bufferSpan[bufferIndex - 1] = '\\';
                            bufferSpan[bufferIndex++] = '"';
                            i += 9;
                            break;
                        case ' ':
                            break;
                        case '\'':
                            inString = true;
                            bufferSpan[bufferIndex++] = '"';
                            break;
                        default:
                            bufferSpan[bufferIndex++] = currentChar;
                            break;
                    }
                }
            }

            return bufferSpan[..bufferIndex];
        }
        finally
        {
            s_arrayPool.Return(buffer);
        }
    }
}
