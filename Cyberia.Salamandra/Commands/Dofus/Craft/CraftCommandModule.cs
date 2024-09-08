using Cyberia.Api;
using Cyberia.Salamandra.Managers;

using DSharpPlus.Commands;
using DSharpPlus.Commands.ArgumentModifiers;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Commands.Processors.SlashCommands.Metadata;
using DSharpPlus.Entities;

using System.ComponentModel;

namespace Cyberia.Salamandra.Commands.Dofus.Craft;

public sealed class CraftCommandModule
{
    [Command("craft"), Description("Permet de calculer les ressources nécessaires pour craft un objet")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall, DiscordApplicationIntegrationType.UserInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild, DiscordInteractionContextType.PrivateChannel)]
    public static async Task ExecuteAsync(SlashCommandContext ctx,
        [Parameter("nom"), Description("Nom de l'item à craft")]
        [SlashAutoCompleteProvider<CraftAutocompleteProvider>]
        [MinMaxLength(1, 70)]
        string value,
        [Parameter("quantite"), Description("Quantité à craft")]
        [MinMaxValue(1, CraftMessageBuilder.MaxQuantity)]
        int quantity = 1)
    {
        CultureManager.SetCulture(ctx.Interaction);

        DiscordInteractionResponseBuilder? response = null;

        if (int.TryParse(value, out var id))
        {
            var craftData = DofusApi.Datacenter.CraftsRepository.GetCraftDataById(id);
            if (craftData is not null)
            {
                response = await new CraftMessageBuilder(craftData, quantity).BuildAsync<DiscordInteractionResponseBuilder>();
            }
        }
        else
        {
            var craftsData = DofusApi.Datacenter.CraftsRepository.GetCraftsDataByItemName(value).ToList();
            if (craftsData.Count == 1)
            {
                response = await new CraftMessageBuilder(craftsData[0], quantity).BuildAsync<DiscordInteractionResponseBuilder>();
            }
            else if (craftsData.Count > 1)
            {
                response = await new PaginatedCraftMessageBuilder(craftsData, value, quantity).BuildAsync<DiscordInteractionResponseBuilder>();
            }
        }

        response ??= new DiscordInteractionResponseBuilder().WithContent(BotTranslations.Craft_NotFound);
        await ctx.RespondAsync(response);
    }
}
