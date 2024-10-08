﻿using Cyberia.Api.Data.Spells;
using Cyberia.Api.Extensions;
using Cyberia.Salamandra.Formatters;

using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus.Spell;

public static class SpellComponentsBuilder
{
    public static DiscordButtonComponent SpellButtonBuilder(SpellData spell, bool disable = false)
    {
        return new(DiscordButtonStyle.Success, SpellMessageBuilder.GetPacket(spell.Id, spell.GetMaxLevelNumber()), spell.Name, disable);
    }

    public static DiscordSelectComponent SpellsSelectBuilder(int index, IEnumerable<SpellData> spells, bool disable = false)
    {
        var options = spells
            .Take(Constant.MaxSelectOption)
            .Select(x =>
            {
                return new DiscordSelectComponentOption(
                    StringExtensions.WithMaxLength(x.Name, 100),
                    SpellMessageBuilder.GetPacket(x.Id, x.GetMaxLevelNumber()),
                    x.SpellCategory.GetDescription());
            });

        return new(PacketFormatter.Select(index), BotTranslations.Select_Spell_Placeholder, options, disable);
    }
}
