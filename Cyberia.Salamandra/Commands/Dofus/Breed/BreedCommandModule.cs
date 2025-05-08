using Cyberia.Api.Data;
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
    private readonly ICultureService _cultureService;
    private readonly DofusDatacenter _dofusDatacenter;
    private readonly IEmbedBuilderService _embedBuilderService;

    public BreedCommandModule(ICultureService cultureService, DofusDatacenter dofusDatacenter, IEmbedBuilderService embedBuilderService)
    {
        _cultureService = cultureService;
        _dofusDatacenter = dofusDatacenter;
        _embedBuilderService = embedBuilderService;
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
        int breedId,
        [Parameter(BreedInteractionLocalizer.Gladiatrool_ParameterName), Description(BreedInteractionLocalizer.Gladiatrool_ParameterDescription)]
        [InteractionLocalizer<BreedInteractionLocalizer>]
        bool gladiatrool = false)
    {
        var culture = await _cultureService.GetCultureAsync(ctx.Interaction);

        var breedData = _dofusDatacenter.BreedsRepository.GetBreedDataById(breedId);

        if (breedData is null)
        {
            await ctx.RespondAsync(Translation.Get<BotTranslations>("Breed.NotFound", culture));
            return;
        }

        var message = gladiatrool
            ? await new GladiatroolBreedMessageBuilder(_embedBuilderService, breedData, culture).BuildAsync<DiscordInteractionResponseBuilder>()
            : await new BreedMessageBuilder(_embedBuilderService, breedData, culture).BuildAsync<DiscordInteractionResponseBuilder>();
        
        await ctx.RespondAsync(message);
    }
}
