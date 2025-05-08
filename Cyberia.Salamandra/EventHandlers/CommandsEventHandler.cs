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
    private readonly ICachedChannelsService _cachedChannelsService;
    private readonly ICultureService _cultureService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandsEventHandler"/> class.
    /// </summary>
    /// <param name="cachedChannelsService">The service to get the channels from.</param>
    /// <param name="cultureService">The service to get the culture from.</param>
    public CommandsEventHandler(ICachedChannelsService cachedChannelsService, ICultureService cultureService)
    {
        _cachedChannelsService = cachedChannelsService;
        _cultureService = cultureService;
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
        string exceptionBlock;
        if (exception is DiscordException discordException && !string.IsNullOrEmpty(discordException.JsonMessage))
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
        await _cachedChannelsService.SendErrorMessage(embed);
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
