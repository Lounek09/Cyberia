using Cyberia.Api;
using Cyberia.Api.Data.Maps;
using Cyberia.Salamandra.Commands.Dofus.Map;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.ArgumentModifiers;
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
        [MinMaxLength(1, 70)]
        string value)
    {
        CommandManager.SetCulture();

        DiscordInteractionResponseBuilder? response = null;

        if (int.TryParse(value, out var id))
        {
            var houseData = DofusApi.Datacenter.HousesRepository.GetHouseDataById(id);
            if (houseData is not null)
            {
                response = await new HouseMessageBuilder(houseData).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }
        }
        else
        {
            var housesData = DofusApi.Datacenter.HousesRepository.GetHousesDataByName(value).ToList();
            if (housesData.Count == 1)
            {
                response = await new HouseMessageBuilder(housesData[0]).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }
            else if (housesData.Count > 1)
            {
                response = await new PaginatedHouseMessageBuilder(housesData, HouseSearchCategory.Name, value).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }
        }

        response ??= new DiscordInteractionResponseBuilder().WithContent(BotTranslations.House_NotFound);
        await ctx.RespondAsync(response);
    }


    [Command("coordonnees"), Description("Retourne une liste de maisons à partir de leurs coordonnées")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public static async Task CoordinateExecuteAsync(SlashCommandContext ctx,
        [Parameter("x"), Description("Coordonnée x de la map de la maison")]
        [MinMaxValue(-666, 666)]
        int x,
        [Parameter("y"), Description("Coordonnée y de la map de la maison")]
        [MinMaxValue(-666, 666)]
        int y)
    {
        CommandManager.SetCulture();

        var housesData = DofusApi.Datacenter.HousesRepository.GetHousesDataByCoordinate(x, y).ToList();

        if (housesData.Count == 0)
        {
            await ctx.RespondAsync(Translation.Format(BotTranslations.House_NotFound_Coordinate, x, y));
        }
        else if (housesData.Count == 1)
        {
            await ctx.RespondAsync(await new HouseMessageBuilder(housesData[0]).GetMessageAsync<DiscordInteractionResponseBuilder>());
        }
        else
        {
            await ctx.RespondAsync(await new PaginatedHouseMessageBuilder(housesData, HouseSearchCategory.Coordinate, $"{x}{InteractionManager.PacketParameterSeparator}{y}")
                .GetMessageAsync<DiscordInteractionResponseBuilder>());
        }
    }


    [Command("sous-zone"), Description("Retourne une liste de maisons à partir de leur sous-zone")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public static async Task MapSubAreaExecuteAsync(SlashCommandContext ctx,
        [Parameter("nom"), Description("Nom de la sous-zone")]
        [SlashAutoCompleteProvider<MapSubAreaAutocompleteProvider>]
        [MinMaxLength(1, 70)]
        string value)
    {
        CommandManager.SetCulture();

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
            var housesData = DofusApi.Datacenter.HousesRepository.GetHousesDataByMapSubAreaId(id).ToList();

            if (housesData.Count == 0)
            {
                await ctx.RespondAsync(Translation.Format(BotTranslations.House_NotFound_MapSubArea, Formatter.Bold(mapSubAreaData.Name)));
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
        [MinMaxLength(1, 70)]
        string value)
    {
        CommandManager.SetCulture();

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
            var housesData = DofusApi.Datacenter.HousesRepository.GetHousesDataByMapAreaId(id).ToList();

            if (housesData.Count == 0)
            {
                await ctx.RespondAsync(Translation.Format(BotTranslations.House_NotFound_MapArea, Formatter.Bold(mapAreaData.Name)));
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
