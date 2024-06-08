namespace Cyberia.Api;

public static class PatternDecoder
{
    private static readonly char[] s_hash =
    [
        'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '-', '_'
    ];

    public static string Ip(string value)
    {
        if (value.Length != 11)
        {
            throw new ArgumentException("The encoded IP must be 11 characters long");
        }

        var ipCrypt = value[..8];
        var portCrypt = value[8..];

        var ip = string.Empty;
        int d1, d2;
        for (var i = 0; i < ipCrypt.Length; i++)
        {
            d1 = ipCrypt[i] - 48;
            i++;
            d2 = ipCrypt[i] - 48;
            ip += (d1 & 15) << 4 | d2 & 15;
            if (i < ipCrypt.Length - 1)
            {
                ip += ".";
            }
        }

        var port = 0D;
        for (var i = 0; i < portCrypt.Length; i++)
        {
            port += Math.Pow(64, 2 - i) * Base64(portCrypt[i]);
        }

        return $"{ip}:{port}";
    }

    public static int Base64(char value)
    {
        return Array.IndexOf(s_hash, value);
    }
}
