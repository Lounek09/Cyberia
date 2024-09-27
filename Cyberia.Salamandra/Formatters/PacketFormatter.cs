using System.Text;

namespace Cyberia.Salamandra.Formatters;

/// <summary>
/// Provides utility methods for formatting packets.
/// </summary>
public static class PacketFormatter
{
    /// <summary>
    /// The separator character for packets.
    /// </summary>
    public const char Separator = '|';

    /// <summary>
    /// Creates a packet that represents an action.
    /// </summary>
    /// <param name="header">The header of the packet.</param>
    /// <param name="version">The version of the packet.</param>
    /// <param name="parameters">The parameters of the packet.</param>
    /// <returns>The formatted packet as a string.</returns>
    public static string Action(string header, int version, params object[] parameters)
    {
        StringBuilder packetBuilder = new();

        packetBuilder.Append(header);
        packetBuilder.Append(Separator);
        packetBuilder.Append(version);

        if (parameters.Length > 0)
        {
            packetBuilder.Append(Separator);
            packetBuilder.Append(string.Join(Separator, parameters));
        }

        return packetBuilder.ToString();
    }

    /// <summary>
    /// Creates a packet that represents a select component.
    /// </summary>
    /// <param name="uniqueIndex">The unique index of the select component.</param>
    /// <returns>The formatted packet as a string.</returns>
    public static string Select(int uniqueIndex)
    {
        return $"SELECT{uniqueIndex}";
    }
}

