using Cyberia.Api.DatacenterNS;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class SpellCommandModule : ApplicationCommandModule
    {
        [SlashCommand("sort", "Retourne les informations d'un sort à partir de son nom")]
        public async Task Command(InteractionContext ctx,
            [Option("nom", "Nom du sort", true)]
            [Autocomplete(typeof(SpellAutocompleteProvider))]
            string value)
        {
            DiscordInteractionResponseBuilder? response = null;

            if (int.TryParse(value, out int id))
            {
                Spell? spell = Bot.Instance.Api.Datacenter.SpellsData.GetSpellById(id);
                if (spell is not null)
                    response = await new SpellMessageBuilder(spell, spell.GetMaxLevelNumber()).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }
            else
            {
                List<Spell> spells = Bot.Instance.Api.Datacenter.SpellsData.GetSpellsByName(value);
                if (spells.Count == 1)
                    response = await new SpellMessageBuilder(spells[0], spells[0].GetMaxLevelNumber()).GetMessageAsync<DiscordInteractionResponseBuilder>();
                else if (spells.Count > 1)
                    response = await new PaginatedSpellMessageBuilder(spells, value).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }

            response ??= new DiscordInteractionResponseBuilder().WithContent("Sort introuvable");
            await ctx.CreateResponseAsync(response);
        }
    }
}
