using Cyberia.Api.Data.Spells;
using Cyberia.Api.Extensions;
using Cyberia.Salamandra.Formatters;

using DSharpPlus.Entities;

using System.Globalization;

namespace Cyberia.Salamandra.Commands.Dofus.Spell;

public static class SpellComponentsBuilder
{
    public static DiscordButtonComponent SpellButtonBuilder(SpellData spell, CultureInfo? culture, bool disable = false)
    {
        return new DiscordButtonComponent(
            DiscordButtonStyle.Success,
            SpellMessageBuilder.GetPacket(spell.Id, spell.GetMaxLevelNumber()),
            spell.Name.ToString(culture),
            disable);
    }

    public static DiscordSelectComponent SpellsSelectBuilder(int index, IEnumerable<SpellData> spells, CultureInfo? culture, bool disable = false)
    {
        var options = spells
            .Take(Constant.MaxSelectOption)
            .Select(x =>
            {
                return new DiscordSelectComponentOption(
                    x.Name.ToString(culture).WithMaxLength(100),
                    SpellMessageBuilder.GetPacket(x.Id, x.GetMaxLevelNumber()),
                    x.SpellCategory.GetDescription(culture));
            });

        return new DiscordSelectComponent(
            PacketFormatter.Select(index),
            Translation.Get<BotTranslations>("Select.Spell.Placeholder", culture),
            options,
            disable);
    }
}
