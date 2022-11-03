namespace Salamandra.Api
{
    public static class PatternDecoder
    {
        public static readonly char[] HASH = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
                                               'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
                                               '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '-', '_' };

        public static string DecodeIp(string value)
        {
            if (value.Length == 11)
            {
                string ipCrypt = value[..8];
                string portCrypt = value.Substring(8, 3);

                string ip = "";
                int d1, d2;
                for (int i = 0; i < 8; i++)
                {
                    d1 = ipCrypt[i] - 48;
                    i++;
                    d2 = ipCrypt[i] - 48;
                    ip += (d1 & 15) << 4 | d2 & 15;
                    if (i < 7)
                        ip += ".";
                }

                double port = 0;
                for (int i = 0; i < 3; i++)
                    port += Math.Pow(64, 2 - i) * Array.FindIndex(HASH, e => e == portCrypt[i]);

                return ip + ":" + port;
            }
            else
                throw new ArgumentException("L'ip encodée doit faire 11 caractères");
        }

        public static int Decode64(char value)
        {
            return Array.IndexOf(HASH, value);
        }

        public static string DecodeDescription(string description, params string[] parameters)
        {
            if (description.Contains('{') && description.Contains('}'))
            {
                int a = -1, b = -1;
                for (int i = 0; i < parameters.Length; i++)
                {
                    if (description.Contains("~" + (i + 1)))
                    {
                        if (a == -1)
                            a = i;
                        else
                            b = i;
                    }
                }

                if (string.IsNullOrEmpty(parameters[a]) || string.IsNullOrEmpty(parameters[b]) || parameters[a] == parameters[b])
                {
                    int length = description.Split("{")[1].Split("}")[0].Length;
                    description = description.Replace("{" + description.Split("{")[1][..length] + "}", "");
                    description = description.Replace("#" + (b + 1), "");
                }
                else
                {
                    description = description.Replace("{" + description.Split("{")[1][..4], "");
                    description = description.Replace("}", "");
                }
            }

            for (int i = 0; i < parameters.Length; i++)
                description = description.Replace("#" + (i + 1), parameters[i]);

            return description;
        }
    }
}
