using Cyberia.Salamandra.Commands;
using Cyberia.Salamandra.Commands.Data;
using Cyberia.Salamandra.Commands.Dofus;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

using System.Text;
using System.Text.RegularExpressions;

namespace Cyberia.Salamandra.Managers;

public static partial class InteractionManager
{
    public const char PACKET_PARAMETER_SEPARATOR = '|';

    private static readonly Dictionary<string, Func<int, string[], ICustomMessageBuilder?>> _factory = new()
    {
        { BreedMessageBuilder.PACKET_HEADER, BreedMessageBuilder.Create },
        { CraftMessageBuilder.PACKET_HEADER, CraftMessageBuilder.Create },
        { PaginatedCraftMessageBuilder.PACKET_HEADER, PaginatedCraftMessageBuilder.Create },
        { HouseMessageBuilder.PACKET_HEADER, HouseMessageBuilder.Create },
        { PaginatedHouseMessageBuilder.PACKET_HEADER, PaginatedHouseMessageBuilder.Create },
        { IncarnationMessageBuilder.PACKET_HEADER, IncarnationMessageBuilder.Create },
        { PaginatedIncarnationMessageBuilder.PACKET_HEADER, PaginatedIncarnationMessageBuilder.Create },
        { ItemMessageBuilder.PACKET_HEADER, ItemMessageBuilder.Create },
        { PaginatedItemMessageBuilder.PACKET_HEADER, PaginatedItemMessageBuilder.Create },
        { ItemSetMessageBuilder.PACKET_HEADER, ItemSetMessageBuilder.Create },
        { PaginatedItemSetMessageBuilder.PACKET_HEADER, PaginatedItemSetMessageBuilder.Create },
        { LangsMessageBuilder.PACKET_HEADER, LangsMessageBuilder.Create },
        { MapMessageBuilder.PACKET_HEADER, MapMessageBuilder.Create },
        { PaginatedMapMessageBuilder.PACKET_HEADER, PaginatedMapMessageBuilder.Create },
        { PaginatedMapSubAreaMessageBuilder.PACKET_HEADER, PaginatedMapSubAreaMessageBuilder.Create },
        { PaginatedMapAreaMessageBuilder.PACKET_HEADER, PaginatedMapAreaMessageBuilder.Create },
        { MonsterMessageBuilder.PACKET_HEADER, MonsterMessageBuilder.Create },
        { PaginatedMonsterMessageBuilder.PACKET_HEADER, PaginatedMonsterMessageBuilder.Create },
        { QuestMessageBuilder.PACKET_HEADER, QuestMessageBuilder.Create },
        { PaginatedQuestMessageBuilder.PACKET_HEADER, PaginatedQuestMessageBuilder.Create },
        { RuneItemMessageBuilder.PACKET_HEADER, RuneItemMessageBuilder.Create },
        { PaginatedRuneItemMessageBuilder.PACKET_HEADER, PaginatedRuneItemMessageBuilder.Create },
        { SpellMessageBuilder.PACKET_HEADER, SpellMessageBuilder.Create },
        { PaginatedSpellMessageBuilder.PACKET_HEADER, PaginatedSpellMessageBuilder.Create }
    };

    [GeneratedRegex(@"SELECT\d+", RegexOptions.Compiled)]
    private static partial Regex SelectComponentPacketRegex();

    public static string ComponentPacketBuilder(string header, int version, params object[] parameters)
    {
        StringBuilder packetBuilder = new();

        packetBuilder.Append(header);
        packetBuilder.Append(PACKET_PARAMETER_SEPARATOR);
        packetBuilder.Append(version);

        if (parameters.Length > 0)
        {
            packetBuilder.Append(PACKET_PARAMETER_SEPARATOR);
            packetBuilder.Append(string.Join(PACKET_PARAMETER_SEPARATOR, parameters));
        }

        return packetBuilder.ToString();
    }

    public static string SelectComponentPacketBuilder(int uniqueIndex)
    {
        return $"SELECT{uniqueIndex}";
    }

    public static async Task OnComponentInteractionCreated(DiscordClient _, ComponentInteractionCreateEventArgs e)
    {
        if (e.User.IsBot || string.IsNullOrEmpty(e.Id))
        {
            return;
        }

        var response = new DiscordInteractionResponseBuilder();

        var decomposedPacket = (SelectComponentPacketRegex().IsMatch(e.Id) ? e.Values[0] : e.Id).Split(PACKET_PARAMETER_SEPARATOR);

        if (decomposedPacket.Length < 2)
        {
            response.WithContent("Le message est trop ancien").AsEphemeral();
            await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, response);
            return;
        }

        var header = decomposedPacket[0];
        var version = int.Parse(decomposedPacket[1]);
        var parameters = decomposedPacket.Length > 2 ? decomposedPacket[2..] : [];

        if (!_factory.TryGetValue(header, out var builder))
        {
            response.WithContent("Bouton inconnu, le message est probablement trop ancien").AsEphemeral();
            await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, response);
            return;
        }

        var message = builder(version, parameters);
        if (message is null)
        {
            response.WithContent("Un problème a eu lieu lors de la création de la réponse, le message est probablement trop ancien").AsEphemeral();
            await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, response);
            return;
        }

        response = await message.GetMessageAsync<DiscordInteractionResponseBuilder>();
        await e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, response);
    }
}
