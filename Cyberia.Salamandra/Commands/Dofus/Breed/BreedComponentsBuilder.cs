using Cyberia.Api.Data.Breeds;

using DSharpPlus.Entities;

using System.Globalization;

namespace Cyberia.Salamandra.Commands.Dofus.Breed;

public static class BreedComponentsBuilder
{
    public static DiscordButtonComponent BreedButtonBuilder(BreedData breedData, CultureInfo? culture, bool disable = false)
    {
        return new DiscordButtonComponent(
            DiscordButtonStyle.Success,
            BreedMessageBuilder.GetPacket(breedData.Id),
            breedData.Name.ToString(culture),
            disable);
    }

    public static DiscordButtonComponent GladiatroolBreedButtonBuilder(BreedData breedData, bool displayBreedName, CultureInfo? culture, bool disable = false)
    {
        var label = displayBreedName
            ? $"{breedData.Name.ToString(culture)} - {Translation.Get<BotTranslations>("Gladiatrool", culture)}"
            : Translation.Get<BotTranslations>("Gladiatrool", culture);

        return new DiscordButtonComponent(
            DiscordButtonStyle.Success,
            GladiatroolBreedMessageBuilder.GetPacket(breedData.Id),
            label,
            disable);
    }
}
