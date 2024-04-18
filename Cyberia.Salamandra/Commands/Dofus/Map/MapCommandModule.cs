using Cyberia.Api;
using Cyberia.Api.Data.Maps;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Commands.Processors.SlashCommands.Metadata;
using DSharpPlus.Entities;

using System.ComponentModel;

namespace Cyberia.Salamandra.Commands.Dofus;

[Command("map"), Description("Retourne les informations d'une map")]
public sealed class MapCommandModule
{
    [Command("id"), Description("Retourne les informations d'une map à partir de son id")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall, DiscordApplicationIntegrationType.UserInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild, DiscordInteractionContextType.PrivateChannel)]
    public static async Task IdExecuteAsync(SlashCommandContext ctx,
        [Parameter("id"), Description("Id de la map")]
        [SlashMinMaxValue(MinValue = 1, MaxValue = 99999)]
        long id)
    {
        var mapData = DofusApi.Datacenter.MapsData.GetMapDataById((int)id);
        if (mapData is null)
        {
            await ctx.RespondAsync("Map introuvable");
            return;
        }

        await ctx.RespondAsync(await new MapMessageBuilder(mapData).GetMessageAsync<DiscordInteractionResponseBuilder>());
    }


    [Command("coordonnees"), Description("Retourne une liste de maps à partir de leurs coordonnées")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall, DiscordApplicationIntegrationType.UserInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild, DiscordInteractionContextType.PrivateChannel)]
    public static async Task CoordinateExecuteAsync(SlashCommandContext ctx,
        [Parameter("x"), Description("Coordonnée x de la map")]
        [SlashMinMaxValue(MinValue = -666, MaxValue = 666)]
        long xCoord,
        [Parameter("y"), Description("Coordonnée y de la map")]
        [SlashMinMaxValue(MinValue = -666, MaxValue = 666)]
        long yCoord)
    {
        var mapsData = DofusApi.Datacenter.MapsData.GetMapsDataByCoordinate((int)xCoord, (int)yCoord).ToList();

        if (mapsData.Count == 0)
        {
            await ctx.RespondAsync($"Il n'y a aucune map en [{xCoord}, {yCoord}]");
            return;
        }

        if (mapsData.Count == 1)
        {
            await ctx.RespondAsync(await new MapMessageBuilder(mapsData[0]).GetMessageAsync<DiscordInteractionResponseBuilder>());
            return;
        }
        
        await ctx.RespondAsync(await new PaginatedMapMessageBuilder(mapsData, MapSearchCategory.Coordinate, $"{xCoord}{InteractionManager.PacketParameterSeparator}{yCoord}")
            .GetMessageAsync<DiscordInteractionResponseBuilder>());
    }


    [Command("sous-zone"), Description("Retourne une liste de maps à partir de leur sous-zone")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall, DiscordApplicationIntegrationType.UserInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild, DiscordInteractionContextType.PrivateChannel)]
    public static async Task MapSubAreaExecuteAsync(SlashCommandContext ctx,
        [Parameter("nom"), Description("Nom de la sous-zone")]
        [SlashAutoCompleteProvider<MapSubAreaAutocompleteProvider>]
        string value)
    {
        MapSubAreaData? mapSubAreaData = null;

        if (int.TryParse(value, out var id))
        {
            mapSubAreaData = DofusApi.Datacenter.MapsData.GetMapSubAreaDataById(id);
        }

        if (mapSubAreaData is null)
        {
            await ctx.RespondAsync("Sous-zone introuvable");
        }
        else
        {
            var mapsData = mapSubAreaData.GetMapsData().ToList();

            if (mapsData.Count == 0)
            {
                await ctx.RespondAsync($"La sous-zone {Formatter.Bold(mapSubAreaData.Name)} ne contient aucune map");
            }
            else
            {
                await ctx.RespondAsync(await new PaginatedMapMessageBuilder(mapsData, MapSearchCategory.MapSubArea, value).GetMessageAsync<DiscordInteractionResponseBuilder>());
            }
        }
    }


    [Command("zone"), Description("Retourne une liste de maps à partir de leur zone")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall, DiscordApplicationIntegrationType.UserInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild, DiscordInteractionContextType.PrivateChannel)]
    public static async Task MapAreaExecuteAsync(SlashCommandContext ctx,
        [Parameter("nom"), Description("Nom de la zone")]
        [SlashAutoCompleteProvider<MapAreaAutocompleteProvider>]
        string value)
    {
        MapAreaData? mapAreaData = null;

        if (int.TryParse(value, out var id))
        {
            mapAreaData = DofusApi.Datacenter.MapsData.GetMapAreaDataById(id);
        }

        if (mapAreaData is null)
        {
            await ctx.RespondAsync("Zone introuvable");
        }
        else
        {
            var mapsData = mapAreaData.GetMapsData().ToList();

            if (mapsData.Count == 0)
            {
                await ctx.RespondAsync($"La zone {Formatter.Bold(mapAreaData.Name)} ne contient aucune map");
            }
            else
            {
                await ctx.RespondAsync(await new PaginatedMapMessageBuilder(mapsData, MapSearchCategory.MapArea, value).GetMessageAsync<DiscordInteractionResponseBuilder>());
            }
        }
    }
}
