using Cyberia.Api.DatacenterNS;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus
{
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
                HouseData? houseData = Bot.Instance.Api.Datacenter.HousesData.GetHouseDataById(id);
                if (houseData is not null)
                    response = await new HouseMessageBuilder(houseData).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }
            else
            {
                List<HouseData> housesData = Bot.Instance.Api.Datacenter.HousesData.GetHousesDataByName(value);
                if (housesData.Count == 1)
                    response = await new HouseMessageBuilder(housesData[0]).GetMessageAsync<DiscordInteractionResponseBuilder>();
                else if (housesData.Count > 1)
                    response = await new PaginatedHouseMessageBuilder(housesData, HouseSearchCategory.Name, value).GetMessageAsync<DiscordInteractionResponseBuilder>();
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
            List<HouseData> housesData = Bot.Instance.Api.Datacenter.HousesData.GetHousesDataByCoordinate((int)xCoord, (int)yCoord);

            if (housesData.Count == 0)
                await ctx.CreateResponseAsync($"Il n'y a aucune maison en [{xCoord}, {yCoord}]");
            else if (housesData.Count == 1)
                await ctx.CreateResponseAsync(await new HouseMessageBuilder(housesData[0]).GetMessageAsync<DiscordInteractionResponseBuilder>());
            else
                await ctx.CreateResponseAsync(await new PaginatedHouseMessageBuilder(housesData, HouseSearchCategory.Coordinate, $"{xCoord}{InteractionManager.PACKET_PARAMETER_SEPARATOR}{yCoord}").GetMessageAsync<DiscordInteractionResponseBuilder>());
        }


        [SlashCommand("sous-zone", "Retourne une liste de maisons à partir de leur sous-zone")]
        public async Task MapSubAreaCommand(InteractionContext ctx,
            [Option("nom", "Nom de la sous-zone")]
            [Autocomplete(typeof(MapSubAreaAutocompleteProvider))]
            string value)
        {
            MapSubAreaData? mapSubAreaData = null;

            if (int.TryParse(value, out int id))
                mapSubAreaData = Bot.Instance.Api.Datacenter.MapsData.GetMapSubAreaDataById(id);

            if (mapSubAreaData is null)
                await ctx.CreateResponseAsync("Sous-zone introuvable");
            else
            {
                List<HouseData> housesData = Bot.Instance.Api.Datacenter.HousesData.GetHousesDataByMapSubAreaId(id);

                if (housesData.Count == 0)
                    await ctx.CreateResponseAsync($"La sous-zone {Formatter.Bold(mapSubAreaData.Name)} ne contient aucune maison");
                else if (housesData.Count == 1)
                    await ctx.CreateResponseAsync(await new HouseMessageBuilder(housesData[0]).GetMessageAsync<DiscordInteractionResponseBuilder>());
                else
                    await ctx.CreateResponseAsync(await new PaginatedHouseMessageBuilder(housesData, HouseSearchCategory.MapSubArea, value).GetMessageAsync<DiscordInteractionResponseBuilder>());
            }
        }


        [SlashCommand("zone", "Retourne une liste de maisons à partir de leur zone")]
        public async Task MapAreaCommand(InteractionContext ctx,
            [Option("nom", "Nom de la zone")]
            [Autocomplete(typeof(MapAreaAutocompleteProvider))]
            string value)
        {
            MapAreaData? mapAreaData = null;

            if (int.TryParse(value, out int id))
                mapAreaData = Bot.Instance.Api.Datacenter.MapsData.GetMapAreaDataById(id);

            if (mapAreaData is null)
                await ctx.CreateResponseAsync("Zone introuvable");
            else
            {
                List<HouseData> housesData = Bot.Instance.Api.Datacenter.HousesData.GetHousesDataByMapAreaId(id);

                if (housesData.Count == 0)
                    await ctx.CreateResponseAsync($"La zone {Formatter.Bold(mapAreaData.Name)} ne contient aucune maison");
                else if (housesData.Count == 1)
                    await ctx.CreateResponseAsync(await new HouseMessageBuilder(housesData[0]).GetMessageAsync<DiscordInteractionResponseBuilder>());
                else
                    await ctx.CreateResponseAsync(await new PaginatedHouseMessageBuilder(housesData, HouseSearchCategory.MapArea, value).GetMessageAsync<DiscordInteractionResponseBuilder>());
            }
        }
    }
}
