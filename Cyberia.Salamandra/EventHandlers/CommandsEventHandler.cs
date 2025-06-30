using Cyberia.Salamandra.Extensions.DSharpPlus;
using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.ContextChecks;
using DSharpPlus.Commands.EventArgs;
using DSharpPlus.Commands.Exceptions;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;

namespace Cyberia.Salamandra.EventHandlers;

/// <summary>
/// Represents the handler for events related to commands.
/// </summary>
public sealed class CommandsEventHandler : IEventHandler<CommandErroredEventArgs>
{
    private readonly ICachedChannelsManager _cachedChannelsManager;
    private readonly ICultureService _cultureService;
    private readonly IEmbedBuilderService _embedBuilderService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandsEventHandler"/> class.
    /// </summary>
    /// <param name="cachedChannelsManager">The service to get the channels from.</param>
    /// <param name="cultureService">The service to get the culture from.</param>
    /// <param name="embedBuilderService">The service to build embeds.</param>
    public CommandsEventHandler(ICachedChannelsManager cachedChannelsManager, ICultureService cultureService, IEmbedBuilderService embedBuilderService)
    {
        _cachedChannelsManager = cachedChannelsManager;
        _cultureService = cultureService;
        _embedBuilderService = embedBuilderService;
    }

    public async Task HandleEventAsync(CommandsExtension sender, CommandErroredEventArgs eventArgs)
    {
        var ctx = eventArgs.Context.As<SlashCommandContext>();
        var interaction = ctx.Interaction;
        var exception = eventArgs.Exception;
        var culture = await _cultureService.GetCultureAsync(interaction);

        // If the exception is a checks failed exception, we just need to respond to the user.
        if (exception is ChecksFailedException checkFailedException)
        {
            var message = string.Join('\n', checkFailedException.Errors.Select(x => x.ContextCheckAttribute switch
            {
                RequireApplicationOwnerAttribute => Translation.Get<BotTranslations>("Command.Error.Check.RequireApplicationOwner", culture),
                RequireGuildAttribute => Translation.Get<BotTranslations>("Command.Error.Check.RequireGuild", culture),
                _ => Translation.Format(Translation.Get<BotTranslations>("Command.Error.Check.Unknown", culture), x.ContextCheckAttribute.GetType().Name)
            }));

            await ctx.RespondAsync(message, true);
            return;
        }

        // If the interaction timed out, we ignore it.
        if (exception is TaskCanceledException && exception.InnerException is TimeoutException)
        {
            return;
        }

        var commandName = ctx.Command?.FullName ?? "NotFound";

        // If the exception is a discord exception, we get the json message.
        if (exception is DiscordException discordException && !string.IsNullOrEmpty(discordException.JsonMessage))
        {
            Log.Error(exception, "A discord error occurred when {UserName} ({UserId}) used the {CommandName} command.\n{JsonMessage}",
                interaction.User.Username, interaction.User.Id, commandName, discordException.JsonMessage);
        }
        else
        {
            Log.Error(exception, "An error occurred when {UserName} ({UserId}) used the {CommandName} command",
                interaction.User.Username, interaction.User.Id, commandName);
        }

        var embed = _embedBuilderService.CreateErrorEmbedBuilder(
            "An error occurred when executing a slash command",
            $"An error occurred when {Formatter.Sanitize(interaction.User.Username)} ({interaction.User.Mention}) used the {Formatter.Bold(commandName)} command.",
            exception);

#if DEBUG
        await ctx.Channel.SendMessageSafeAsync(embed);
#else
        await _cachedChannelsManager.SendErrorMessage(embed);
#endif

        var response = Translation.Get<BotTranslations>("Command.Error.UserResponse", culture);
        if (interaction.ResponseState == DiscordInteractionResponseState.Unacknowledged)
        {
            await ctx.RespondAsync(response, true);
        }
        else
        {
            await ctx.FollowupAsync(response, true);
        }
    }

    public Task HandleEventAsync(DiscordClient sender, CommandErroredEventArgs eventArgs)
    {
        //TODO: Move the upper method to this one when the extension supports the IEventHandler interface.
        return Task.CompletedTask;
    }
}
