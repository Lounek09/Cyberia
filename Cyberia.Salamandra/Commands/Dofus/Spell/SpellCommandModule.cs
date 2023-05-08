using Cyberia.Api.DatacenterNS;

using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class SpellCommandModule : ApplicationCommandModule
    {
        [SlashCommand("sort", "Retourne les informations d'un sort à partir de son nom")]
        public async Task Command(InteractionContext ctx,
            [Option("nom", "Nom du sort")]
            [Autocomplete(typeof(SpellAutocompleteProvider))]
            string sId)
        {
            Spell? spell = null;

            if (int.TryParse(sId, out int id))
                spell = Bot.Instance.Api.Datacenter.SpellsData.GetSpellById(id);

            if (spell is null)
                await ctx.CreateResponseAsync("Sort introuvable");
            else
                await new SpellMessageBuilder(spell).SendInteractionResponse(ctx.Interaction);
        }
    }
}
