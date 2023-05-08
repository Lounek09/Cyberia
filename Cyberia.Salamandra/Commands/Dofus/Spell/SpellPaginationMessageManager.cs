using Cyberia.Api.DatacenterNS;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class SpellPaginationMessageBuilde : PaginationMessageBuilder
    {
        private readonly List<Spell> _spells;

        public SpellPaginationMessageBuilde(List<Spell> spells) :
            base(DofusEmbedCategory.Spells, "Livre de sorts", "Plusieurs sorts trouvés :")
        {
            _spells = spells;
            PopulateContent();
        }

        protected override void PopulateContent()
        {
            foreach (Spell spell in _spells)
            {
                int level = spell.GetNeededLevel();
                _content.Add($"- {(level == 0 ? "" : $"Niv.{level}")} {Formatter.Bold(spell.Name)} ({spell.Id})");
            }
        }

        protected override DiscordSelectComponent SelectBuilder()
        {
            HashSet<DiscordSelectComponentOption> options = new();
            foreach (Spell spell in _spells.GetRange(GetStartPageIndex(), GetEndPageIndex()))
                options.Add(new(spell.Name.WithMaxLength(100), spell.Id.ToString(), spell.Id.ToString()));

            return new("select", "Sélectionne un sort pour l'afficher", options);
        }

        protected override async Task<bool> InteractionTreatment(ComponentInteractionCreateEventArgs e)
        {
            if (await base.InteractionTreatment(e))
                return true;

            if (e.Id.Equals("select"))
            {
                int id = Convert.ToInt32(e.Interaction.Data.Values.First());
                Spell? spell = Bot.Instance.Api.Datacenter.SpellsData.GetSpellById(id);

                if (spell is not null)
                {
                    await new SpellMessageBuilder(spell).UpdateInteractionResponse(e.Interaction);
                    return true;
                }
            }

            await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
            return false;
        }
    }
}
