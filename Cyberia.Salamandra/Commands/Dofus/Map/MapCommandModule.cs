using Cyberia.Api;
using Cyberia.Api.Data.Maps;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.ArgumentModifiers;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Commands.Processors.SlashCommands.Metadata;
using DSharpPlus.Entities;

using System.ComponentModel;

namespace Cyberia.Salamandra.Commands.Dofus.Map;

[Command("map"), Description("Retourne les informations d'une map")]
[InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall, DiscordApplicationIntegrationType.UserInstall)]
[InteractionAllowedContexts(DiscordInteractionContextType.Guild, DiscordInteractionContextType.PrivateChannel)]
public sealed class MapCommandModule
{
    [Command("id"), Description("Retourne les informations d'une map à partir de son id")]
    public static async Task IdExecuteAsync(SlashCommandContext ctx,
        [Parameter("id"), Description("Id de la map")]
        [MinMaxValue(1, 99999)]
        int id)
    {
        var mapData = DofusApi.Datacenter.MapsRepository.GetMapDataById((int)id);
        if (mapData is null)
        {
            await ctx.RespondAsync(BotTranslations.Map_NotFound);
            return;
        }

        await ctx.RespondAsync(await new MapMessageBuilder(mapData).GetMessageAsync<DiscordInteractionResponseBuilder>());
    }


    [Command("coordonnees"), Description("Retourne une liste de maps à partir de leurs coordonnées")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public static async Task CoordinateExecuteAsync(SlashCommandContext ctx,
        [Parameter("x"), Description("Coordonnée x de la map")]
        [MinMaxValue(-666, 666)]
        int x,
        [Parameter("y"), Description("Coordonnée y de la map")]
        [MinMaxValue(-666, 666)]
        int y)
    {
        var mapsData = DofusApi.Datacenter.MapsRepository.GetMapsDataByCoordinate(x, y).ToList();

        if (mapsData.Count == 0)
        {
            await ctx.RespondAsync(Translation.Format(BotTranslations.Map_NotFound_Coordinate, x, y));
            return;
        }

        if (mapsData.Count == 1)
        {
            await ctx.RespondAsync(await new MapMessageBuilder(mapsData[0]).GetMessageAsync<DiscordInteractionResponseBuilder>());
            return;
        }
        
        await ctx.RespondAsync(await new PaginatedMapMessageBuilder(mapsData, MapSearchCategory.Coordinate, $"{x}{InteractionManager.PacketParameterSeparator}{y}")
            .GetMessageAsync<DiscordInteractionResponseBuilder>());
    }


    [Command("sous-zone"), Description("Retourne une liste de maps à partir de leur sous-zone")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public static async Task MapSubAreaExecuteAsync(SlashCommandContext ctx,
        [Parameter("nom"), Description("Nom de la sous-zone")]
        [SlashAutoCompleteProvider<MapSubAreaAutocompleteProvider>]
        [MinMaxLength(1, 70)]
        string value)
    {
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
                await ctx.RespondAsync(await new PaginatedMapMessageBuilder(mapsData, MapSearchCategory.MapSubArea, value)
                    .GetMessageAsync<DiscordInteractionResponseBuilder>());
            }
        }
    }


    [Command("zone"), Description("Retourne une liste de maps à partir de leur zone")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public static async Task MapAreaExecuteAsync(SlashCommandContext ctx,
        [Parameter("nom"), Description("Nom de la zone")]
        [SlashAutoCompleteProvider<MapAreaAutocompleteProvider>]
        [MinMaxLength(1, 70)]
        string value)
    {
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
                await ctx.RespondAsync(await new PaginatedMapMessageBuilder(mapsData, MapSearchCategory.MapArea, value)
                    .GetMessageAsync<DiscordInteractionResponseBuilder>());
            }
        }
    }
}
