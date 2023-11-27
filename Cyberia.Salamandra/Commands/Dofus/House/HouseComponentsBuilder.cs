using Cyberia.Api.Data;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus;

public static class HouseComponentsBuilder
{
    public static DiscordButtonComponent HouseButtonBuilder(HouseData houseData, bool disable = false)
    {
        return new(ButtonStyle.Success, HouseMessageBuilder.GetPacket(houseData.Id), houseData.Name, disable);
    }

    public static DiscordSelectComponent HousesSelectBuilder(int uniqueIndex, List<HouseData> housesData, bool disable = false)
    {
        var options = housesData.Select(x => new DiscordSelectComponentOption(x.Name.WithMaxLength(100), HouseMessageBuilder.GetPacket(x.Id), x.Id.ToString()));

        return new(InteractionManager.SelectComponentPacketBuilder(uniqueIndex), "Sélectionne une maison pour l'afficher", options, disable);
    }
}
