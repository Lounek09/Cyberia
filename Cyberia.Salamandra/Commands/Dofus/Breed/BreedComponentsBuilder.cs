using Cyberia.Api.Data;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus;

public static class BreedComponentsBuilder
{
    public static DiscordButtonComponent BreedButtonBuilder(BreedData breedData, bool disable = false)
    {
        return new(ButtonStyle.Success, BreedMessageBuilder.GetPacket(breedData.Id), breedData.Name, disable);
    }
}
