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
    public const char PacketParameterSeparator = '|';

    private static readonly Dictionary<string, Func<int, string[], ICustomMessageBuilder?>> _factory = new()
    {
        { BreedMessageBuilder.PacketHeader, BreedMessageBuilder.Create },
        { CraftMessageBuilder.PacketHeader, CraftMessageBuilder.Create },
        { PaginatedCraftMessageBuilder.PacketHeader, PaginatedCraftMessageBuilder.Create },
        { HouseMessageBuilder.PacketHeader, HouseMessageBuilder.Create },
        { PaginatedHouseMessageBuilder.PacketHeader, PaginatedHouseMessageBuilder.Create },
        { IncarnationMessageBuilder.PacketHeader, IncarnationMessageBuilder.Create },
        { PaginatedIncarnationMessageBuilder.PacketHeader, PaginatedIncarnationMessageBuilder.Create },
        { ItemMessageBuilder.PacketHeader, ItemMessageBuilder.Create },
        { PaginatedItemMessageBuilder.PacketHeader, PaginatedItemMessageBuilder.Create },
        { ItemSetMessageBuilder.PacketHeader, ItemSetMessageBuilder.Create },
        { PaginatedItemSetMessageBuilder.PacketHeader, PaginatedItemSetMessageBuilder.Create },
        { LangsMessageBuilder.PacketHeader, LangsMessageBuilder.Create },
        { MapMessageBuilder.PacketHeader, MapMessageBuilder.Create },
        { PaginatedMapMessageBuilder.PacketHeader, PaginatedMapMessageBuilder.Create },
        { PaginatedMapSubAreaMessageBuilder.PacketHeader, PaginatedMapSubAreaMessageBuilder.Create },
        { PaginatedMapAreaMessageBuilder.PacketHeader, PaginatedMapAreaMessageBuilder.Create },
        { MonsterMessageBuilder.PacketHeader, MonsterMessageBuilder.Create },
        { PaginatedMonsterMessageBuilder.PacketHeader, PaginatedMonsterMessageBuilder.Create },
        { QuestMessageBuilder.PacketHeader, QuestMessageBuilder.Create },
        { PaginatedQuestMessageBuilder.PacketHeader, PaginatedQuestMessageBuilder.Create },
        { RuneItemMessageBuilder.PacketHeader, RuneItemMessageBuilder.Create },
        { PaginatedRuneItemMessageBuilder.PacketHeader, PaginatedRuneItemMessageBuilder.Create },
        { SpellMessageBuilder.PacketHeader, SpellMessageBuilder.Create },
        { PaginatedSpellMessageBuilder.PacketHeader, PaginatedSpellMessageBuilder.Create }
    };

    [GeneratedRegex(@"SELECT\d+", RegexOptions.Compiled)]
    private static partial Regex SelectComponentPacketRegex();

    public static string ComponentPacketBuilder(string header, int version, params object[] parameters)
    {
        StringBuilder packetBuilder = new();

        packetBuilder.Append(header);
        packetBuilder.Append(PacketParameterSeparator);
        packetBuilder.Append(version);

        if (parameters.Length > 0)
        {
            packetBuilder.Append(PacketParameterSeparator);
            packetBuilder.Append(string.Join(PacketParameterSeparator, parameters));
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

        var decomposedPacket = (SelectComponentPacketRegex().IsMatch(e.Id) ? e.Values[0] : e.Id)
            .Split(PacketParameterSeparator, StringSplitOptions.RemoveEmptyEntries);

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
