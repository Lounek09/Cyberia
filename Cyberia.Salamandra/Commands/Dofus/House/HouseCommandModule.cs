using Cyberia.Api;
using Cyberia.Api.Data.Maps;
using Cyberia.Salamandra.Commands.Dofus.Map;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Commands.Processors.SlashCommands.Metadata;
using DSharpPlus.Entities;

using System.ComponentModel;

namespace Cyberia.Salamandra.Commands.Dofus.House;

[Command("maison"), Description("Retourne les informations d'une maison")]
[InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall, DiscordApplicationIntegrationType.UserInstall)]
[InteractionAllowedContexts(DiscordInteractionContextType.Guild, DiscordInteractionContextType.PrivateChannel)]
public sealed class HouseCommandModule
{
    [Command("nom"), Description("Retourne les informations d'une maison à partir de son nom")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public static async Task NameExecuteAsync(SlashCommandContext ctx,
        [Parameter("nom"), Description("Nom de la maison")]
        [SlashAutoCompleteProvider<HouseAutocompleteProvider>]
        [SlashMinMaxLength(MinLength = 1, MaxLength = 70)]
        string value)
    {
        DiscordInteractionResponseBuilder? response = null;

        if (int.TryParse(value, out var id))
        {
            var houseData = DofusApi.Datacenter.HousesData.GetHouseDataById(id);
            if (houseData is not null)
            {
                response = await new HouseMessageBuilder(houseData).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }
        }
        else
        {
            var housesData = DofusApi.Datacenter.HousesData.GetHousesDataByName(value).ToList();
            if (housesData.Count == 1)
            {
                response = await new HouseMessageBuilder(housesData[0]).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }
            else if (housesData.Count > 1)
            {
                response = await new PaginatedHouseMessageBuilder(housesData, HouseSearchCategory.Name, value).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }
        }

        response ??= new DiscordInteractionResponseBuilder().WithContent("Maison introuvable");
        await ctx.RespondAsync(response);
    }


    [Command("coordonnees"), Description("Retourne une liste de maisons à partir de leurs coordonnées")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public static async Task CoordinateExecuteAsync(SlashCommandContext ctx,
        [Parameter("x"), Description("Coordonnée x de la map de la maison")]
        [SlashMinMaxValue(MinValue = -666, MaxValue = 666)]
        int xCoord,
        [Parameter("y"), Description("Coordonnée y de la map de la maison")]
        [SlashMinMaxValue(MinValue = -666, MaxValue = 666)]
        int yCoord)
    {
        var housesData = DofusApi.Datacenter.HousesData.GetHousesDataByCoordinate(xCoord, yCoord).ToList();

        if (housesData.Count == 0)
        {
            await ctx.RespondAsync($"Il n'y a aucune maison en [{xCoord}, {yCoord}]");
        }
        else if (housesData.Count == 1)
        {
            await ctx.RespondAsync(await new HouseMessageBuilder(housesData[0]).GetMessageAsync<DiscordInteractionResponseBuilder>());
        }
        else
        {
            await ctx.RespondAsync(await new PaginatedHouseMessageBuilder(housesData, HouseSearchCategory.Coordinate, $"{xCoord}{InteractionManager.PacketParameterSeparator}{yCoord}")
                .GetMessageAsync<DiscordInteractionResponseBuilder>());
        }
    }


    [Command("sous-zone"), Description("Retourne une liste de maisons à partir de leur sous-zone")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public static async Task MapSubAreaExecuteAsync(SlashCommandContext ctx,
        [Parameter("nom"), Description("Nom de la sous-zone")]
        [SlashAutoCompleteProvider<MapSubAreaAutocompleteProvider>]
        [SlashMinMaxLength(MinLength = 1, MaxLength = 70)]
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
            var housesData = DofusApi.Datacenter.HousesData.GetHousesDataByMapSubAreaId(id).ToList();

            if (housesData.Count == 0)
            {
                await ctx.RespondAsync($"La sous-zone {Formatter.Bold(mapSubAreaData.Name)} ne contient aucune maison");
            }
            else if (housesData.Count == 1)
            {
                await ctx.RespondAsync(await new HouseMessageBuilder(housesData[0]).GetMessageAsync<DiscordInteractionResponseBuilder>());
            }
            else
            {
                await ctx.RespondAsync(await new PaginatedHouseMessageBuilder(housesData, HouseSearchCategory.MapSubArea, value)
                    .GetMessageAsync<DiscordInteractionResponseBuilder>());
            }
        }
    }


    [Command("zone"), Description("Retourne une liste de maisons à partir de leur zone")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public static async Task MapAreaExecuteAsync(SlashCommandContext ctx,
        [Parameter("nom"), Description("Nom de la zone")]
        [SlashAutoCompleteProvider<MapAreaAutocompleteProvider>]
        [SlashMinMaxLength(MinLength = 1, MaxLength = 70)]
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
            var housesData = DofusApi.Datacenter.HousesData.GetHousesDataByMapAreaId(id).ToList();

            if (housesData.Count == 0)
            {
                await ctx.RespondAsync($"La zone {Formatter.Bold(mapAreaData.Name)} ne contient aucune maison");
            }
            else if (housesData.Count == 1)
            {
                await ctx.RespondAsync(await new HouseMessageBuilder(housesData[0]).GetMessageAsync<DiscordInteractionResponseBuilder>());
            }
            else
            {
                await ctx.RespondAsync(await new PaginatedHouseMessageBuilder(housesData, HouseSearchCategory.MapArea, value)
                    .GetMessageAsync<DiscordInteractionResponseBuilder>());
            }
        }
    }
}
