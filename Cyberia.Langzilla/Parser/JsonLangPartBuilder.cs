using Cyberia.Langzilla.Parser.Extensions;

using System.Text;
using System.Text.Json;

namespace Cyberia.Langzilla.Parser;

/// <summary>
/// Builds lang parts for parsing purposes.
/// </summary>
internal sealed class JsonLangPartBuilder
{
    private readonly string _name;
    private readonly JsonValueKind _valueKind;
    private readonly StringBuilder _builder;
    private readonly StringBuilder _valueSanitizerBuilder;

    /// <summary>
    /// Initializes a new instance of the LangPartBuilder class.
    /// </summary>
    /// <param name="name">The name of the part to be built.</param>
    /// <param name="valueKind">Indicates whether the part to build should be formatted as an array.</param>
    private JsonLangPartBuilder(string name, JsonValueKind valueKind)
    {
        _name = name;
        _valueKind = valueKind;
        _builder = new();
        _valueSanitizerBuilder = new();

        _builder.Append('"').Append(name).Append('"').Append(':');
        _builder.AppendStartJsonToken(valueKind);
    }

    /// <summary>
    /// Creates a new <see cref="JsonLangPartBuilder"/> instance.
    /// </summary>
    /// <param name="name">The name of the part.</param>
    /// <param name="keySegment">The key segment used to determine the value kind of the part.</param>
    /// <returns>A new instance of <see cref="JsonLangPartBuilder"/>.</returns>
    public static JsonLangPartBuilder Create(string name, ReadOnlySpan<char> keySegment)
    {
        var truncatedKeySegment = keySegment[name.Length..];
        var firstChar = truncatedKeySegment.IsEmpty ? '\0' : truncatedKeySegment[0];
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
    /// <param name="keySegment">The key segment of the line.</param>
    /// <param name="valueSegment">The value segment of the line.</param>
    /// <returns>A reference to this instance after the append operation has completed.</returns>
    public JsonLangPartBuilder Append(ReadOnlySpan<char> keySegment, ReadOnlySpan<char> valueSegment)
    {
        var truncatedKeySegment = _name.Length > keySegment.Length ? ReadOnlySpan<char>.Empty : keySegment[_name.Length..];
        var sanitizedValueSegment = SanitizeValueSegment(valueSegment);

        if (_valueKind == JsonValueKind.Undefined || truncatedKeySegment.IsEmpty)
        {
            _builder.Append(sanitizedValueSegment);
        }
        else if (_valueKind == JsonValueKind.Object)
        {
            _builder.AppendJsonProperty(truncatedKeySegment[1..], sanitizedValueSegment);
        }
        else if (_valueKind == JsonValueKind.Array)
        {
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
    private ReadOnlySpan<char> SanitizeValueSegment(ReadOnlySpan<char> valueSegment)
    {
        var firstChar = valueSegment[0];
        if (firstChar is not ('[' or '{' or '\''))
        {
            return valueSegment[..^1];
        }

        _valueSanitizerBuilder.Clear();

        var inString = firstChar == '\'';
        _valueSanitizerBuilder.Append(inString ? '"' : firstChar);

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
                        _valueSanitizerBuilder.Append('\'');
                        i++;
                        break;
                    case '\'':
                        inString = false;
                        _valueSanitizerBuilder.Append('"');
                        break;
                    case '"':
                        _valueSanitizerBuilder.Append('\\').Append('"');
                        break;
                    case < (char)32:
                        _valueSanitizerBuilder.AppendEscapedChar(currentChar);
                        break;
                    default:
                        _valueSanitizerBuilder.Append(currentChar);
                        break;
                }
            }
            else
            {
                switch (currentChar)
                {
                    case ' ' when previousChar == '\'' && nextChar == '+':
                        inString = true;
                        _valueSanitizerBuilder[^1] = '\\';
                        _valueSanitizerBuilder.Append('"');
                        i += 9;
                        break;
                    case '\'':
                        inString = true;
                        _valueSanitizerBuilder.Append('"');
                        break;
                    default:
                        _valueSanitizerBuilder.Append(currentChar);
                        break;
                }
            }
        }

        return _valueSanitizerBuilder.ToString().AsSpan();
    }
}
