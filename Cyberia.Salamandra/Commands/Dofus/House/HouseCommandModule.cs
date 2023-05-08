using Cyberia.Api.DatacenterNS;

using DSharpPlus;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus
{
    [SlashCommandGroup("maison", "Retourne les informations des maisons")]
    public sealed class HouseCommandModule : ApplicationCommandModule
    {
        [SlashCommand("nom", "Retourne les informations d'une maison à partir de son nom")]
        public async Task NameCommand(InteractionContext ctx,
            [Option("nom", "Nom de la maison")]
            [Autocomplete(typeof(HouseAutocompleteProvider))]
            string sId)
        {
            House? house = null;

            if (int.TryParse(sId, out int id))
                house = Bot.Instance.Api.Datacenter.HousesData.GetHouseById(id);

            if (house is null)
                await ctx.CreateResponseAsync("Maison introuvable");
            else
                await new HouseMessageBuilder(house).SendInteractionResponse(ctx.Interaction);
        }


        [SlashCommand("coordonnees", "Retourne une liste de maisons à partir de leurs coordonnées")]
        public async Task CoordinateCommand(InteractionContext ctx,
            [Option("x", "Coordonnée x de la map de la maison")]
            [Minimum(-666), Maximum(666)]
            long x,
            [Option("y", "Coordonnée y de la map de la maison")]
            [Minimum(-666), Maximum(666)]
            long y)
        {
            List<House> houses = Bot.Instance.Api.Datacenter.HousesData.GetHousesByCoordinate((int)x, (int)y);

            if (houses.Count == 0)
                await ctx.CreateResponseAsync($"Il n'y a aucune maison en [{x}, {y}]");
            else if (houses.Count == 1)
                await new HouseMessageBuilder(houses[0]).SendInteractionResponse(ctx.Interaction);
            else
                await new HousePaginationMessageBuilder(houses).SendInteractionResponse(ctx.Interaction);
        }


        [SlashCommand("sous-zone", "Retourne une liste de maisons à partir de leur sous-zone")]
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
                List<House> houses = Bot.Instance.Api.Datacenter.HousesData.GetHousesByMapSubAreaId(id);

                if (houses.Count == 0)
                    await ctx.CreateResponseAsync($"La sous-zone {Formatter.Bold(mapSubArea.Name)} ne contient aucune maison");
                else if (houses.Count == 1)
                    await new HouseMessageBuilder(houses[0]).SendInteractionResponse(ctx.Interaction);
                else
                    await new HousePaginationMessageBuilder(houses).SendInteractionResponse(ctx.Interaction);
            }
        }


        [SlashCommand("zone", "Retourne une liste de maisons à partir de leur zone")]
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
                List<House> houses = Bot.Instance.Api.Datacenter.HousesData.GetHousesByMapAreaId(id);

                if (houses.Count == 0)
                    await ctx.CreateResponseAsync($"La zone {Formatter.Bold(mapArea.Name)} ne contient aucune maison");
                else if (houses.Count == 1)
                    await new HouseMessageBuilder(houses[0]).SendInteractionResponse(ctx.Interaction);
                else
                    await new HousePaginationMessageBuilder(houses).SendInteractionResponse(ctx.Interaction);
            }
        }
    }
}
