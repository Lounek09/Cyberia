using Cyberia.Salamandra.Commands.Admin;
using Cyberia.Salamandra.Commands.Data;
using Cyberia.Salamandra.Commands.Dofus;
using Cyberia.Salamandra.Commands.Other;

using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.ContextChecks;
using DSharpPlus.Commands.EventArgs;
using DSharpPlus.Commands.Exceptions;
using DSharpPlus.Entities;

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
        if (args.Exception is ChecksFailedException checkFailedException)
        {
            var message = string.Join(
                '\n',
                checkFailedException.Errors.Select(x => x.ContextCheckAttribute switch
                {
                    RequireApplicationOwnerAttribute => "- Cette commande n'est utilisable que par le propriétaire du bot.",
                    RequireGuildAttribute => "- Cette commande n'est utilisable que dans un serveur.",
                    _ => $"- Le contrôle de type {x.ContextCheckAttribute.GetType().Name} pour cette commande a raté."
                }));

            await args.Context.RespondAsync(new DiscordInteractionResponseBuilder()
                .WithContent(message)
                .AsEphemeral());

            return;
        }

        Log.Error(
            args.Exception,
            "An error occurred when {UserName} ({UserId}) used the {CommandName} command",
            args.Context.User.Username,
            args.Context.User.Id,
            args.Context.Command.FullName);

        var commandArgs = string.Join(
            '\n',
            args.Context.Arguments.Select(x => $"- {x.Key.Name} : {Formatter.InlineCode(x.Value is null ? "null" : x.Value.ToString()!)}"));

        DiscordEmbedBuilder embed = new()
        {
            Title = "An error occurred when executing a slash command",
            Description =
            $"""
            An error occurred when {Formatter.Sanitize(args.Context.User.Username)} ({args.Context.User.Mention}) used the {Formatter.Bold(args.Context.Command.Name)} command.
            {(commandArgs.Length > 0 ? $"\n{Formatter.Bold("Arguments :")}\n{commandArgs}\n" : string.Empty)}
            {Formatter.Bold($"{args.Exception.GetType().Name} :")}
            {Formatter.BlockCode($"{args.Exception.Message}\n{args.Exception.StackTrace}".WithMaxLength(3000))}
            """,
            Color = DiscordColor.Red
        };

#if DEBUG
        await args.Context.Channel.SendMessageAsync(embed);
#else
        await MessageManager.SendErrorMessage(embed);
#endif

        try
        {
            await args.Context.RespondAsync(new DiscordInteractionResponseBuilder()
                .WithContent("La commande a rencontré un problème d'exécution, un rapport de bug a été envoyé automatiquement au propriétaire du bot.")
                .AsEphemeral());
        }
        catch
        {
            await args.Context.FollowupAsync(new DiscordFollowupMessageBuilder()
                .WithContent("La commande a rencontré un problème d'exécution, un rapport de bug a été envoyé automatiquement au propriétaire du bot.")
                .AsEphemeral());
        }
    }
}
