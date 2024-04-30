using Cyberia.Api;

using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Commands.Processors.SlashCommands.Metadata;
using DSharpPlus.Entities;

using System.ComponentModel;

namespace Cyberia.Salamandra.Commands.Dofus.Breed;

public sealed class BreedCommandModule
{
    [Command("classe"), Description("Retourne les informations d'une classe à partir de son nom")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall, DiscordApplicationIntegrationType.UserInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild, DiscordInteractionContextType.PrivateChannel)]
    public static async Task ExecuteAsync(SlashCommandContext ctx,
        [Parameter("nom"), Description("Nom de la classe")]
        [SlashChoiceProvider<BreedChoiceProvider>]
        int breedId)
    {
        var breedData = DofusApi.Datacenter.BreedsData.GetBreedDataById(breedId);

        if (breedData is null)
        {
            await ctx.RespondAsync("Classe introuvable");
        }
        else
        {
            await ctx.RespondAsync(await new BreedMessageBuilder(breedData)
                .GetMessageAsync<DiscordInteractionResponseBuilder>());
        }
    }
}
