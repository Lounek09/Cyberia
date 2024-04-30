using Cyberia.Api.Data.Breeds;

using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus.Breed;

public static class BreedComponentsBuilder
{
    public static DiscordButtonComponent BreedButtonBuilder(BreedData breedData, bool disable = false)
    {
        return new(DiscordButtonStyle.Success, BreedMessageBuilder.GetPacket(breedData.Id), breedData.Name, disable);
    }
}
