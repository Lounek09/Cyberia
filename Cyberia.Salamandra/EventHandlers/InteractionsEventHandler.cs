﻿using Cyberia.Salamandra.Commands;
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
using System.Globalization;
using System.Text.RegularExpressions;

namespace Cyberia.Salamandra.EventHandlers;

/// <summary>
/// Represents the handler for events related to interactions.
/// </summary>
public sealed partial class InteractionsEventHandler : IEventHandler<ComponentInteractionCreatedEventArgs>
{
    private static readonly FrozenDictionary<string, Func<IServiceProvider, int, CultureInfo?, string[], ICustomMessageBuilder?>> s_factory = new Dictionary<string, Func<IServiceProvider, int, CultureInfo?, string[], ICustomMessageBuilder?>>()
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

    private readonly IServiceProvider _serviceProvider;
    private readonly CultureService _cultureService;

    /// <summary>
    /// Initializes a new instance of the <see cref="InteractionsEventHandler"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider to get the services from.</param>
    /// <param name="cultureService">The service to get the culture from.</param>
    public InteractionsEventHandler(IServiceProvider serviceProvider, CultureService cultureService)
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

        var response = new DiscordInteractionResponseBuilder().AsEphemeral();

        var decomposedPacket = (SelectComponentPacketRegex().IsMatch(eventArgs.Id) ? eventArgs.Values[0] : eventArgs.Id)
            .Split(PacketFormatter.Separator, StringSplitOptions.RemoveEmptyEntries);

        if (decomposedPacket.Length < 2)
        {
            response.WithContent(Translation.Get<BotTranslations>("Command.Error.Component", culture));
            await interaction.CreateResponseAsync(DiscordInteractionResponseType.ChannelMessageWithSource, response);
            return;
        }

        var header = decomposedPacket[0];

        if (!s_factory.TryGetValue(header, out var builder))
        {
            response.WithContent(Translation.Get<BotTranslations>("Command.Error.Component", culture));
            await interaction.CreateResponseAsync(DiscordInteractionResponseType.ChannelMessageWithSource, response);
            return;
        }

        var version = int.Parse(decomposedPacket[1]);
        var parameters = decomposedPacket.Length > 2 ? decomposedPacket[2..] : [];

        var message = builder(_serviceProvider, version, culture, parameters);
        if (message is null)
        {
            response.WithContent(Translation.Get<BotTranslations>("Command.Error.Component", culture));
            await interaction.CreateResponseAsync(DiscordInteractionResponseType.ChannelMessageWithSource, response);
            return;
        }

        response = await message.BuildAsync<DiscordInteractionResponseBuilder>();
        await interaction.CreateResponseAsync(DiscordInteractionResponseType.UpdateMessage, response);
    }
}
