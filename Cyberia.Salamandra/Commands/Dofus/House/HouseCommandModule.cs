﻿using Cyberia.Api.Data;
using Cyberia.Api.Data.Maps;
using Cyberia.Salamandra.Commands.Dofus.Map;
using Cyberia.Salamandra.Formatters;
using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.ArgumentModifiers;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Commands.Processors.SlashCommands.Localization;
using DSharpPlus.Commands.Processors.SlashCommands.Metadata;
using DSharpPlus.Entities;

using System.ComponentModel;

namespace Cyberia.Salamandra.Commands.Dofus.House;

[Command(HouseInteractionLocalizer.CommandName), Description(HouseInteractionLocalizer.CommandDescription)]
[InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall, DiscordApplicationIntegrationType.UserInstall)]
[InteractionAllowedContexts(DiscordInteractionContextType.Guild, DiscordInteractionContextType.PrivateChannel)]
[InteractionLocalizer<HouseInteractionLocalizer>]
public sealed class HouseCommandModule
{
    private readonly ICultureService _cultureService;
    private readonly DofusDatacenter _dofusDatacenter;
    private readonly IEmbedBuilderService _embedBuilderService;

    public HouseCommandModule(ICultureService cultureService, DofusDatacenter dofusDatacenter, IEmbedBuilderService embedBuilderService)
    {
        _cultureService = cultureService;
        _dofusDatacenter = dofusDatacenter;
        _embedBuilderService = embedBuilderService;
    }

    [Command(HouseInteractionLocalizer.Name_CommandName), Description(HouseInteractionLocalizer.Name_CommandDescription)]
    [InteractionLocalizer<HouseInteractionLocalizer>]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public async Task NameExecuteAsync(SlashCommandContext ctx,
        [Parameter(HouseInteractionLocalizer.Name_Value_ParameterName), Description(HouseInteractionLocalizer.Name_Value_ParameterDescription)]
        [InteractionLocalizer<HouseInteractionLocalizer>]
        [SlashAutoCompleteProvider<HouseAutocompleteProvider>]
        [MinMaxLength(1, 70)]
        string value)
    {
        var culture = await _cultureService.GetCultureAsync(ctx.Interaction);

        DiscordInteractionResponseBuilder? response = null;

        if (int.TryParse(value, out var id))
        {
            var houseData = _dofusDatacenter.HousesRepository.GetHouseDataById(id);
            if (houseData is not null)
            {
                response = await new HouseMessageBuilder(_embedBuilderService, houseData, culture)
                    .BuildAsync<DiscordInteractionResponseBuilder>();
            }
        }
        else
        {
            var housesData = _dofusDatacenter.HousesRepository.GetHousesDataByName(value, culture).ToList();
            if (housesData.Count == 1)
            {
                response = await new HouseMessageBuilder(_embedBuilderService, housesData[0], culture)
                    .BuildAsync<DiscordInteractionResponseBuilder>();
            }
            else if (housesData.Count > 1)
            {
                response = await new PaginatedHouseMessageBuilder(_embedBuilderService, housesData, HouseSearchCategory.Name, value, culture)
                    .BuildAsync<DiscordInteractionResponseBuilder>();
            }
        }

        response ??= new DiscordInteractionResponseBuilder().WithContent(Translation.Get<BotTranslations>("House.NotFound", culture));
        await ctx.RespondAsync(response);
    }


    [Command(HouseInteractionLocalizer.Coordinates_CommandName), Description(HouseInteractionLocalizer.Coordinates_CommandDescription)]
    [InteractionLocalizer<HouseInteractionLocalizer>]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public async Task CoordinateExecuteAsync(SlashCommandContext ctx,
        [Parameter(HouseInteractionLocalizer.Coordinates_X_ParameterName), Description(HouseInteractionLocalizer.Coordinates_X_ParameterDescription)]
        [InteractionLocalizer<HouseInteractionLocalizer>]
        [MinMaxValue(-666, 666)]
        int x,
        [Parameter(HouseInteractionLocalizer.Coordinates_Y_ParameterName), Description(HouseInteractionLocalizer.Coordinates_Y_ParameterDescription)]
        [InteractionLocalizer<HouseInteractionLocalizer>]
        [MinMaxValue(-666, 666)]
        int y)
    {
        var culture = await _cultureService.GetCultureAsync(ctx.Interaction);

        var housesData = _dofusDatacenter.HousesRepository.GetHousesDataByCoordinate(x, y).ToList();

        if (housesData.Count == 0)
        {
            await ctx.RespondAsync(Translation.Format(Translation.Get<BotTranslations>("House.NotFound.Coordinate", culture), x, y));
        }
        else if (housesData.Count == 1)
        {
            await ctx.RespondAsync(await new HouseMessageBuilder(_embedBuilderService, housesData[0], culture)
                .BuildAsync<DiscordInteractionResponseBuilder>());
        }
        else
        {
            await ctx.RespondAsync(await new PaginatedHouseMessageBuilder(_embedBuilderService, housesData, HouseSearchCategory.Coordinate, $"{x}{PacketFormatter.Separator}{y}", culture)
                .BuildAsync<DiscordInteractionResponseBuilder>());
        }
    }


