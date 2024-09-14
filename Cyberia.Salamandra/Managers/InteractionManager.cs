using Cyberia.Salamandra.Commands;
using Cyberia.Salamandra.Commands.Data.Cytrus;
using Cyberia.Salamandra.Commands.Dofus.Breed;
using Cyberia.Salamandra.Commands.Dofus.Craft;
using Cyberia.Salamandra.Commands.Dofus.House;
using Cyberia.Salamandra.Commands.Dofus.Incarnation;
using Cyberia.Salamandra.Commands.Dofus.Item;
using Cyberia.Salamandra.Commands.Dofus.ItemSet;
using Cyberia.Salamandra.Commands.Dofus.Map;
using Cyberia.Salamandra.Commands.Dofus.Monster;
using Cyberia.Salamandra.Commands.Dofus.Quest;
using Cyberia.Salamandra.Commands.Dofus.Rune;
using Cyberia.Salamandra.Commands.Dofus.Spell;
using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

using Microsoft.Extensions.DependencyInjection;

using System.Collections.Frozen;
using System.Text;
using System.Text.RegularExpressions;

namespace Cyberia.Salamandra.Managers;

public static partial class InteractionManager
{
    public const char PacketParameterSeparator = '|';

    private static readonly FrozenDictionary<string, Func<int, string[], ICustomMessageBuilder?>> s_factory = new Dictionary<string, Func<int, string[], ICustomMessageBuilder?>>()
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
    }.ToFrozenDictionary();

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

    public static async Task OnComponentInteractionCreated(DiscordClient client, ComponentInteractionCreatedEventArgs args)
    {
        if (args.User.IsBot || string.IsNullOrEmpty(args.Id))
        {
            return;
        }

        var cultureService = client.ServiceProvider.GetRequiredService<CultureService>();
        await cultureService.SetCultureAsync(args.Interaction);

        var response = new DiscordInteractionResponseBuilder().AsEphemeral();

        var decomposedPacket = (SelectComponentPacketRegex().IsMatch(args.Id) ? args.Values[0] : args.Id)
            .Split(PacketParameterSeparator, StringSplitOptions.RemoveEmptyEntries);

        if (decomposedPacket.Length < 2)
        {
            response.WithContent(BotTranslations.Command_Error_Component);
            await args.Interaction.CreateResponseAsync(DiscordInteractionResponseType.ChannelMessageWithSource, response);
            return;
        }

        var header = decomposedPacket[0];

        if (!s_factory.TryGetValue(header, out var builder))
        {
            response.WithContent(BotTranslations.Command_Error_Component);
            await args.Interaction.CreateResponseAsync(DiscordInteractionResponseType.ChannelMessageWithSource, response);
            return;
        }

        var version = int.Parse(decomposedPacket[1]);
        var parameters = decomposedPacket.Length > 2 ? decomposedPacket[2..] : [];

        var message = builder(version, parameters);
        if (message is null)
        {
            response.WithContent(BotTranslations.Command_Error_Component);
            await args.Interaction.CreateResponseAsync(DiscordInteractionResponseType.ChannelMessageWithSource, response);
            return;
        }

        response = await message.BuildAsync<DiscordInteractionResponseBuilder>();
        await args.Interaction.CreateResponseAsync(DiscordInteractionResponseType.UpdateMessage, response);
    }
}
