using Cyberia.Salamandra.Commands;
using Cyberia.Salamandra.Commands.Data.Langs;
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
using Cyberia.Salamandra.Formatters;
using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Cyberia.Salamandra.EventHandlers;

/// <summary>
/// Represents the handler for events related to interactions.
/// </summary>
public sealed partial class InteractionsEventHandler : IEventHandler<ComponentInteractionCreatedEventArgs>
{

    /// <summary>
    /// A dictionary mapping the packet header to the factory method to create the message.
    /// </summary>
    private static readonly FrozenDictionary<string, Func<IServiceProvider, int, CultureInfo?, ReadOnlySpan<string>, ICustomMessageBuilder?>> s_factories = new Dictionary<string, Func<IServiceProvider, int, CultureInfo?, ReadOnlySpan<string>, ICustomMessageBuilder?>>()
    {
        { BreedMessageBuilder.PacketHeader, BreedMessageBuilder.Create },
        { GladiatroolBreedMessageBuilder.PacketHeader, GladiatroolBreedMessageBuilder.Create },
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

    private static readonly FrozenDictionary<string, Func<IServiceProvider, int, CultureInfo?, ReadOnlySpan<string>, ICustomMessageBuilder?>>
        .AlternateLookup<ReadOnlySpan<char>> s_factoriesLookup = s_factories.GetAlternateLookup<ReadOnlySpan<char>>();

    [GeneratedRegex(@$"{PacketFormatter.SelectComponentHeader}\d+", RegexOptions.Compiled)]
    private static partial Regex SelectComponentPacketRegex();

    private readonly IServiceProvider _serviceProvider;
    private readonly ICultureService _cultureService;

    /// <summary>
    /// Initializes a new instance of the <see cref="InteractionsEventHandler"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider to get the services from.</param>
    /// <param name="cultureService">The service to get the culture from.</param>
    public InteractionsEventHandler(IServiceProvider serviceProvider, ICultureService cultureService)
    {
        _serviceProvider = serviceProvider;
        _cultureService = cultureService;
    }

    public async Task HandleEventAsync(DiscordClient sender, ComponentInteractionCreatedEventArgs eventArgs)
    {
        if (eventArgs.User.IsBot || string.IsNullOrEmpty(eventArgs.Id))
        {
            return;
        }

        var interaction = eventArgs.Interaction;
        var culture = await _cultureService.GetCultureAsync(interaction);

        ReadOnlySpan<char> packet = SelectComponentPacketRegex().IsMatch(eventArgs.Id) ? eventArgs.Values[0] : eventArgs.Id;
        if (!TryParsePacket(packet, out var builder, out var version, out var parameters))
        {
            await SendErrorResponse(interaction, culture);
            return;
        }

        var message = builder(_serviceProvider, version, culture, parameters);
        if (message is null)
        {
            await SendErrorResponse(interaction, culture);
            return;
        }

        var response = await message.BuildAsync<DiscordInteractionResponseBuilder>();
        await interaction.CreateResponseAsync(DiscordInteractionResponseType.UpdateMessage, response);
    }

    /// <summary>
    /// Tries to parse the interaction packet.
    /// </summary>
    /// <param name="packet">The packet to parse.</param>   
    /// <param name="builder">The builder to use to create the message.</param>
    /// <param name="version">The version of the packet.</param>
    /// <param name="parameters">The parameters of the packet.</param>
    /// <returns><see langword="true"/> if the packet was parsed successfully; otherwise, <see langword="false"/>.</returns>
    private static bool TryParsePacket(
        ReadOnlySpan<char> packet,
        [NotNullWhen(true)] out Func<IServiceProvider, int, CultureInfo?, ReadOnlySpan<string>, ICustomMessageBuilder?>? builder,
        out int version,
        out Span<string> parameters)
    {
        builder = null;
        version = 0;
        parameters = Span<string>.Empty;

        var indexOfSeparator = packet.IndexOf(PacketFormatter.Separator);
        if (indexOfSeparator == -1)
        {
            return false;
        }

        var header = packet[..indexOfSeparator];
        if (!s_factoriesLookup.TryGetValue(header, out builder))
        {
            return false;
        }

        packet = packet[(indexOfSeparator + 1)..];

        indexOfSeparator = packet.IndexOf(PacketFormatter.Separator);
        if (indexOfSeparator == -1 || !int.TryParse(packet[..indexOfSeparator], out version))
        {
            return false;
        }

        packet = packet[(indexOfSeparator + 1)..];

        if (!packet.IsEmpty)
        {
            var parameterCount = packet.Count(PacketFormatter.Separator) + 1;
            parameters = new string[parameterCount];

            var index = 0;
            foreach (var range in packet.Split(PacketFormatter.Separator))
            {
                parameters[index++] = packet[range].ToString();
            }
        }

        return true;
    }

    /// <summary>
    /// Sends an error response when the interaction packet is invalid.
    /// </summary>
    /// <param name="interaction">The interaction to respond to.</param>
    /// <param name="culture">The culture to use for the response.</param>
    private static async Task SendErrorResponse(DiscordInteraction interaction, CultureInfo culture)
    {
        var response = new DiscordInteractionResponseBuilder()
            .WithContent(Translation.Get<BotTranslations>("Command.Error.Component", culture))
            .AsEphemeral();

        await interaction.CreateResponseAsync(DiscordInteractionResponseType.ChannelMessageWithSource, response);
    }
}
