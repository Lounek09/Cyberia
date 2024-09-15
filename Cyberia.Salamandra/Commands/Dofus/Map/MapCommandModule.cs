using Cyberia.Api;
using Cyberia.Api.Data.Maps;
using Cyberia.Salamandra.EventHandlers;
using Cyberia.Salamandra.Managers;
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

namespace Cyberia.Salamandra.Commands.Dofus.Map;

[Command(MapInteractionLocalizer.CommandName), Description(MapInteractionLocalizer.CommandDescription)]
[InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall, DiscordApplicationIntegrationType.UserInstall)]
[InteractionAllowedContexts(DiscordInteractionContextType.Guild, DiscordInteractionContextType.PrivateChannel)]
[InteractionLocalizer<MapInteractionLocalizer>]
public sealed class MapCommandModule
{
    private readonly CultureService _cultureService;
    private readonly EmbedBuilderService _embedBuilderService;

    public MapCommandModule(CultureService cultureService, EmbedBuilderService embedBuilderService)
    {
        _cultureService = cultureService;
        _embedBuilderService = embedBuilderService;
    }

    [Command(MapInteractionLocalizer.Id_CommandName), Description(MapInteractionLocalizer.Id_CommandDescription)]
    [InteractionLocalizer<MapInteractionLocalizer>]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public async Task IdExecuteAsync(SlashCommandContext ctx,
        [Parameter(MapInteractionLocalizer.Id_Id_ParameterName), Description(MapInteractionLocalizer.Id_Id_ParameterDescription)]
        [InteractionLocalizer<MapInteractionLocalizer>]
        [MinMaxValue(1, 99999)]
        int id)
    {
        await _cultureService.SetCultureAsync(ctx.Interaction);

        var mapData = DofusApi.Datacenter.MapsRepository.GetMapDataById((int)id);
        if (mapData is null)
        {
            await ctx.RespondAsync(BotTranslations.Map_NotFound);
            return;
        }

        await ctx.RespondAsync(await new MapMessageBuilder(_embedBuilderService, mapData).BuildAsync<DiscordInteractionResponseBuilder>());
    }


    [Command(MapInteractionLocalizer.Coordinates_CommandName), Description(MapInteractionLocalizer.Coordinates_CommandDescription)]
    [InteractionLocalizer<MapInteractionLocalizer>]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public async Task CoordinateExecuteAsync(SlashCommandContext ctx,
        [Parameter(MapInteractionLocalizer.Coordinates_X_ParameterName), Description(MapInteractionLocalizer.Coordinates_X_ParameterDescription)]
        [InteractionLocalizer<MapInteractionLocalizer>]
        [MinMaxValue(-666, 666)]
        int x,
        [Parameter(MapInteractionLocalizer.Coordinates_Y_ParameterName), Description(MapInteractionLocalizer.Coordinates_Y_ParameterDescription)]
        [InteractionLocalizer<MapInteractionLocalizer>]
        [MinMaxValue(-666, 666)]
        int y)
    {
        await _cultureService.SetCultureAsync(ctx.Interaction);

        var mapsData = DofusApi.Datacenter.MapsRepository.GetMapsDataByCoordinate(x, y).ToList();

        if (mapsData.Count == 0)
        {
            await ctx.RespondAsync(Translation.Format(BotTranslations.Map_NotFound_Coordinate, x, y));
            return;
        }

        if (mapsData.Count == 1)
        {
            await ctx.RespondAsync(await new MapMessageBuilder(_embedBuilderService, mapsData[0]).BuildAsync<DiscordInteractionResponseBuilder>());
            return;
        }
        
        await ctx.RespondAsync(await new PaginatedMapMessageBuilder(_embedBuilderService, mapsData, MapSearchCategory.Coordinate, $"{x}{PacketManager.ParameterSeparator}{y}")
            .BuildAsync<DiscordInteractionResponseBuilder>());
    }


    [Command(MapInteractionLocalizer.SubArea_CommandName), Description(MapInteractionLocalizer.SubArea_CommandDescription)]
    [InteractionLocalizer<MapInteractionLocalizer>]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public async Task MapSubAreaExecuteAsync(SlashCommandContext ctx,
        [Parameter(MapInteractionLocalizer.SubArea_Value_ParameterName), Description(MapInteractionLocalizer.SubArea_Value_ParameterDescription)]
        [InteractionLocalizer<MapInteractionLocalizer>]
        [SlashAutoCompleteProvider<MapSubAreaAutocompleteProvider>]
        [MinMaxLength(1, 70)]
        string value)
    {
        await _cultureService.SetCultureAsync(ctx.Interaction);

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
            var mapsData = mapSubAreaData.GetMapsData().ToList();

            if (mapsData.Count == 0)
            {
                await ctx.RespondAsync(Translation.Format(BotTranslations.Map_NotFound_MapSubArea, Formatter.Bold(mapSubAreaData.Name)));
            }
            else
            {
                await ctx.RespondAsync(await new PaginatedMapMessageBuilder(_embedBuilderService, mapsData, MapSearchCategory.MapSubArea, value)
                    .BuildAsync<DiscordInteractionResponseBuilder>());
            }
        }
    }


    [Command(MapInteractionLocalizer.Area_CommandName), Description(MapInteractionLocalizer.Area_CommandDescription)]
    [InteractionLocalizer<MapInteractionLocalizer>]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public async Task MapAreaExecuteAsync(SlashCommandContext ctx,
        [Parameter(MapInteractionLocalizer.Area_Value_ParameterName), Description(MapInteractionLocalizer.Area_Value_ParameterDescription)]
        [InteractionLocalizer<MapInteractionLocalizer>]
        [SlashAutoCompleteProvider<MapAreaAutocompleteProvider>]
        [MinMaxLength(1, 70)]
        string value)
    {
        await _cultureService.SetCultureAsync(ctx.Interaction);

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
            var mapsData = mapAreaData.GetMapsData().ToList();

            if (mapsData.Count == 0)
            {
                await ctx.RespondAsync(Translation.Format(BotTranslations.Map_NotFound_MapSubArea, Formatter.Bold(mapAreaData.Name)));
            }
            else
            {
                await ctx.RespondAsync(await new PaginatedMapMessageBuilder(_embedBuilderService, mapsData, MapSearchCategory.MapArea, value)
                    .BuildAsync<DiscordInteractionResponseBuilder>());
            }
        }
    }
}
