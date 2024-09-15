using System.Text;

namespace Cyberia.Salamandra.Managers;

public static class PacketManager
{
    public const char ParameterSeparator = '|';

    public static string ComponentBuilder(string header, int version, params object[] parameters)
    {
        StringBuilder packetBuilder = new();

        packetBuilder.Append(header);
        packetBuilder.Append(ParameterSeparator);
        packetBuilder.Append(version);

        if (parameters.Length > 0)
        {
            packetBuilder.Append(ParameterSeparator);
            packetBuilder.Append(string.Join(ParameterSeparator, parameters));
        }

        return packetBuilder.ToString();
    }

    public static string SelectComponentBuilder(int uniqueIndex)
    {
        return $"SELECT{uniqueIndex}";
    }
}

