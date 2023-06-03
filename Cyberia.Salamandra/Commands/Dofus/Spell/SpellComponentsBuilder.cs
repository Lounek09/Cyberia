using Cyberia.Api.DatacenterNS;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public static class SpellComponentsBuilder
    {
        public static DiscordButtonComponent SpellButtonBuilder(Spell spell, bool disable = false)
        {
            return new(ButtonStyle.Success, SpellMessageBuilder.GetPacket(spell.Id, spell.GetMaxLevelNumber()), spell.Name, disable);
        }

        public static DiscordSelectComponent SpellsSelectBuilder(int index, List<Spell> spells, bool disable = false)
        {
            IEnumerable<DiscordSelectComponentOption> options = spells.Select(x =>
            {
                string spellCategoryName = x.GetSpellCategory()?.Name ?? "";
                return new DiscordSelectComponentOption(x.Name.WithMaxLength(100), SpellMessageBuilder.GetPacket(x.Id, x.GetMaxLevelNumber()), spellCategoryName);
            });

            return new(InteractionManager.SelectComponentPacketBuilder(index), "Sélectionne un sort pour l'afficher", options, disable);
        }
    }
}
