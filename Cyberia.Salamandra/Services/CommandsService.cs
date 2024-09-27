using Cyberia.Salamandra.Extensions.DSharpPlus;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.ContextChecks;
using DSharpPlus.Commands.EventArgs;
using DSharpPlus.Commands.Exceptions;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;

namespace Cyberia.Salamandra.Services;

/// <summary>
/// Represents a service to handle commands events.
/// </summary>
public sealed class CommandsService
{
    private readonly CachedChannelsManager _cachedChannelsManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandsService"/> class.
    /// </summary>
    /// <param name="cachedChannelsManager">The manager to get the channels from.</param>
    public CommandsService(CachedChannelsManager cachedChannelsManager)
    {
        _cachedChannelsManager = cachedChannelsManager;
    }

    /// <summary>
    /// Handles the event when a command execution results in an error.
    /// </summary>
    /// <param name="_">Ignored.</param>
    /// <param name="args">The event arguments.</param>
    public async Task OnCommandErrored(CommandsExtension _, CommandErroredEventArgs args)
    {
        var ctx = args.Context.As<SlashCommandContext>();
        var exception = args.Exception;

        if (exception is ChecksFailedException checkFailedException)
        {
            var message = string.Join('\n',
                checkFailedException.Errors.Select(x => x.ContextCheckAttribute switch
                {
                    RequireApplicationOwnerAttribute => BotTranslations.Command_Error_Check_RequireApplicationOwner,
                    RequireGuildAttribute => BotTranslations.Command_Error_Check_RequireGuild,
                    _ => Translation.Format(BotTranslations.Command_Error_Check_Unknown, x.ContextCheckAttribute.GetType().Name)
                }));

            await ctx.RespondAsync(message, true);
            return;
        }

        var commandName = ctx.Command?.FullName ?? "NotFound";

        Log.Error(
            exception,
            "An error occurred when {UserName} ({UserId}) used the {CommandName} command",
            ctx.User.Username,
            ctx.User.Id,
            commandName);

        if (exception is DiscordException discordException)
        {
            Log.Error("With JsonMessage:\n{JsonMessage}", discordException.JsonMessage);
        }

        if (exception is TaskCanceledException && exception.InnerException is TimeoutException)
        {
            return;
        }

        var commandArgs = string.Join('\n',
            ctx.Arguments.Select(x => $"- {x.Key.Name} : {Formatter.InlineCode(x.Value is null ? "null" : x.Value.ToString() ?? string.Empty)}"));

        DiscordEmbedBuilder embed = new()
        {
            Title = "An error occurred when executing a slash command",
            Description = $"""
            An error occurred when {Formatter.Sanitize(ctx.User.Username)} ({ctx.User.Mention}) used the {Formatter.Bold(commandName)} command.
            {(commandArgs.Length > 0 ? $"\n{Formatter.Bold("Arguments :")}\n{commandArgs}\n" : string.Empty)}
            {Formatter.Bold($"{exception.GetType().Name} :")}
            {Formatter.BlockCode($"{exception.Message}\n{exception.StackTrace}".WithMaxLength(Constant.MaxEmbedDescriptionSize - 500))}
            """,
            Color = DiscordColor.Red
        };

#if DEBUG
        await ctx.Channel.SendMessageSafeAsync(embed);
#else
        await _messagesManager.SendErrorMessage(embed);
#endif

        if (ctx.Interaction.ResponseState == DiscordInteractionResponseState.Unacknowledged)
        {
            await ctx.RespondAsync(BotTranslations.Command_Error_UserResponse, true);
        }
        else
        {
            await ctx.FollowupAsync(BotTranslations.Command_Error_UserResponse, true);
        }
    }
}
