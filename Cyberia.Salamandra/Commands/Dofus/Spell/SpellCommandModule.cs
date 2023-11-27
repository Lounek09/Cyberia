using Cyberia.Api;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus;

public sealed class SpellCommandModule : ApplicationCommandModule
{
    [SlashCommand("sort", "Retourne les informations d'un sort à partir de son nom")]
    public async Task Command(InteractionContext ctx,
        [Option("nom", "Nom du sort", true)]
        [Autocomplete(typeof(SpellAutocompleteProvider))]
        string value)
    {
        DiscordInteractionResponseBuilder? response = null;

        if (int.TryParse(value, out var id))
        {
            var spellData = DofusApi.Datacenter.SpellsData.GetSpellDataById(id);
            if (spellData is not null)
            {
                response = await new SpellMessageBuilder(spellData, spellData.GetMaxLevelNumber()).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }
        }
        else
        {
            var spellsData = DofusApi.Datacenter.SpellsData.GetSpellsDataByName(value).ToList();
            if (spellsData.Count == 1)
            {
                response = await new SpellMessageBuilder(spellsData[0], spellsData[0].GetMaxLevelNumber()).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }
            else if (spellsData.Count > 1)
            {
                response = await new PaginatedSpellMessageBuilder(spellsData, value).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }
        }

        response ??= new DiscordInteractionResponseBuilder().WithContent("Sort introuvable");
        await ctx.CreateResponseAsync(response);
    }
}
