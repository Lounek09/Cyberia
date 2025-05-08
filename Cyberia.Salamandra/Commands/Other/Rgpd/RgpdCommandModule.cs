using Cyberia.Database.Repositories;
using Cyberia.Salamandra.Services;

using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.Localization;
using DSharpPlus.Commands.Processors.SlashCommands.Metadata;
using DSharpPlus.Entities;

using System.ComponentModel;
using System.Text;
using System.Text.Json;

namespace Cyberia.Salamandra.Commands.Other.Rgpd;

[Command(RgpdInteractionLocalizer.CommandName), Description(RgpdInteractionLocalizer.CommandDescription)]
[InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall, DiscordApplicationIntegrationType.UserInstall)]
[InteractionAllowedContexts(DiscordInteractionContextType.Guild, DiscordInteractionContextType.PrivateChannel)]
[InteractionLocalizer<RgpdInteractionLocalizer>]
public sealed class RgpdCommandModule
{
    private static readonly JsonSerializerOptions s_jsonOptions = new()
    {
        WriteIndented = true
    };

    private readonly ICultureService _cultureService;
    private readonly DiscordCachedUserRepository _discordCachedUserRepository;

    public RgpdCommandModule(ICultureService cultureService, DiscordCachedUserRepository discordCachedUserRepository)
    {
        _cultureService = cultureService;
        _discordCachedUserRepository = discordCachedUserRepository;
    }

    [Command(RgpdInteractionLocalizer.Get_CommandName), Description(RgpdInteractionLocalizer.Get_CommandDescription)]
    [InteractionLocalizer<RgpdInteractionLocalizer>]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public async Task GetExecuteAsync(SlashCommandContext ctx)
    {
        var culture = await _cultureService.GetCultureAsync(ctx.Interaction);

        var user = await _discordCachedUserRepository.GetAsync(ctx.Interaction.User.Id);
        if (user is null)
        {
            await ctx.RespondAsync(Translation.Get<BotTranslations>("Rgpd.NoData", culture), true);
            return;
        }

        var json = JsonSerializer.Serialize(user, s_jsonOptions);
        using MemoryStream stream = new(Encoding.UTF8.GetBytes(json));

        var message = new DiscordInteractionResponseBuilder()
            .WithContent(Translation.Get<BotTranslations>("Rgpd.Data", culture))
            .AddFile("data.json", stream)
            .AsEphemeral();

        await ctx.RespondAsync(message);
    }

    [Command(RgpdInteractionLocalizer.Delete_CommandName), Description(RgpdInteractionLocalizer.Delete_CommandDescription)]
    [InteractionLocalizer<RgpdInteractionLocalizer>]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public async Task DeleteExecuteAsync(SlashCommandContext ctx)
    {
        var culture = await _cultureService.GetCultureAsync(ctx.Interaction);

        var success = await _discordCachedUserRepository.DeleteAsync(ctx.Interaction.User.Id);
        if (success)
        {
            await ctx.RespondAsync(Translation.Get<BotTranslations>("Rgpd.Deleted", culture), true);
            return;
        }

        await ctx.RespondAsync(Translation.Get<BotTranslations>("Rgpd.NoData", culture), true);
    }
}

