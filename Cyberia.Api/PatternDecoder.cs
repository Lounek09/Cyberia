using System.Text;

namespace Cyberia.Api;

public static class PatternDecoder
{
    private static readonly char[] HASH =
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
        return Array.IndexOf(HASH, value);
    }

    public static string Description<T>(string value, T parameter)
    {
        return Description(value,
            parameter?.ToString() ?? string.Empty);
    }

    public static string Description<T0, T1>(string value, T0 parameter0, T1 parameter1)
    {
        return Description(value,
            parameter0?.ToString() ?? string.Empty,
            parameter1?.ToString() ?? string.Empty);
    }

    public static string Description<T0, T1, T2>(string value, T0 parameter0, T1 parameter1, T2 parameter2)
    {
        return Description(value,
            parameter0?.ToString() ?? string.Empty,
            parameter1?.ToString() ?? string.Empty,
            parameter2?.ToString() ?? string.Empty);
    }

    public static string Description(string value, params object?[] parameters)
    {
        return Description(value, Array.ConvertAll(parameters, x => x?.ToString() ?? string.Empty));
    }

    public static string Description(string value, params string[] parameters)
    {
        StringBuilder builder = new(value);

        for (var i = 0; i < parameters.Length; i++)
        {
            builder.Replace($"#{i + 1}", parameters[i]);
        }

        var indexOfOpenBrace = value.IndexOf('{');
        while (indexOfOpenBrace != -1)
        {
            var indexOfCloseBrace = value.IndexOf('}', indexOfOpenBrace);
            if (indexOfCloseBrace == -1)
            {
                break;
            }

            var replacement = value[(indexOfOpenBrace + 1)..indexOfCloseBrace];
            for (var i = 0; i < parameters.Length; i++)
            {
                if (!value[(indexOfOpenBrace + 1)..indexOfCloseBrace].Contains($"~{i + 1}"))
                {
                    continue;
                }

                if (string.IsNullOrEmpty(parameters[i]))
                {
                    replacement = string.Empty;
                    break;
                }

                replacement = replacement.Replace($"~{i + 1}", string.Empty);
            }

            if (replacement.Contains('~'))
            {
                replacement = string.Empty;
            }

            builder.Replace(value[indexOfOpenBrace..(indexOfCloseBrace + 1)], replacement);

            indexOfOpenBrace = value.IndexOf('{', indexOfCloseBrace);
        }

        return builder.ToString();
    }
}
