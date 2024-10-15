using Cyberia.Api;
using Cyberia.Api.Data.Incarnations;
using Cyberia.Salamandra.Formatters;

using DSharpPlus.Entities;

using System.Globalization;

namespace Cyberia.Salamandra.Commands.Dofus.Incarnation;

public static class IncarnationComponentsBuilder
{
    public static DiscordButtonComponent IncarnationButtonBuilder(IncarnationData incarnationData, CultureInfo? culture, bool disable = false)
    {
        var itemName = DofusApi.Datacenter.ItemsRepository.GetItemNameById(incarnationData.Id, culture);

        return new DiscordButtonComponent(
            DiscordButtonStyle.Success,
            IncarnationMessageBuilder.GetPacket(incarnationData.Id),
            itemName,
            disable);
    }

    public static DiscordSelectComponent IncarnationsSelectBuilder(int index, IEnumerable<IncarnationData> incarnationsData, CultureInfo? culture, bool disable = false)
    {
        var options = incarnationsData
            .Take(Constant.MaxSelectOption)
            .Select(x =>
            {
                var itemName = DofusApi.Datacenter.ItemsRepository.GetItemNameById(x.Id, culture);
                return new DiscordSelectComponentOption(
                    itemName.WithMaxLength(100),
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
