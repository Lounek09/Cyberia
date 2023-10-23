using Cyberia.Api.DatacenterNS;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class BreedCommandModule : ApplicationCommandModule
    {
        [SlashCommand("classe", "Retourne les informations d'une classe à partir de son nom")]
        public async Task Command(InteractionContext ctx,
            [Option("nom", "Nom de la classe")]
            [ChoiceProvider(typeof(BreedChoiceProvider))]
            string breedName)
        {
            BreedData? breedData = Bot.Instance.Api.Datacenter.BreedsData.GetBreedDataByName(breedName);

            if (breedData is null)
            {
                await ctx.CreateResponseAsync("Classe introuvable");
            }
            else
            {
                await ctx.CreateResponseAsync(await new BreedMessageBuilder(breedData).GetMessageAsync<DiscordInteractionResponseBuilder>());
            }
        }
    }
}
