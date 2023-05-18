using Cyberia.Api.DatacenterNS;

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
            List<Breed> breeds = Bot.Instance.Api.Datacenter.BreedsData.GetBreedsByName(breedName);

            if (breeds.Count > 0)
                await new BreedMessageBuilder(breeds[0]).SendInteractionResponse(ctx.Interaction);
            else
                await ctx.CreateResponseAsync("Classe introuvable");
        }
    }
}