    [Command(HouseInteractionLocalizer.SubArea_CommandName), Description(HouseInteractionLocalizer.SubArea_CommandDescription)]
    [InteractionLocalizer<HouseInteractionLocalizer>]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public async Task MapSubAreaExecuteAsync(SlashCommandContext ctx,
        [Parameter(HouseInteractionLocalizer.SubArea_Value_ParameterName), Description(HouseInteractionLocalizer.SubArea_Value_ParameterDescription)]
        [InteractionLocalizer<HouseInteractionLocalizer>]
        [SlashAutoCompleteProvider<MapSubAreaAutocompleteProvider>]
        [MinMaxLength(1, 70)]
        string value)
    {
        var culture = await _cultureService.GetCultureAsync(ctx.Interaction);

        MapSubAreaData? mapSubAreaData = null;

        if (int.TryParse(value, out var id))
        {
            mapSubAreaData = _dofusDatacenter.MapsRepository.GetMapSubAreaDataById(id);
        }

        if (mapSubAreaData is null)
        {
            await ctx.RespondAsync(Translation.Get<BotTranslations>("MapSubArea.NotFound", culture));
        }
        else
        {
            var housesData = _dofusDatacenter.HousesRepository.GetHousesDataByMapSubAreaId(id).ToList();

            if (housesData.Count == 0)
            {
                await ctx.RespondAsync(Translation.Format(Translation.Get<BotTranslations>("House.NotFound.MapSubArea", culture), Formatter.Bold(mapSubAreaData.Name)));
            }
            else if (housesData.Count == 1)
            {
                await ctx.RespondAsync(await new HouseMessageBuilder(_embedBuilderService, housesData[0], culture)
                    .BuildAsync<DiscordInteractionResponseBuilder>());
            }
            else
            {
                await ctx.RespondAsync(await new PaginatedHouseMessageBuilder(_embedBuilderService, housesData, HouseSearchCategory.MapSubArea, value, culture)
                    .BuildAsync<DiscordInteractionResponseBuilder>());
            }
        }
    }


    [Command(HouseInteractionLocalizer.Area_CommandName), Description(HouseInteractionLocalizer.Area_CommandDescription)]
    [InteractionLocalizer<HouseInteractionLocalizer>]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public async Task MapAreaExecuteAsync(SlashCommandContext ctx,
        [Parameter(HouseInteractionLocalizer.Area_Value_ParameterName), Description(HouseInteractionLocalizer.Area_Value_ParameterDescription)]
        [InteractionLocalizer<HouseInteractionLocalizer>]
        [SlashAutoCompleteProvider<MapAreaAutocompleteProvider>]
        [MinMaxLength(1, 70)]
        string value)
    {
        var culture = await _cultureService.GetCultureAsync(ctx.Interaction);

        MapAreaData? mapAreaData = null;

        if (int.TryParse(value, out var id))
        {
            mapAreaData = _dofusDatacenter.MapsRepository.GetMapAreaDataById(id);
        }

        if (mapAreaData is null)
        {
            await ctx.RespondAsync(Translation.Get<BotTranslations>("MapArea.NotFound", culture));
        }
        else
        {
            var housesData = _dofusDatacenter.HousesRepository.GetHousesDataByMapAreaId(id).ToList();

            if (housesData.Count == 0)
            {
                await ctx.RespondAsync(Translation.Format(Translation.Get<BotTranslations>("House.NotFound.MapArea", culture), Formatter.Bold(mapAreaData.Name)));
            }
            else if (housesData.Count == 1)
            {
                await ctx.RespondAsync(await new HouseMessageBuilder(_embedBuilderService, housesData[0], culture)
                    .BuildAsync<DiscordInteractionResponseBuilder>());
            }
            else
            {
                await ctx.RespondAsync(await new PaginatedHouseMessageBuilder(_embedBuilderService, housesData, HouseSearchCategory.MapArea, value, culture)
                    .BuildAsync<DiscordInteractionResponseBuilder>());
            }
        }
    }
}
