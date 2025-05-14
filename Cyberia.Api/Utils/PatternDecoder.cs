namespace Cyberia.Api.Utils;

/// <summary>
/// Provides methods for encoding and decoding strings.
/// </summary>
public static class PatternDecoder
{
    private static readonly char[] s_hash =
    [
        'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '-', '_'
    ];

    /// <summary>
    /// Converts an encoded string to its corresponding IP address and port.
    /// </summary>
    /// <param name="value">The encoded string representing an IP address and port number.</param>
    /// <returns>The decoded IP address and port in the format "IP:Port".</returns>
    /// <exception cref="ArgumentException">Thrown if the encoded string is not exactly 11 characters long.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the last 3 digits in the encoded string contains invalid Base64 characters.</exception>
    public static string Ip(ReadOnlySpan<char> value)
    {
        const int length = 11;
        const int ipLength = 8;
        const int portLength = length - ipLength;

        if (value.Length != length)
        {
            throw new ArgumentException("The encoded IP must be 11 characters long");
        }

        var ipCrypt = value[..ipLength];
        var portCrypt = value[ipLength..];

        Span<byte> ipParts = stackalloc byte[4];
        var ipIndex = 0;
        for (var i = 0; i < ipLength - 1; i++)
        {
            var d1 = ipCrypt[i++] - 48;
            var d2 = ipCrypt[i] - 48;

            ipParts[ipIndex++] = (byte)((d1 & 15) << 4 | d2 & 15);
        }

        var port = 0;
        for (var i = 0; i < portLength; i++)
        {
            port += (int)Math.Pow(s_hash.Length, 2 - i) * CharToBase64Index(portCrypt[i]);
        }

        return $"{ipParts[0]}.{ipParts[1]}.{ipParts[2]}.{ipParts[3]}:{port}";
    }

    /// <summary>
    /// Converts a Base64 character to its corresponding index in the Base64 encoding table.
    /// </summary>
    /// <param name="value">The Base64 character to convert.</param>
    /// <returns>The index of the character in the Base64 encoding table.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the character is not a valid Base64 character.</exception>
    public static int CharToBase64Index(char value)
    {
        var index = Array.IndexOf(s_hash, value);

        if (index == -1)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Character is not a valid base64 character.");
        }

        return index;
    }

    /// <summary>
    /// Converts an index in the Base64 encoding table to its corresponding Base64 character.
    /// </summary>
    /// <param name="index">The index to convert.</param>
    /// <returns>The Base64 character corresponding to the given index.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the index is outside the range of the Base64 encoding table.</exception>
    public static char Base64IndexToChar(int index)
    {
        if (index < 0 || index >= s_hash.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Index must be within the range of the base64 characters.");
        }

        return s_hash[index];
    }
}
