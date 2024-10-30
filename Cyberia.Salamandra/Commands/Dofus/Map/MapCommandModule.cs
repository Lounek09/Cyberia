using Cyberia.Api;
using Cyberia.Api.Data.Maps;
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
        var culture = await _cultureService.GetCultureAsync(ctx.Interaction);

        var mapData = DofusApi.Datacenter.MapsRepository.GetMapDataById(id);
        if (mapData is null)
        {
            await ctx.RespondAsync(Translation.Get<BotTranslations>("Map.NotFound", culture));
            return;
        }

        await ctx.RespondAsync(await new MapMessageBuilder(_embedBuilderService, mapData, culture)
            .BuildAsync<DiscordInteractionResponseBuilder>());
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
        var culture = await _cultureService.GetCultureAsync(ctx.Interaction);

        var mapsData = DofusApi.Datacenter.MapsRepository.GetMapsDataByCoordinate(x, y).ToList();

        if (mapsData.Count == 0)
        {
            await ctx.RespondAsync(Translation.Format(Translation.Get<BotTranslations>("Map.NotFound.Coordinate", culture), x, y));
            return;
        }

        if (mapsData.Count == 1)
        {
            await ctx.RespondAsync(await new MapMessageBuilder(_embedBuilderService, mapsData[0], culture)
                .BuildAsync<DiscordInteractionResponseBuilder>());
            return;
        }
        
        await ctx.RespondAsync(await new PaginatedMapMessageBuilder(_embedBuilderService, mapsData, MapSearchCategory.Coordinate, $"{x}{PacketFormatter.Separator}{y}", culture)
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
        var culture = await _cultureService.GetCultureAsync(ctx.Interaction);

        MapSubAreaData? mapSubAreaData = null;

        if (int.TryParse(value, out var id))
        {
            mapSubAreaData = DofusApi.Datacenter.MapsRepository.GetMapSubAreaDataById(id);
        }

        if (mapSubAreaData is null)
        {
            await ctx.RespondAsync(Translation.Get<BotTranslations>("MapSubArea.NotFound", culture));
        }
        else
        {
            var mapsData = mapSubAreaData.GetMapsData().ToList();

            if (mapsData.Count == 0)
            {
                await ctx.RespondAsync(Translation.Format(Translation.Get<BotTranslations>("Map.NotFound.MapSubArea", culture), Formatter.Bold(mapSubAreaData.Name)));
            }
            else
            {
                await ctx.RespondAsync(await new PaginatedMapMessageBuilder(_embedBuilderService, mapsData, MapSearchCategory.MapSubArea, value, culture)
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
        var culture = await _cultureService.GetCultureAsync(ctx.Interaction);

        MapAreaData? mapAreaData = null;

        if (int.TryParse(value, out var id))
        {
            mapAreaData = DofusApi.Datacenter.MapsRepository.GetMapAreaDataById(id);
        }

        if (mapAreaData is null)
        {
            await ctx.RespondAsync(Translation.Get<BotTranslations>("MapArea.NotFound", culture));
        }
        else
        {
            var mapsData = mapAreaData.GetMapsData().ToList();

            if (mapsData.Count == 0)
            {
                await ctx.RespondAsync(Translation.Format(Translation.Get<BotTranslations>("Map.NotFound.MapSubArea", culture), Formatter.Bold(mapAreaData.Name)));
            }
            else
            {
                await ctx.RespondAsync(await new PaginatedMapMessageBuilder(_embedBuilderService, mapsData, MapSearchCategory.MapArea, value, culture)
                    .BuildAsync<DiscordInteractionResponseBuilder>());
            }
        }
    }
}
