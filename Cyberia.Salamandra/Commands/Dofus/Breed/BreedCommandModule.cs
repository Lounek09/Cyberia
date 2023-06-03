using Cyberia.Api.DatacenterNS;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus
{
#pragma warning disable CA1822 // Mark members as static
    public sealed class BreedCommandModule : ApplicationCommandModule
    {
        [SlashCommand("classe", "Retourne les informations d'une classe à partir de son nom")]
        public async Task Command(InteractionContext ctx,
            [Option("nom", "Nom de la classe")]
            [ChoiceProvider(typeof(BreedChoiceProvider))]
            string breedName)
        {
            Breed? breed = Bot.Instance.Api.Datacenter.BreedsData.GetBreedByName(breedName);

            if (breed is null)
                await ctx.CreateResponseAsync("Classe introuvable");
            else
                await ctx.CreateResponseAsync(await new BreedMessageBuilder(breed).GetMessageAsync<DiscordInteractionResponseBuilder>());
        }
    }
}
