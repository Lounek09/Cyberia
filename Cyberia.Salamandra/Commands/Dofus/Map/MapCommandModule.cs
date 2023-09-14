using Cyberia.Api.DatacenterNS;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus
{
    [SlashCommandGroup("map", "Retourne les informations de la map appelée")]
    public sealed class MapCommandModule : ApplicationCommandModule
    {
        [SlashCommand("id", "Retourne les informations d'une map à partir de son id")]
        public async Task IdCommand(InteractionContext ctx,
            [Option("id", "Id de la map")]
            [Minimum(1), Maximum(99999)]
            long id)
        {
            Map? map = Bot.Instance.Api.Datacenter.MapsData.GetMapById((int)id);

            if (map is null)
                await ctx.CreateResponseAsync("Map introuvable");
            else
                await ctx.CreateResponseAsync(await new MapMessageBuilder(map).GetMessageAsync<DiscordInteractionResponseBuilder>());
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
            List<Map> maps = Bot.Instance.Api.Datacenter.MapsData.GetMapsByCoordinate((int)xCoord, (int)yCoord);

            if (maps.Count == 0)
                await ctx.CreateResponseAsync($"Il n'y a aucune map en [{xCoord}, {yCoord}]");
            else if (maps.Count == 1)
                await ctx.CreateResponseAsync(await new MapMessageBuilder(maps[0]).GetMessageAsync<DiscordInteractionResponseBuilder>());
            else
                await ctx.CreateResponseAsync(await new PaginatedMapMessageBuilder(maps, MapSearchCategory.Coordinate, $"{xCoord}{InteractionManager.PACKET_PARAMETER_SEPARATOR}{yCoord}").GetMessageAsync<DiscordInteractionResponseBuilder>());
        }


        [SlashCommand("sous-zone", "Retourne une liste de maps à partir de leur sous-zone")]
        public async Task MapSubAreaCommand(InteractionContext ctx,
            [Option("nom", "Nom de la sous-zone", true)]
            [Autocomplete(typeof(MapSubAreaAutocompleteProvider))]
            string value)
        {
            MapSubArea? mapSubArea = null;

            if (int.TryParse(value, out int id))
                mapSubArea = Bot.Instance.Api.Datacenter.MapsData.GetMapSubAreaById(id);

            if (mapSubArea is null)
                await ctx.CreateResponseAsync("Sous-zone introuvable");
            else
            {
                List<Map> maps = mapSubArea.GetMaps();

                if (maps.Count == 0)
                    await ctx.CreateResponseAsync($"La sous-zone {Formatter.Bold(mapSubArea.Name)} ne contient aucune map");
                else
                    await ctx.CreateResponseAsync(await new PaginatedMapMessageBuilder(maps, MapSearchCategory.MapSubArea, value).GetMessageAsync<DiscordInteractionResponseBuilder>());
            }
        }


        [SlashCommand("zone", "Retourne une liste de maps à partir de leur zone")]
        public async Task MapAreaCommand(InteractionContext ctx,
            [Option("nom", "Nom de la zone", true)]
            [Autocomplete(typeof(MapAreaAutocompleteProvider))]
            string value)
        {
            MapArea? mapArea = null;

            if (int.TryParse(value, out int id))
                mapArea = Bot.Instance.Api.Datacenter.MapsData.GetMapAreaById(id);

            if (mapArea is null)
                await ctx.CreateResponseAsync("Zone introuvable");
            else
            {
                List<Map> maps = mapArea.GetMaps();

                if (maps.Count == 0)
                    await ctx.CreateResponseAsync($"La zone {Formatter.Bold(mapArea.Name)} ne contient aucune map");
                else
                    await ctx.CreateResponseAsync(await new PaginatedMapMessageBuilder(maps, MapSearchCategory.MapArea, value).GetMessageAsync<DiscordInteractionResponseBuilder>());
            }
        }
    }
}
