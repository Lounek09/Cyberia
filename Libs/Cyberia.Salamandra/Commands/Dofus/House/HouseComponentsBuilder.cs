using Cyberia.Api.Data.Houses;
using Cyberia.Salamandra.Formatters;

using DSharpPlus.Entities;

using System.Globalization;

namespace Cyberia.Salamandra.Commands.Dofus.House;

public static class HouseComponentsBuilder
{
    public static DiscordButtonComponent HouseButtonBuilder(HouseData houseData, CultureInfo? culture, bool disable = false)
    {
        return new DiscordButtonComponent(
            DiscordButtonStyle.Success,
            HouseMessageBuilder.GetPacket(houseData.Id),
            houseData.Name.ToString(culture),
            disable);
    }

    public static DiscordSelectComponent HousesSelectBuilder(int uniqueIndex, IEnumerable<HouseData> housesData, CultureInfo? culture, bool disable = false)
    {
        var options = housesData
            .Take(Constant.MaxSelectOption)
            .Select(x =>
            {
                return new DiscordSelectComponentOption(
                    x.Name.ToString(culture).WithMaxLength(100),
                    HouseMessageBuilder.GetPacket(x.Id),
                    x.Id.ToString());
            });

        return new DiscordSelectComponent(
            PacketFormatter.Select(uniqueIndex),
            Translation.Get<BotTranslations>("Select.House.Placeholder", culture),
            options,
            disable);
    }
}
