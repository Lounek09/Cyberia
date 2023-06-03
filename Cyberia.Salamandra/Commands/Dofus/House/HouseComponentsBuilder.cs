using Cyberia.Api.DatacenterNS;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus
{ 
    public static class HouseComponentsBuilder
    {
        public static DiscordButtonComponent HouseButtonBuilder(House house, bool disable = false)
        {
            return new(ButtonStyle.Success, HouseMessageBuilder.GetPacket(house.Id), house.Name, disable);
        }

        public static DiscordSelectComponent HousesSelectBuilder(int uniqueIndex, List<House> houses, bool disable = false)
        {
            IEnumerable<DiscordSelectComponentOption> options = houses.Select(x => new DiscordSelectComponentOption(x.Name.WithMaxLength(100), HouseMessageBuilder.GetPacket(x.Id), x.Id.ToString()));
            
            return new(InteractionManager.SelectComponentPacketBuilder(uniqueIndex), "Sélectionne une maison pour l'afficher", options, disable);
        }
    }
}
