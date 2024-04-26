using Cyberia.Salamandra.Commands.Admin;
using Cyberia.Salamandra.Commands.Data;
using Cyberia.Salamandra.Commands.Dofus;
using Cyberia.Salamandra.Commands.Other;

using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.ContextChecks;
using DSharpPlus.Commands.EventArgs;
using DSharpPlus.Commands.Exceptions;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;

namespace Cyberia.Salamandra.Managers;

public static class CommandManager
{
    public static void RegisterCommands(this CommandsExtension extension)
    {
        extension.RegisterAdminCommands(Bot.Config.AdminGuildId);
        extension.RegisterDataCommands(Bot.Config.AdminGuildId);
        extension.RegisterDofusCommands();
        extension.RegisterOtherCommands();
    }

    public static async Task OnCommandErrored(CommandsExtension _, CommandErroredEventArgs args)
    {
        var ctx = args.Context.As<SlashCommandContext>();

        if (args.Exception is ChecksFailedException checkFailedException)
        {
            var message = string.Join(
                '\n',
                checkFailedException.Errors.Select(x => x.ContextCheckAttribute switch
                {
                    RequireApplicationOwnerAttribute => "Cette commande n'est utilisable que par le propriétaire du bot.",
                    RequireGuildAttribute => "Cette commande n'est utilisable que dans un serveur.",
                    _ => $"Le contrôle de type {x.ContextCheckAttribute.GetType().Name} pour cette commande a raté."
                }));

            await ctx.RespondAsync(message, true);
            return;
        }

        var commandName = ctx.Command?.FullName ?? "undefined";

        Log.Error(
            args.Exception,
            "An error occurred when {UserName} ({UserId}) used the {CommandName} command",
            ctx.User.Username,
            ctx.User.Id,
            commandName);

        if (args.Exception is DiscordException discordException)
        {
            Log.Error("With JsonMessage:\n{JsonMessage}", discordException.JsonMessage);
        }

        var commandArgs = string.Join('\n',
            ctx.Arguments.Select(x => $"- {x.Key.Name} : {Formatter.InlineCode(x.Value is null ? "null" : x.Value.ToString() ?? string.Empty)}"));

        DiscordEmbedBuilder embed = new()
        {
            Title = "An error occurred when executing a slash command",
            Description =
            $"""
            An error occurred when {Formatter.Sanitize(ctx.User.Username)} ({ctx.User.Mention}) used the {Formatter.Bold(commandName)} command.
            {(commandArgs.Length > 0 ? $"\n{Formatter.Bold("Arguments :")}\n{commandArgs}\n" : string.Empty)}
            {Formatter.Bold($"{args.Exception.GetType().Name} :")}
            {Formatter.BlockCode($"{args.Exception.Message}\n{args.Exception.StackTrace}".WithMaxLength(3000))}
            """,
            Color = DiscordColor.Red
        };

#if DEBUG
        await ctx.Channel.SendMessageAsync(embed);
#else
        await MessageManager.SendErrorMessage(embed);
#endif

        try
        {
            await ctx.RespondAsync("La commande a rencontré un problème d'exécution, un rapport de bug a été envoyé automatiquement au propriétaire du bot.", true);
        }
        catch
        {
            await ctx.FollowupAsync("La commande a rencontré un problème d'exécution, un rapport de bug a été envoyé automatiquement au propriétaire du bot.", true);
        }
    }
}
