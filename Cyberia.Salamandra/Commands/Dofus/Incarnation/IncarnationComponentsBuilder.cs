using Cyberia.Api.Data;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus;

public static class IncarnationComponentsBuilder
{
    public static DiscordButtonComponent IncarnationButtonBuilder(IncarnationData incarnationData, bool disable = false)
    {
        return new(ButtonStyle.Success, IncarnationMessageBuilder.GetPacket(incarnationData.Id), incarnationData.Name, disable);
    }

    public static DiscordSelectComponent IncarnationsSelectBuilder(int index, List<IncarnationData> incarnationsData, bool disable = false)
    {
        var options = incarnationsData.Select(x => new DiscordSelectComponentOption(x.Name.WithMaxLength(100), IncarnationMessageBuilder.GetPacket(x.Id), x.Id.ToString()));

        return new(InteractionManager.SelectComponentPacketBuilder(index), "Sélectionne une incarnation pour l'afficher", options, disable);
    }
}
