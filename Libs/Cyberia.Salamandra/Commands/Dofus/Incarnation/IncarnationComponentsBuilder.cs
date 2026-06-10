using Cyberia.Api.Data.Incarnations;
using Cyberia.Salamandra.Formatters;

using DSharpPlus.Entities;

using System.Globalization;

namespace Cyberia.Salamandra.Commands.Dofus.Incarnation;

public static class IncarnationComponentsBuilder
{
    public static DiscordButtonComponent IncarnationButtonBuilder(IncarnationData incarnationData, CultureInfo? culture, bool disable = false)
    {
        return new DiscordButtonComponent(
            DiscordButtonStyle.Success,
            IncarnationMessageBuilder.GetPacket(incarnationData.Id),
            incarnationData.GetItemName(culture),
            disable);
    }

    public static DiscordSelectComponent IncarnationsSelectBuilder(int index, IEnumerable<IncarnationData> incarnationsData, CultureInfo? culture, bool disable = false)
    {
        var options = incarnationsData
            .Take(Constant.MaxSelectOption)
            .Select(x =>
            {
                return new DiscordSelectComponentOption(
                    x.GetItemName(culture).WithMaxLength(100),
                    IncarnationMessageBuilder.GetPacket(x.Id),
                    x.Id.ToString());
            });

        return new DiscordSelectComponent(
            PacketFormatter.Select(index),
            Translation.Get<BotTranslations>("Select.Incarnation.Placeholder", culture),
            options,
            disable);
    }
}
