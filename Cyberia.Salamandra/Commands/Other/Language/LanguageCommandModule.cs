using Cyberia.Database.Models;
using Cyberia.Database.Repositories;
using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Commands.Processors.SlashCommands.Localization;
using DSharpPlus.Commands.Processors.SlashCommands.Metadata;
using DSharpPlus.Entities;

using System.ComponentModel;

namespace Cyberia.Salamandra.Commands.Other.Language;

public sealed class LanguageCommandModule
{
    private readonly CultureService _cultureService;
    private readonly DiscordCachedUserRepository _discordCachedUserRepository;

    public LanguageCommandModule(CultureService cultureService, DiscordCachedUserRepository discordCachedUserRepository)
    {
        _cultureService = cultureService;
        _discordCachedUserRepository = discordCachedUserRepository;
    }

    [Command(LanguageInteractionLocalizer.CommandName), Description(LanguageInteractionLocalizer.CommandDescription)]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall, DiscordApplicationIntegrationType.UserInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild, DiscordInteractionContextType.PrivateChannel)]
    [InteractionLocalizer<LanguageInteractionLocalizer>]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public async Task ExecuteAsync(SlashCommandContext ctx,
        [Parameter(LanguageInteractionLocalizer.Value_ParameterName), Description(LanguageInteractionLocalizer.Value_ParameterDescription)]
        [InteractionLocalizer<LanguageInteractionLocalizer>]
        [SlashChoiceProvider<LanguageChoiceProvider>]
        string value)
    {
        await _discordCachedUserRepository.CreateOrUpdateAsync(new DiscordCachedUser()
        {
            Id = ctx.User.Id,
            Locale = value
        });

        var culture = await _cultureService.GetCultureAsync(ctx.Interaction);

        await ctx.RespondAsync(Translation.Format(Translation.Get<BotTranslations>("Language.Set", culture), Formatter.Bold(value)), true);
    }
}

