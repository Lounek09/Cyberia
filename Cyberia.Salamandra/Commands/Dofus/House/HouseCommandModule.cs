using Cyberia.Api.DatacenterNS;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

using Newtonsoft.Json.Linq;

namespace Cyberia.Salamandra.Commands.Dofus
{
#pragma warning disable CA1822 // Mark members as static
    [SlashCommandGroup("maison", "Retourne les informations des maisons")]
    public sealed class HouseCommandModule : ApplicationCommandModule
    {
        [SlashCommand("nom", "Retourne les informations d'une maison à partir de son nom")]
        public async Task NameCommand(InteractionContext ctx,
            [Option("nom", "Nom de la maison", true)]
            [Autocomplete(typeof(HouseAutocompleteProvider))]
            string value)
        {
            DiscordInteractionResponseBuilder? response = null;

            if (int.TryParse(value, out int id))
            {
                House? house = Bot.Instance.Api.Datacenter.HousesData.GetHouseById(id);
                if (house is not null)
                    response = await new HouseMessageBuilder(house).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }
            else
            {
                List<House> houses = Bot.Instance.Api.Datacenter.HousesData.GetHousesByName(value);
                if (houses.Count == 1)
                    response = await new HouseMessageBuilder(houses[0]).GetMessageAsync<DiscordInteractionResponseBuilder>();
                else if (houses.Count > 1)
                    response = await new PaginatedHouseMessageBuilder(houses, HouseSearchCategory.Name, value).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }

            response ??= new DiscordInteractionResponseBuilder().WithContent("Maison introuvable");
            await ctx.CreateResponseAsync(response);
        }


        [SlashCommand("coordonnees", "Retourne une liste de maisons à partir de leurs coordonnées")]
        public async Task CoordinateCommand(InteractionContext ctx,
            [Option("x", "Coordonnée x de la map de la maison")]
            [Minimum(-666), Maximum(666)]
            long xCoord,
            [Option("y", "Coordonnée y de la map de la maison")]
            [Minimum(-666), Maximum(666)]
            long yCoord)
        {
            List<House> houses = Bot.Instance.Api.Datacenter.HousesData.GetHousesByCoordinate((int)xCoord, (int)yCoord);

            if (houses.Count == 0)
                await ctx.CreateResponseAsync($"Il n'y a aucune maison en [{xCoord}, {yCoord}]");
            else if (houses.Count == 1)
                await ctx.CreateResponseAsync(await new HouseMessageBuilder(houses[0]).GetMessageAsync<DiscordInteractionResponseBuilder>());
            else
                await ctx.CreateResponseAsync(await new PaginatedHouseMessageBuilder(houses, HouseSearchCategory.Coordinate, $"{xCoord}{InteractionManager.PACKET_PARAMETER_SEPARATOR}{yCoord}").GetMessageAsync<DiscordInteractionResponseBuilder>());
        }


        [SlashCommand("sous-zone", "Retourne une liste de maisons à partir de leur sous-zone")]
        public async Task MapSubAreaCommand(InteractionContext ctx,
            [Option("nom", "Nom de la sous-zone")]
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
                List<House> houses = Bot.Instance.Api.Datacenter.HousesData.GetHousesByMapSubAreaId(id);

                if (houses.Count == 0)
                    await ctx.CreateResponseAsync($"La sous-zone {Formatter.Bold(mapSubArea.Name)} ne contient aucune maison");
                else if (houses.Count == 1)
                    await ctx.CreateResponseAsync(await new HouseMessageBuilder(houses[0]).GetMessageAsync<DiscordInteractionResponseBuilder>());
                else
                    await ctx.CreateResponseAsync(await new PaginatedHouseMessageBuilder(houses, HouseSearchCategory.MapSubArea, value).GetMessageAsync<DiscordInteractionResponseBuilder>());
            }
        }


        [SlashCommand("zone", "Retourne une liste de maisons à partir de leur zone")]
        public async Task MapAreaCommand(InteractionContext ctx,
            [Option("nom", "Nom de la zone")]
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
                List<House> houses = Bot.Instance.Api.Datacenter.HousesData.GetHousesByMapAreaId(id);

                if (houses.Count == 0)
                    await ctx.CreateResponseAsync($"La zone {Formatter.Bold(mapArea.Name)} ne contient aucune maison");
                else if (houses.Count == 1)
                    await ctx.CreateResponseAsync(await new HouseMessageBuilder(houses[0]).GetMessageAsync<DiscordInteractionResponseBuilder>());
                else
                    await ctx.CreateResponseAsync(await new PaginatedHouseMessageBuilder(houses, HouseSearchCategory.MapArea, value).GetMessageAsync<DiscordInteractionResponseBuilder>());
            }
        }
    }
}
