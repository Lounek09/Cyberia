using System.Buffers;
using System.Collections.ObjectModel;

namespace Cyberia.Api.Utils;

/// <summary>
/// Provides methods for encoding and decoding strings.
/// </summary>
public static class PatternDecoder
{
    private const int c_base64Length = 64;

    private static readonly char[] s_base64Chars =
    [
        'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '-', '_'
    ];
    private static readonly SearchValues<char> s_base64CharsSearch = SearchValues.Create(s_base64Chars);

    /// <summary>
    /// Checks if the given character is a valid Base64 character.
    /// </summary>
    /// <param name="value">The character to check.</param>
    /// <returns><see langword="true"/> if the character is a valid Base64 character; otherwise, <see langword="false"/>.</returns>
    public static bool IsBase64(char value)
    {
        return s_base64CharsSearch.Contains(value);
    }

    /// <summary>
    /// Checks if the given string is a valid Base64 string.
    /// </summary>
    /// <param name="value">The string to check.</param>
    /// <returns><see langword="true"/> if the string is a valid Base64 string; otherwise, <see langword="false"/>.</returns>
    public static bool IsBase64(ReadOnlySpan<char> value)
    {
        return !value.ContainsAnyExcept(s_base64CharsSearch);
    }

    /// <summary>
    /// Decodes a Base64 character to its corresponding index in the Base64 encoding table.
    /// </summary>
    /// <param name="value">The Base64 character to convert.</param>
    /// <returns>The index of the character in the Base64 encoding table.</returns>
    /// <exception cref="ArgumentException">Thrown if the character is not a valid Base64 character.</exception>
    public static int Decode64(char value)
    {
        var index = Array.IndexOf(s_base64Chars, value);

        if (index == -1)
        {
            throw new ArgumentException($"The character '{value}' is not a valid Base64 character.", nameof(value));
        }

        return index;
    }

    /// <summary>
    /// Encodes an index in the Base64 encoding table to its corresponding Base64 character.
    /// </summary>
    /// <param name="index">The index to convert.</param>
    /// <returns>The Base64 character corresponding to the given index.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the index is outside the range of the Base64 encoding table.</exception>
    public static char Encode64(int index)
    {
        if (index < 0 || index >= c_base64Length)
        {
            throw new ArgumentOutOfRangeException(nameof(index), index, $"The index must be between 0 and {c_base64Length - 1}.");
        }

        return s_base64Chars[index];
    }

    /// <summary>
    /// Decodes a Base64 encoded string representing an IP address and port number.
    /// </summary>
    /// <param name="value">The encoded string representing an IP address and port number.</param>
    /// <returns>The decoded IP address and port in the format "IP:Port".</returns>
    /// <exception cref="ArgumentException">Thrown if the encoded string is not valid.</exception>
    public static string DecodeIp(ReadOnlySpan<char> value)
    {
        const int length = 11;
        const int ipLength = 8;

        if (value.Length != length)
        {
            throw new ArgumentException($"The encoded IP must be {length} characters long.", nameof(value));
        }

        if (!IsBase64(value))
        {
            throw new ArgumentException($"The encoded IP '{value}' contains invalid base64 characters.", nameof(value));
        }

        var ipCrypt = value[..ipLength];
        var portCrypt = value[ipLength..];

        Span<byte> ip = stackalloc byte[4];
        var ipIndex = 0;
        for (var i = 0; i < ipLength - 1; i++)
        {
            var d1 = ipCrypt[i++] - 48;
            var d2 = ipCrypt[i] - 48;

            ip[ipIndex++] = (byte)((d1 & 15) << 4 | d2 & 15);
        }

        var port = Decode64(portCrypt[0]) * c_base64Length * c_base64Length;
        port += Decode64(portCrypt[1]) * c_base64Length;
        port += Decode64(portCrypt[2]);

        return $"{ip[0]}.{ip[1]}.{ip[2]}.{ip[3]}:{port}";
    }

    /// <summary>
    /// Decodes a Base64 encoded string representing placement cells.
    /// </summary>
    /// <param name="value">The encoded string representing placement cells.</param>
    /// <returns>The list of decoded placement cells.</returns>
    /// <exception cref="ArgumentException">Thrown if the encoded string is not valid.</exception>"
    public static ReadOnlyCollection<int> DecodeMapPlacement(ReadOnlySpan<char> value)
    {
        if (value.IsEmpty)
        {
            return ReadOnlyCollection<int>.Empty;
        }

        if (!IsBase64(value))
        {
            throw new ArgumentException($"The encoded placement '{value}' contain invalid base64 characters.", nameof(value));
        }

        List<int> cells = new(value.Length / 2);

        for (var i = 0; i < value.Length - 1; i++)
        {
            cells.Add(Decode64(value[i++]) << 6 | Decode64(value[i]));
        }

        return cells.AsReadOnly();
    }
}
