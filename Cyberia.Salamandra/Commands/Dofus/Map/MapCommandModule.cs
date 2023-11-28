using Cyberia.Api;
using Cyberia.Api.Data.Maps;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus;

[SlashCommandGroup("map", "Retourne les informations de la map appelée")]
public sealed class MapCommandModule : ApplicationCommandModule
{
    [SlashCommand("id", "Retourne les informations d'une map à partir de son id")]
    public async Task IdCommand(InteractionContext ctx,
        [Option("id", "Id de la map")]
        [Minimum(1), Maximum(99999)]
        long id)
    {
        var mapData = DofusApi.Datacenter.MapsData.GetMapDataById((int)id);

        if (mapData is null)
        {
            await ctx.CreateResponseAsync("Map introuvable");
        }
        else
        {
            await ctx.CreateResponseAsync(await new MapMessageBuilder(mapData).GetMessageAsync<DiscordInteractionResponseBuilder>());
        }
    }


    [SlashCommand("coordonnees", "Retourne une liste de maps à partir de leurs coordonnées")]
    public async Task CoordinateCommand(InteractionContext ctx,
        [Option("x", "Coordonnée x de la map")]
        [Minimum(-666), Maximum(666)]
        long xCoord,
        [Option("y", "Coordonnée y de la map")]
        [Minimum(-666), Maximum(666)]
        long yCoord)
    {
        var mapsData = DofusApi.Datacenter.MapsData.GetMapsDataByCoordinate((int)xCoord, (int)yCoord).ToList();

        if (mapsData.Count == 0)
        {
            await ctx.CreateResponseAsync($"Il n'y a aucune map en [{xCoord}, {yCoord}]");
        }
        else if (mapsData.Count == 1)
        {
            await ctx.CreateResponseAsync(await new MapMessageBuilder(mapsData[0]).GetMessageAsync<DiscordInteractionResponseBuilder>());
        }
        else
        {
            await ctx.CreateResponseAsync(await new PaginatedMapMessageBuilder(mapsData, MapSearchCategory.Coordinate, $"{xCoord}{InteractionManager.PACKET_PARAMETER_SEPARATOR}{yCoord}").GetMessageAsync<DiscordInteractionResponseBuilder>());
        }
    }


    [SlashCommand("sous-zone", "Retourne une liste de maps à partir de leur sous-zone")]
    public async Task MapSubAreaCommand(InteractionContext ctx,
        [Option("nom", "Nom de la sous-zone", true)]
        [Autocomplete(typeof(MapSubAreaAutocompleteProvider))]
        string value)
    {
        MapSubAreaData? mapSubAreaData = null;

        if (int.TryParse(value, out var id))
        {
            mapSubAreaData = DofusApi.Datacenter.MapsData.GetMapSubAreaDataById(id);
        }

        if (mapSubAreaData is null)
        {
            await ctx.CreateResponseAsync("Sous-zone introuvable");
        }
        else
        {
            var mapsData = mapSubAreaData.GetMapsData().ToList();

            if (mapsData.Count == 0)
            {
                await ctx.CreateResponseAsync($"La sous-zone {Formatter.Bold(mapSubAreaData.Name)} ne contient aucune map");
            }
            else
            {
                await ctx.CreateResponseAsync(await new PaginatedMapMessageBuilder(mapsData, MapSearchCategory.MapSubArea, value).GetMessageAsync<DiscordInteractionResponseBuilder>());
            }
        }
    }


    [SlashCommand("zone", "Retourne une liste de maps à partir de leur zone")]
    public async Task MapAreaCommand(InteractionContext ctx,
        [Option("nom", "Nom de la zone", true)]
        [Autocomplete(typeof(MapAreaAutocompleteProvider))]
        string value)
    {
        MapAreaData? mapAreaData = null;

        if (int.TryParse(value, out var id))
        {
            mapAreaData = DofusApi.Datacenter.MapsData.GetMapAreaDataById(id);
        }

        if (mapAreaData is null)
        {
            await ctx.CreateResponseAsync("Zone introuvable");
        }
        else
        {
            var mapsData = mapAreaData.GetMapsData().ToList();

            if (mapsData.Count == 0)
            {
                await ctx.CreateResponseAsync($"La zone {Formatter.Bold(mapAreaData.Name)} ne contient aucune map");
            }
            else
            {
                await ctx.CreateResponseAsync(await new PaginatedMapMessageBuilder(mapsData, MapSearchCategory.MapArea, value).GetMessageAsync<DiscordInteractionResponseBuilder>());
            }
        }
    }
}
