using System.Text;
using System.Text.Json;

namespace Cyberia.Langzilla.Parser;

public static class JsonLangParserUtils
{
    private static readonly char[] s_hexDigits = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'];

    extension(StringBuilder builder)
    {
        /// <summary>
        /// Appends the start of a JSON token based on the specified <see cref="JsonValueKind"/>.
        /// </summary>
        /// <param name="valueKind">The <see cref="JsonValueKind"/> that determines the type of token to append.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public StringBuilder AppendStartJsonToken(JsonValueKind valueKind)
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
        /// <param name="valueKind">The <see cref="JsonValueKind"/> that determines the type of token to append.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public StringBuilder AppendEndJsonToken(JsonValueKind valueKind)
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
        /// <param name="name">The name of the property. This is always treated as a string and enclosed in double quotes.</param>
        /// <param name="value">The value of the property. This can be enclosed in double quotes based on the value of <paramref name="encloseValueInQuotes"/>.</param>
        /// <param name="encloseValueInQuotes">Specifies whether the value should be enclosed in double quotes.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public StringBuilder AppendJsonProperty(ReadOnlySpan<char> name, ReadOnlySpan<char> value, bool encloseValueInQuotes = false)
        {
            builder.Append('"').Append(name).Append('"').Append(':');

            if (encloseValueInQuotes)
            {
                return builder.Append('"').Append(value).Append('"');
            }

            return builder.Append(value);
        }
    }

    extension(Span<char> value)
    {
        /// <summary>
        /// Appends the specified character at the specified index to the Span as a Unicode escape sequence (\uXXXX).
        /// Make sure the buffer has enough space to append the character (length of buffer - index >= 6).
        /// </summary>
        /// <param name="c">The character to be escaped and appended./param>
        /// <param name="index">The index at which the character will be appended.</param>
        /// <returns>The number of characters appended to the Span.</returns>
        public int AppendEscapedChar(char c, ref int index)
        {
            var hexDigits = s_hexDigits.AsSpan();

            value[index++] = '\\';
            value[index++] = 'u';
            value[index++] = hexDigits[c >> 12 & 0xF];
            value[index++] = hexDigits[c >> 8 & 0xF];
            value[index++] = hexDigits[c >> 4 & 0xF];
            value[index++] = hexDigits[c & 0xF];

            return 6;
        }
    }
}
