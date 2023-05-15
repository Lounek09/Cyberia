using Cyberia.Api.DatacenterNS;

using DSharpPlus;
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
                await new MapMessageBuilder(map).SendInteractionResponse(ctx.Interaction);
        }


        [SlashCommand("coordonnees", "Retourne une liste de maps à partir de leurs coordonnées")]
        public async Task CoordinateCommand(InteractionContext ctx,
            [Option("x", "Coordonnée x de la map")]
            [Minimum(-666), Maximum(666)]
            long x,
            [Option("y", "Coordonnée y de la map")]
            [Minimum(-666), Maximum(666)]
            long y)
        {
            List<Map> maps = Bot.Instance.Api.Datacenter.MapsData.GetMapsByCoordinate((int)x, (int)y);

            if (maps.Count == 0)
                await ctx.CreateResponseAsync($"Il n'y a aucune map en [{x}, {y}]");
            if (maps.Count == 1)
                await new MapMessageBuilder(maps[0]).SendInteractionResponse(ctx.Interaction);
            else if (maps.Count > 1)
                await new MapPaginationMessageBuilder($"Plusieurs maps trouvés en [{x}, {y}] :", maps).SendInteractionResponse(ctx.Interaction);
        }


        [SlashCommand("sous-zone", "Retourne une liste de maps à partir de leur sous-zone")]
        public async Task MapSubAreaCommand(InteractionContext ctx,
            [Option("nom", "Nom de la sous-zone")]
            [Autocomplete(typeof(MapSubAreaAutocompleteProvider))]
            string sId)
        {
            MapSubArea? mapSubArea = null;

            if (int.TryParse(sId, out int id))
                mapSubArea = Bot.Instance.Api.Datacenter.MapsData.GetMapSubAreaById(id);

            if (mapSubArea is null)
                await ctx.CreateResponseAsync("Sous-zone introuvable");
            else
            {
                List<Map> maps = mapSubArea.GetMaps();

                if (maps.Count == 0)
                    await ctx.CreateResponseAsync($"La sous-zone {Formatter.Bold(mapSubArea.Name)} ne contient aucune map");
                else
                    await new MapPaginationMessageBuilder($"Liste des maps de la sous-zone '{mapSubArea.Name}' :", maps).SendInteractionResponse(ctx.Interaction);
            }
        }


        [SlashCommand("zone", "Retourne une liste de maps à partir de leur zone")]
        public async Task MapAreaCommand(InteractionContext ctx,
            [Option("nom", "Nom de la zone")]
            [Autocomplete(typeof(MapAreaAutocompleteProvider))]
            string sId)
        {
            MapArea? mapArea = null;

            if (int.TryParse(sId, out int id))
                mapArea = Bot.Instance.Api.Datacenter.MapsData.GetMapAreaById(id);

            if (mapArea is null)
                await ctx.CreateResponseAsync("Zone introuvable");
            else
            {
                List<Map> maps = mapArea.GetMaps();

                if (maps.Count == 0)
                    await ctx.CreateResponseAsync($"La zone {Formatter.Bold(mapArea.Name)} ne contient aucune map");
                else
                    await new MapPaginationMessageBuilder($"Liste des maps de la zone '{mapArea.Name}' :", maps).SendInteractionResponse(ctx.Interaction);
            }
        }
    }
}
