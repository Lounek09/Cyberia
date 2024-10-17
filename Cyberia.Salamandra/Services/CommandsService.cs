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
    private readonly CultureService _cultureService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandsService"/> class.
    /// </summary>
    /// <param name="cachedChannelsManager">The manager to get the channels from.</param>
    /// <param name="cultureService">The service to get the culture from.</param>
    public CommandsService(CachedChannelsManager cachedChannelsManager, CultureService cultureService)
    {
        _cachedChannelsManager = cachedChannelsManager;
        _cultureService = cultureService;
    }

    /// <summary>
    /// Handles the event when a command execution results in an error.
    /// </summary>
    /// <param name="_">Ignored.</param>
    /// <param name="args">The event arguments.</param>
    public async Task OnCommandErrored(CommandsExtension _, CommandErroredEventArgs args)
    {
        var ctx = args.Context.As<SlashCommandContext>();
        var culture = await _cultureService.GetCultureAsync(ctx.Interaction);
        var exception = args.Exception;
        var interaction = ctx.Interaction;
        var commandName = ctx.Command?.FullName ?? "NotFound";

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

        // If the exception is a discord exception, we get the json message.
        string exceptionBlock;
        if (exception is DiscordException discordException)
        {
            Log.Error(exception, "A discord error occurred when {UserName} ({UserId}) used the {CommandName} command.\n{JsonMessage}",
                interaction.User.Username, interaction.User.Id, commandName, discordException.JsonMessage);

            exceptionBlock = $"{exception.Message}\n{discordException.JsonMessage}\n{exception.StackTrace}";
        }
        else
        {
            Log.Error(exception, "An error occurred when {UserName} ({UserId}) used the {CommandName} command",
                interaction.User.Username, interaction.User.Id, commandName);

            exceptionBlock = $"{exception.Message}\n{exception.StackTrace}";
        }

        DiscordEmbedBuilder embed = new()
        {
            Title = "An error occurred when executing a slash command",
            Description = $"""
                An error occurred when {Formatter.Sanitize(interaction.User.Username)} ({interaction.User.Mention}) used the {Formatter.Bold(commandName)} command.
                {Formatter.Bold($"{exception.GetType().Name} :")}
                {Formatter.BlockCode(exceptionBlock.WithMaxLength(Constant.MaxEmbedDescriptionSize - 500))}
                """,
            Color = DiscordColor.Red
        };

#if DEBUG
        await ctx.Channel.SendMessageSafeAsync(embed);
#else
        await _cachedChannelsManager.SendErrorMessage(embed);
#endif

        var response = Translation.Get<BotTranslations>("Command.Error.Internal", culture);
        if (interaction.ResponseState == DiscordInteractionResponseState.Unacknowledged)
        {
            await ctx.RespondAsync(response, true);
        }
        else
        {
            await ctx.FollowupAsync(response, true);
        }
    }
}
