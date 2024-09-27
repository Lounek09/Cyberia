using Cyberia.Api;
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
    private readonly CultureService _cultureService;
    private readonly EmbedBuilderService _embedBuilderService;

    public HouseCommandModule(CultureService cultureService, EmbedBuilderService embedBuilderService)
    {
        _cultureService = cultureService;
        _embedBuilderService = embedBuilderService;
    }

    [Command(HouseInteractionLocalizer.Name_CommandName), Description(HouseInteractionLocalizer.Name_CommandDescription)]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [InteractionLocalizer<HouseInteractionLocalizer>]
    public async Task NameExecuteAsync(SlashCommandContext ctx,
        [Parameter(HouseInteractionLocalizer.Name_Value_ParameterName), Description(HouseInteractionLocalizer.Name_Value_ParameterDescription)]
        [InteractionLocalizer<HouseInteractionLocalizer>]
        [SlashAutoCompleteProvider<HouseAutocompleteProvider>]
        [MinMaxLength(1, 70)]
        string value)
    {
        using CultureScope scope = new(await _cultureService.GetCultureAsync(ctx.Interaction));

        DiscordInteractionResponseBuilder? response = null;

        if (int.TryParse(value, out var id))
        {
            var houseData = DofusApi.Datacenter.HousesRepository.GetHouseDataById(id);
            if (houseData is not null)
            {
                response = await new HouseMessageBuilder(_embedBuilderService, houseData).BuildAsync<DiscordInteractionResponseBuilder>();
            }
        }
        else
        {
            var housesData = DofusApi.Datacenter.HousesRepository.GetHousesDataByName(value).ToList();
            if (housesData.Count == 1)
            {
                response = await new HouseMessageBuilder(_embedBuilderService, housesData[0]).BuildAsync<DiscordInteractionResponseBuilder>();
            }
            else if (housesData.Count > 1)
            {
                response = await new PaginatedHouseMessageBuilder(_embedBuilderService, housesData, HouseSearchCategory.Name, value).BuildAsync<DiscordInteractionResponseBuilder>();
            }
        }

        response ??= new DiscordInteractionResponseBuilder().WithContent(BotTranslations.House_NotFound);
        await ctx.RespondAsync(response);
    }


    [Command(HouseInteractionLocalizer.Coordinates_CommandName), Description(HouseInteractionLocalizer.Coordinates_CommandDescription)]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [InteractionLocalizer<HouseInteractionLocalizer>]
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
        using CultureScope scope = new(await _cultureService.GetCultureAsync(ctx.Interaction));

        var housesData = DofusApi.Datacenter.HousesRepository.GetHousesDataByCoordinate(x, y).ToList();

        if (housesData.Count == 0)
        {
            await ctx.RespondAsync(Translation.Format(BotTranslations.House_NotFound_Coordinate, x, y));
        }
        else if (housesData.Count == 1)
        {
            await ctx.RespondAsync(await new HouseMessageBuilder(_embedBuilderService, housesData[0]).BuildAsync<DiscordInteractionResponseBuilder>());
        }
        else
        {
            await ctx.RespondAsync(await new PaginatedHouseMessageBuilder(_embedBuilderService, housesData, HouseSearchCategory.Coordinate, $"{x}{PacketFormatter.Separator}{y}")
                .BuildAsync<DiscordInteractionResponseBuilder>());
        }
    }


    [Command(HouseInteractionLocalizer.SubArea_CommandName), Description(HouseInteractionLocalizer.SubArea_CommandDescription)]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [InteractionLocalizer<HouseInteractionLocalizer>]
    public async Task MapSubAreaExecuteAsync(SlashCommandContext ctx,
        [Parameter(HouseInteractionLocalizer.SubArea_Value_ParameterName), Description(HouseInteractionLocalizer.SubArea_Value_ParameterDescription)]
        [InteractionLocalizer<HouseInteractionLocalizer>]
        [SlashAutoCompleteProvider<MapSubAreaAutocompleteProvider>]
        [MinMaxLength(1, 70)]
        string value)
    {
        using CultureScope scope = new(await _cultureService.GetCultureAsync(ctx.Interaction));

        MapSubAreaData? mapSubAreaData = null;

        if (int.TryParse(value, out var id))
        {
            mapSubAreaData = DofusApi.Datacenter.MapsRepository.GetMapSubAreaDataById(id);
        }

        if (mapSubAreaData is null)
        {
            await ctx.RespondAsync(BotTranslations.MapSubArea_NotFound);
        }
        else
        {
            var housesData = DofusApi.Datacenter.HousesRepository.GetHousesDataByMapSubAreaId(id).ToList();

            if (housesData.Count == 0)
            {
                await ctx.RespondAsync(Translation.Format(BotTranslations.House_NotFound_MapSubArea, Formatter.Bold(mapSubAreaData.Name)));
            }
            else if (housesData.Count == 1)
            {
                await ctx.RespondAsync(await new HouseMessageBuilder(_embedBuilderService, housesData[0]).BuildAsync<DiscordInteractionResponseBuilder>());
            }
            else
            {
                await ctx.RespondAsync(await new PaginatedHouseMessageBuilder(_embedBuilderService, housesData, HouseSearchCategory.MapSubArea, value)
                    .BuildAsync<DiscordInteractionResponseBuilder>());
            }
        }
    }


    [Command(HouseInteractionLocalizer.Area_CommandName), Description(HouseInteractionLocalizer.Area_CommandDescription)]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [InteractionLocalizer<HouseInteractionLocalizer>]
    public async Task MapAreaExecuteAsync(SlashCommandContext ctx,
        [Parameter(HouseInteractionLocalizer.Area_Value_ParameterName), Description(HouseInteractionLocalizer.Area_Value_ParameterDescription)]
        [InteractionLocalizer<HouseInteractionLocalizer>]
        [SlashAutoCompleteProvider<MapAreaAutocompleteProvider>]
        [MinMaxLength(1, 70)]
        string value)
    {
        using CultureScope scope = new(await _cultureService.GetCultureAsync(ctx.Interaction));

        MapAreaData? mapAreaData = null;

        if (int.TryParse(value, out var id))
        {
            mapAreaData = DofusApi.Datacenter.MapsRepository.GetMapAreaDataById(id);
        }

        if (mapAreaData is null)
        {
            await ctx.RespondAsync(BotTranslations.MapArea_NotFound);
        }
        else
        {
            var housesData = DofusApi.Datacenter.HousesRepository.GetHousesDataByMapAreaId(id).ToList();

            if (housesData.Count == 0)
            {
                await ctx.RespondAsync(Translation.Format(BotTranslations.House_NotFound_MapArea, Formatter.Bold(mapAreaData.Name)));
            }
            else if (housesData.Count == 1)
            {
                await ctx.RespondAsync(await new HouseMessageBuilder(_embedBuilderService, housesData[0]).BuildAsync<DiscordInteractionResponseBuilder>());
            }
            else
            {
                await ctx.RespondAsync(await new PaginatedHouseMessageBuilder(_embedBuilderService, housesData, HouseSearchCategory.MapArea, value)
                    .BuildAsync<DiscordInteractionResponseBuilder>());
            }
        }
    }
}
