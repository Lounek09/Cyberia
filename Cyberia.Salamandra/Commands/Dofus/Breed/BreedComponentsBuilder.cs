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
}
