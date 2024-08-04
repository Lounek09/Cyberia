using Cyberia.Api.Data.Incarnations;
using Cyberia.Salamandra.Managers;

using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus.Incarnation;

public static class IncarnationComponentsBuilder
{
    public static DiscordButtonComponent IncarnationButtonBuilder(IncarnationData incarnationData, bool disable = false)
    {
        return new(DiscordButtonStyle.Success, IncarnationMessageBuilder.GetPacket(incarnationData.Id), incarnationData.Name, disable);
    }

    public static DiscordSelectComponent IncarnationsSelectBuilder(int index, IEnumerable<IncarnationData> incarnationsData, bool disable = false)
    {
        var options = incarnationsData
            .Take(Constant.MaxSelectOption)
            .Select(x =>
            {
                return new DiscordSelectComponentOption(
                    ExtendString.WithMaxLength(x.Name, 100),
                    IncarnationMessageBuilder.GetPacket(x.Id),
                    x.Id.ToString());
            });

        return new(InteractionManager.SelectComponentPacketBuilder(index), BotTranslations.Select_Incarnation_Placeholder, options, disable);
    }
}
