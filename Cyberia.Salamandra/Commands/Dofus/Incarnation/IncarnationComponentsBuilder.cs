using Cyberia.Api;
using Cyberia.Api.Data.Incarnations;
using Cyberia.Salamandra.Formatters;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus.Incarnation;

public static class IncarnationComponentsBuilder
{
    public static DiscordButtonComponent IncarnationButtonBuilder(IncarnationData incarnationData, bool disable = false)
    {
        var itemName = DofusApi.Datacenter.ItemsRepository.GetItemNameById(incarnationData.Id);
        return new(DiscordButtonStyle.Success, IncarnationMessageBuilder.GetPacket(incarnationData.Id), itemName, disable);
    }

    public static DiscordSelectComponent IncarnationsSelectBuilder(int index, IEnumerable<IncarnationData> incarnationsData, bool disable = false)
    {
        var options = incarnationsData
            .Take(Constant.MaxSelectOption)
            .Select(x =>
            {
                var itemName = DofusApi.Datacenter.ItemsRepository.GetItemNameById(x.Id);
                return new DiscordSelectComponentOption(
                    itemName.WithMaxLength(100),
                    IncarnationMessageBuilder.GetPacket(x.Id),
                    x.Id.ToString());
            });

        return new(PacketFormatter.Select(index), BotTranslations.Select_Incarnation_Placeholder, options, disable);
    }
}
