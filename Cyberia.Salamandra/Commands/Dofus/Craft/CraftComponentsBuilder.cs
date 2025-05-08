using Cyberia.Api.Data.Crafts;
using Cyberia.Salamandra.Formatters;

using DSharpPlus.Entities;

using System.Globalization;

namespace Cyberia.Salamandra.Commands.Dofus.Craft;

public static class CraftComponentsBuilder
{
    public static DiscordButtonComponent CraftButtonBuilder(CraftData craftData, int quantity, CultureInfo? culture, bool disable = false)
    {
        return new DiscordButtonComponent(
            DiscordButtonStyle.Success,
            CraftMessageBuilder.GetPacket(craftData.Id, quantity),
            Translation.Get<BotTranslations>("Button.Craft", culture),
            disable);
    }

    public static DiscordSelectComponent CraftsSelectBuilder(int uniqueIndex, IEnumerable<CraftData> craftsData, int quantity, CultureInfo? culture, bool disable = false)
    {
        var options = craftsData
            .Take(Constant.MaxSelectOption)
            .Select(x =>
            {
                return new DiscordSelectComponentOption(
                    x.GetItemName(culture).WithMaxLength(100),
                    CraftMessageBuilder.GetPacket(x.Id, quantity),
                    x.Id.ToString());
            });

        return new DiscordSelectComponent(
            PacketFormatter.Select(uniqueIndex),
            Translation.Get<BotTranslations>("Select.Craft.Placeholder", culture),
            options,
            disable);
    }
}
