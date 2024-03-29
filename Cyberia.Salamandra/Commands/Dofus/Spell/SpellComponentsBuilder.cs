using Cyberia.Api.Data.Spells;
using Cyberia.Api.Values;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus;

public static class SpellComponentsBuilder
{
    public static DiscordButtonComponent SpellButtonBuilder(SpellData spell, bool disable = false)
    {
        return new(ButtonStyle.Success, SpellMessageBuilder.GetPacket(spell.Id, spell.GetMaxLevelNumber()), spell.Name, disable);
    }

    public static DiscordSelectComponent SpellsSelectBuilder(int index, IEnumerable<SpellData> spells, bool disable = false)
    {
        var options = spells
            .Take(Constant.MAX_SELECT_OPTION)
            .Select(x =>
        {
            return new DiscordSelectComponentOption(
                x.Name.WithMaxLength(100),
                SpellMessageBuilder.GetPacket(x.Id, x.GetMaxLevelNumber()),
                x.SpellCategory.GetDescription());
        });

        return new(InteractionManager.SelectComponentPacketBuilder(index), "Sélectionne un sort pour l'afficher", options, disable);
    }
}
