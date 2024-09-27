using Cyberia.Api.Data.Houses;
using Cyberia.Salamandra.Formatters;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus.House;

public static class HouseComponentsBuilder
{
    public static DiscordButtonComponent HouseButtonBuilder(HouseData houseData, bool disable = false)
    {
        return new(DiscordButtonStyle.Success, HouseMessageBuilder.GetPacket(houseData.Id), houseData.Name, disable);
    }

    public static DiscordSelectComponent HousesSelectBuilder(int uniqueIndex, IEnumerable<HouseData> housesData, bool disable = false)
    {
        var options = housesData
            .Take(Constant.MaxSelectOption)
            .Select(x =>
            {
                return new DiscordSelectComponentOption(
                    StringExtensions.WithMaxLength(x.Name, 100),
                    HouseMessageBuilder.GetPacket(x.Id),
                    x.Id.ToString());
            });

        return new(PacketFormatter.Select(uniqueIndex), BotTranslations.Select_House_Placeholder, options, disable);
    }
}
