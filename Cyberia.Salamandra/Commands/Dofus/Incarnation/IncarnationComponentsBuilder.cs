using Cyberia.Api.DatacenterNS;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public static class IncarnationComponentsBuilder
    {
        public static DiscordButtonComponent IncarnationButtonBuilder(Incarnation incarnation, bool disable = false)
        {
            return new(ButtonStyle.Success, IncarnationMessageBuilder.GetPacket(incarnation.Id), incarnation.Name, disable);
        }

        public static DiscordSelectComponent IncarnationsSelectBuilder(int index, List<Incarnation> incarnations, bool disable = false)
        {
            IEnumerable<DiscordSelectComponentOption> options = incarnations.Select(x => new DiscordSelectComponentOption(x.Name.WithMaxLength(100), IncarnationMessageBuilder.GetPacket(x.Id), x.Id.ToString()));

            return new(InteractionManager.SelectComponentPacketBuilder(index), "Sélectionne une incarnation pour l'afficher", options, disable);
        }
    }
}
