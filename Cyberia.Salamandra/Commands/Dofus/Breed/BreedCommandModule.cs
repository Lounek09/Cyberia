using Cyberia.Api;
using Cyberia.Salamandra.Services;

using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Commands.Processors.SlashCommands.Localization;
using DSharpPlus.Commands.Processors.SlashCommands.Metadata;
using DSharpPlus.Entities;

using System.ComponentModel;

namespace Cyberia.Salamandra.Commands.Dofus.Breed;

public sealed class BreedCommandModule
{
    private readonly CultureService _cultureService;

    public BreedCommandModule(CultureService cultureService)
    {
        _cultureService = cultureService;
    }

    [Command(BreedInteractionLocalizer.CommandName), Description(BreedInteractionLocalizer.CommandDescription)]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall, DiscordApplicationIntegrationType.UserInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild, DiscordInteractionContextType.PrivateChannel)]
    [InteractionLocalizer<BreedInteractionLocalizer>]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public async Task ExecuteAsync(SlashCommandContext ctx,
        [Parameter(BreedInteractionLocalizer.BreedId_ParameterName), Description(BreedInteractionLocalizer.BreedId_ParameterDescription)]
        [InteractionLocalizer<BreedInteractionLocalizer>]
        [SlashAutoCompleteProvider<BreedAutocompleteProvider>]
        int breedId)
    {
        await _cultureService.SetCultureAsync(ctx.Interaction);

        var breedData = DofusApi.Datacenter.BreedsRepository.GetBreedDataById(breedId);

        if (breedData is null)
        {
            await ctx.RespondAsync(BotTranslations.Breed_NotFound);
            return;
        }

        await ctx.RespondAsync(await new BreedMessageBuilder(breedData).BuildAsync<DiscordInteractionResponseBuilder>());
    }
}
