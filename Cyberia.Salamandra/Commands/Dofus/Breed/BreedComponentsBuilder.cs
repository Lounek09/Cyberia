using Cyberia.Api.DatacenterNS;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus
{ 
    public static class BreedComponentsBuilder
    {
        public static DiscordButtonComponent BreedButtonBuilder(Breed breed, bool disable = false)
        {
            return new(ButtonStyle.Success, BreedMessageBuilder.GetPacket(breed.Id), breed.Name, disable);
        }
    }
}
