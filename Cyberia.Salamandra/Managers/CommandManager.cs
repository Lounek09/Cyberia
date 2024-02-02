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

    public static async Task OnSlashCommandErrored(SlashCommandsExtension _, SlashCommandErrorEventArgs e)
    {
        switch (e.Exception)
        {
            case SlashExecutionChecksFailedException checkFailedException
            when checkFailedException.FailedChecks.OfType<SlashRequireOwnerAttribute>().Any():
                await e.Context.CreateResponseAsync("Cette commande n'est utilisable que par le propriétaire du bot.", true);
                return;
            case SlashExecutionChecksFailedException checkFailedException
            when checkFailedException.FailedChecks.OfType<SlashRequireGuildAttribute>().Any():
                await e.Context.CreateResponseAsync("Cette commande n'est utilisable que dans un serveur.", true);
                return;
            case SlashExecutionChecksFailedException checkFailedException:
                await e.Context.CreateResponseAsync("Un des contrôles pour cette commande a raté. Vérifie bien que tu as les droits avant de l'utiliser.", true);
                return;
            case InvalidOperationException when e.Exception.Message == "Slash commands failed to register properly on startup.":
            case NullReferenceException:
                Log.Error(e.Exception, "Slash commands failed to register on startup");

                var embed = new DiscordEmbedBuilder()
                {
                    Title = "Slash commands failed to register on startup",
                    Description = $"""
                        {Formatter.Bold(e.Exception.GetType().Name)} :
                        {Formatter.BlockCode($"{e.Exception.Message}\n{e.Exception.StackTrace}".WithMaxLength(3000))}
                        """,
                    Color = DiscordColor.Red
                };

#if DEBUG
                await e.Context.Channel.SendMessageAsync(embed);
#else
                await MessageManager.SendCommandErrorMessage(embed);
#endif
                break;
            default:
                Log.Error(e.Exception,
                    "An error occurred when {UserName} ({UserId}) used the {CommandName} command",
                    e.Context.User.Username,
                    e.Context.User.Id,
                    e.Context.CommandName);

                var args = string.Empty;
                if (e.Context.Interaction.Data.Options is not null)
                {
                    args = string.Join("\n", e.Context.Interaction.Data.Options
                        .Select(x => $"- {x.Name} : {Formatter.InlineCode(x.Value.ToString()!)}")
                        .ToArray());
                }

                embed = new DiscordEmbedBuilder()
                {
                    Title = "An error occurred when executing a slash command",
                    Description = $"""
                        An error occurred when {Formatter.Sanitize(e.Context.User.Username)} ({e.Context.User.Mention}) used the {Formatter.Bold(e.Context.CommandName)} command.
                        {(args.Length > 0 ? $"\n{Formatter.Bold("Arguments :")}\n{args}\n" : string.Empty)}
                        {Formatter.Bold($"{e.Exception.GetType().Name} :")}
                        {Formatter.BlockCode($"{e.Exception.Message}\n{e.Exception.StackTrace}".WithMaxLength(3000))}
                        """,
                    Color = DiscordColor.Red
                };

#if DEBUG
                await e.Context.Channel.SendMessageAsync(embed);
#else
                await MessageManager.SendCommandErrorMessage(embed);
#endif
                break;
        }

        try
        {
            await e.Context.CreateResponseAsync("La commande a rencontré un problème d'exécution, un rapport de bug a été envoyé automatiquement au propriétaire du bot.", true);
        }
        catch
        {
            await e.Context.FollowUpAsync(new DiscordFollowupMessageBuilder()
                .WithContent("La commande a rencontré un problème d'exécution, un rapport de bug a été envoyé automatiquement au propriétaire du bot.")
                .AsEphemeral());
        }
    }
}
