using Cyberia.Salamandra.Commands.Admin;
using Cyberia.Salamandra.Commands.Data;
using Cyberia.Salamandra.Commands.Dofus;
using Cyberia.Salamandra.Commands.Other;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;
using DSharpPlus.SlashCommands.EventArgs;

namespace Cyberia.Salamandra.Managers;

public static class CommandManager
{
    public static void RegisterCommands(this SlashCommandsExtension extension)
    {
        extension.RegisterAdminCommands(Bot.Config.AdminGuildId);
        extension.RegisterDataCommands(Bot.Config.AdminGuildId);
        extension.RegisterDofusCommands();
        extension.RegisterOtherCommands();
    }

    public static async Task OnSlashCommandErrored(SlashCommandsExtension _, SlashCommandErrorEventArgs args)
    {
        switch (args.Exception)
        {
            case SlashExecutionChecksFailedException checkFailedException
            when checkFailedException.FailedChecks.Any(x => x is SlashRequireOwnerAttribute):
                await args.Context.CreateResponseAsync("Cette commande n'est utilisable que par le propriétaire du bot.", true);
                return;
            case SlashExecutionChecksFailedException checkFailedException
            when checkFailedException.FailedChecks.Any(x => x is SlashRequireGuildAttribute):
                await args.Context.CreateResponseAsync("Cette commande n'est utilisable que dans un serveur.", true);
                return;
            case SlashExecutionChecksFailedException checkFailedException:
                await args.Context.CreateResponseAsync("Un des contrôles pour cette commande a raté. Vérifie bien que tu as les droits avant de l'utiliser.", true);
                return;
            case InvalidOperationException when args.Exception.Message == "Slash commands failed to register properly on startup.":
            case NullReferenceException:
                Log.Error(args.Exception, "Slash commands failed to register on startup");

                var embed = new DiscordEmbedBuilder()
                {
                    Title = "Slash commands failed to register on startup",
                    Description = $"""
                        {Formatter.Bold(args.Exception.GetType().Name)} :
                        {Formatter.BlockCode($"{args.Exception.Message}\n{args.Exception.StackTrace}".WithMaxLength(3000))}
                        """,
                    Color = DiscordColor.Red
                };

#if DEBUG
                await args.Context.Channel.SendMessageAsync(embed);
#else
                await MessageManager.SendCommandErrorMessage(embed);
#endif
                break;
            default:
                Log.Error(args.Exception,
                    "An error occurred when {UserName} ({UserId}) used the {CommandName} command",
                    args.Context.User.Username,
                    args.Context.User.Id,
                    args.Context.CommandName);

                var commandArgs = string.Empty;
                if (args.Context.Interaction.Data.Options is not null)
                {
                    commandArgs = string.Join("\n", args.Context.Interaction.Data.Options
                        .Where(x => x.Value is not null)
                        .Select(x => $"- {x.Name} : {Formatter.InlineCode(x.Value.ToString()!)}")
                        .ToArray());
                }

                embed = new DiscordEmbedBuilder()
                {
                    Title = "An error occurred when executing a slash command",
                    Description = $"""
                        An error occurred when {Formatter.Sanitize(args.Context.User.Username)} ({args.Context.User.Mention}) used the {Formatter.Bold(args.Context.CommandName)} command.
                        {(commandArgs.Length > 0 ? $"\n{Formatter.Bold("Arguments :")}\n{commandArgs}\n" : string.Empty)}
                        {Formatter.Bold($"{args.Exception.GetType().Name} :")}
                        {Formatter.BlockCode($"{args.Exception.Message}\n{args.Exception.StackTrace}".WithMaxLength(3000))}
                        """,
                    Color = DiscordColor.Red
                };

#if DEBUG
                await args.Context.Channel.SendMessageAsync(embed);
#else
                await MessageManager.SendCommandErrorMessage(embed);
#endif
                break;
        }

        try
        {
            await args.Context.CreateResponseAsync("La commande a rencontré un problème d'exécution, un rapport de bug a été envoyé automatiquement au propriétaire du bot.", true);
        }
        catch
        {
            await args.Context.FollowUpAsync(new DiscordFollowupMessageBuilder()
                .WithContent("La commande a rencontré un problème d'exécution, un rapport de bug a été envoyé automatiquement au propriétaire du bot.")
                .AsEphemeral());
        }
    }
}
