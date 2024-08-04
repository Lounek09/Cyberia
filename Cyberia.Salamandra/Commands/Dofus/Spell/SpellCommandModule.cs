using Cyberia.Api;
using Cyberia.Salamandra.Managers;

using DSharpPlus.Commands;
using DSharpPlus.Commands.ArgumentModifiers;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Commands.Processors.SlashCommands.Metadata;
using DSharpPlus.Entities;

using System.ComponentModel;

namespace Cyberia.Salamandra.Commands.Dofus.Spell;

public sealed class SpellCommandModule
{
    [Command("sort"), Description("Retourne les informations d'un sort à partir de son nom")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall, DiscordApplicationIntegrationType.UserInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild, DiscordInteractionContextType.PrivateChannel)]
    public static async Task ExecuteAsync(SlashCommandContext ctx,
        [Parameter("nom"), Description("Nom du sort")]
        [SlashAutoCompleteProvider<SpellAutocompleteProvider>]
        [MinMaxLength(1, 70)]
        string value)
    {
        CommandManager.SetCulture();

        DiscordInteractionResponseBuilder? response = null;

        if (int.TryParse(value, out var id))
        {
            var spellData = DofusApi.Datacenter.SpellsRepository.GetSpellDataById(id);
            if (spellData is not null)
            {
                response = await new SpellMessageBuilder(spellData, spellData.GetMaxLevelNumber()).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }
        }
        else
        {
            var spellsData = DofusApi.Datacenter.SpellsRepository.GetSpellsDataByName(value).ToList();
            if (spellsData.Count == 1)
            {
                response = await new SpellMessageBuilder(spellsData[0], spellsData[0].GetMaxLevelNumber()).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }
            else if (spellsData.Count > 1)
            {
                response = await new PaginatedSpellMessageBuilder(spellsData, value).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }
        }

        response ??= new DiscordInteractionResponseBuilder().WithContent(BotTranslations.Spell_NotFound);
        await ctx.RespondAsync(response);
    }
}
