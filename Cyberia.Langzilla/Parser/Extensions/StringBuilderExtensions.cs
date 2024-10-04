using System.Text;
using System.Text.Json;

namespace Cyberia.Langzilla.Parser.Extensions;

internal static class StringBuilderExtensions
{
    private static readonly IReadOnlyList<char> s_hexDigits = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'];

    /// <summary>
    /// Appends the start of a JSON token based on the specified <see cref="JsonValueKind"/>.
    /// </summary>
    /// <param name="builder">The StringBuilder instance to which the JSON token will be appended.</param>
    /// <param name="valueKind">The <see cref="JsonValueKind"/> that determines the type of token to append.</param>
    /// <returns>A reference to this instance after the append operation has completed.</returns>
    public static StringBuilder AppendStartJsonToken(this StringBuilder builder, JsonValueKind valueKind)
    {
        return valueKind switch
        {
            JsonValueKind.Array => builder.Append('['),
            JsonValueKind.Object => builder.Append('{'),
            JsonValueKind.String => builder.Append('"'),
            _ => builder
        };
    }

    /// <summary>
    /// Appends the end of a JSON token based on the specified <see cref="JsonValueKind"/>.
    /// </summary>
    /// <param name="builder">The StringBuilder instance to which the JSON token will be appended.</param>
    /// <param name="valueKind">The <see cref="JsonValueKind"/> that determines the type of token to append.</param>
    /// <returns>A reference to this instance after the append operation has completed.</returns>
    public static StringBuilder AppendEndJsonToken(this StringBuilder builder, JsonValueKind valueKind)
    {
        return valueKind switch
        {
            JsonValueKind.Array => builder.Append(']'),
            JsonValueKind.Object => builder.Append('}'),
            JsonValueKind.String => builder.Append('"'),
            _ => builder
        };
    }

    /// <summary>
    /// Appends a named property and its value to the StringBuilder, optionally enclosing the value in quotes.
    /// </summary>
    /// <param name="builder">The StringBuilder to which the property will be appended.</param>
    /// <param name="name">The name of the property. This is always treated as a string and enclosed in double quotes.</param>
    /// <param name="value">The value of the property. This can be enclosed in double quotes based on the value of <paramref name="encloseValueInQuotes"/>.</param>
    /// <param name="encloseValueInQuotes">Specifies whether the value should be enclosed in double quotes.</param>
    /// <returns>A reference to this instance after the append operation has completed.</returns>
    public static StringBuilder AppendJsonProperty(this StringBuilder builder, ReadOnlySpan<char> name, ReadOnlySpan<char> value, bool encloseValueInQuotes = false)
    {
        builder.Append('"').Append(name).Append('"').Append(':');

        if (encloseValueInQuotes)
        {
            return builder.Append('"').Append(value).Append('"');
        }

        return builder.Append(value);
    }

    /// <summary>
    /// Appends the specified character to the StringBuilder as a Unicode escape sequence (\uXXXX).
    /// </summary>
    /// <param name="builder">The StringBuilder instance to which the escape sequence will be appended.</param>
    /// <param name="value">The character to be escaped and appended.</param>
    /// <returns>A reference to this instance after the append operation has completed.</returns>
    public static StringBuilder AppendEscapedChar(this StringBuilder builder, char value)
    {
        return builder.Append('\\').Append('u')
            .Append(s_hexDigits[(value >> 12) & 0xF])
            .Append(s_hexDigits[(value >> 8) & 0xF])
            .Append(s_hexDigits[(value >> 4) & 0xF])
            .Append(s_hexDigits[value & 0xF]);
    }
}
