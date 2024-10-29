using Cyberia.Api;
using Cyberia.Salamandra.Services;

using DSharpPlus.Commands;
using DSharpPlus.Commands.ContextChecks;
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
    private readonly EmbedBuilderService _embedBuilderService;

    public BreedCommandModule(CultureService cultureService, EmbedBuilderService embedBuilderService)
    {
        _cultureService = cultureService;
        _embedBuilderService = embedBuilderService;
    }

    [Command(BreedInteractionLocalizer.CommandName), Description(BreedInteractionLocalizer.CommandDescription)]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall, DiscordApplicationIntegrationType.UserInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild, DiscordInteractionContextType.PrivateChannel)]
    [InteractionLocalizer<BreedInteractionLocalizer>]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [RequirePermissions(DiscordPermissions.UseApplicationCommands)]
    public async Task ExecuteAsync(SlashCommandContext ctx,
        [Parameter(BreedInteractionLocalizer.BreedId_ParameterName), Description(BreedInteractionLocalizer.BreedId_ParameterDescription)]
        [InteractionLocalizer<BreedInteractionLocalizer>]
        [SlashAutoCompleteProvider<BreedAutocompleteProvider>]
        int breedId)
    {
        var culture = await _cultureService.GetCultureAsync(ctx.Interaction);

        var breedData = DofusApi.Datacenter.BreedsRepository.GetBreedDataById(breedId);

        if (breedData is null)
        {
            await ctx.RespondAsync(Translation.Get<BotTranslations>("Breed.NotFound", culture));
            return;
        }

        await ctx.RespondAsync(await new BreedMessageBuilder(_embedBuilderService, breedData, culture)
            .BuildAsync<DiscordInteractionResponseBuilder>());
    }
}
