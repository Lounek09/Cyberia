using Cyberia.Salamandra.Extensions.DSharpPlus;
using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.ContextChecks;
using DSharpPlus.Commands.EventArgs;
using DSharpPlus.Commands.Exceptions;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;

using System.Reflection;

namespace Cyberia.Salamandra.Managers;

public static class CommandManager
{
    public static void RegisterCommands(this CommandsExtension extension, params ulong[] guildIds)
    {
        Dictionary<string, bool> commandGroups = new()
        {
            { "Cyberia.Salamandra.Commands.Admin", true },
            { "Cyberia.Salamandra.Commands.Data", true },
            { "Cyberia.Salamandra.Commands.Dofus", false },
            { "Cyberia.Salamandra.Commands.Other", false }
        };

        var types = Assembly.GetExecutingAssembly().GetTypes()
            .Where(x => !string.IsNullOrEmpty(x.Namespace) && x.Name.EndsWith("CommandModule"));

        foreach (var (startNamespace, requiresGuildId) in commandGroups)
        {
            var commandModules = types.Where(x => x.Namespace!.StartsWith(startNamespace));

            if (requiresGuildId)
            {
                extension.AddCommands(commandModules, guildIds);
            }
            else
            {
                extension.AddCommands(commandModules);
            }
        }
    }

    public static async Task OnCommandErrored(CommandsExtension _, CommandErroredEventArgs args)
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
            {Formatter.Bold($"{args.Exception.GetType().Name} :")}
            {Formatter.BlockCode($"{args.Exception.Message}\n{args.Exception.StackTrace}".WithMaxLength(Constant.MaxEmbedDescriptionSize - 500))}
            """,
            Color = DiscordColor.Red
        };

#if DEBUG
        await ctx.Channel.SendMessageSafeAsync(embed);
#else
        await MessageManager.SendErrorMessage(embed);
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
