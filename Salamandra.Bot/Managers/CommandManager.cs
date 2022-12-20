using DSharpPlus;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;
using DSharpPlus.SlashCommands.EventArgs;

using Salamandra.Bot.Commands.Admin;
using Salamandra.Bot.Commands.Data;
using Salamandra.Bot.Commands.Other;

namespace Salamandra.Bot.Managers
{
    public static class CommandManager
    {
        public static void RegisterCommands()
        {
            AdminCommandsGroup.Register(650835844369743884);
            DataCommandsGroup.Register(650835844369743884);
            OtherCommandsGroup.Register();
        }

        public static async Task OnSlashCommandErrored(SlashCommandsExtension s, SlashCommandErrorEventArgs e)
        {
            if (e.Exception is SlashExecutionChecksFailedException slashExecutionChecksFailedException)
            {
                foreach (SlashCheckBaseAttribute check in slashExecutionChecksFailedException.FailedChecks)
                {
                    if (check is SlashRequireOwnerAttribute)
                        await e.Context.CreateResponseAsync("Cette commande est utilisable uniquement par le propriétaire du bot !", true);
                    else if (check is SlashRequireGuildAttribute)
                        await e.Context.CreateResponseAsync("Cette commande n'est utilisable que dans un serveur !", true);
                }
            }
            else
            {
                string errorMessage = Formatter.BlockCode((e.Exception.Message + (e.Exception.StackTrace is null ? "" : "\n" + e.Exception.StackTrace)).WithMaxLength(2000));
#if DEBUG
                //await e.Context.FollowUpAsync(new DiscordFollowupMessageBuilder().WithContent(errorMessage));
                await e.Context.CreateResponseAsync(errorMessage);
#else
                await e.Context.CreateResponseAsync("La commande a rencontré un problème d'exécution, un rapport de bug a été envoyé automatiquement au propriétaire du bot", true);

                string args = string.Join('\n', e.Context.Interaction.Data.Options.Select(x => x.Name + " : " + x.Value.ToString()));
                await MessageManager.SendCommandErrorMessage($"**{e.Context.CommandName}**{(string.IsNullOrEmpty(args) ? "" : "\n" + args)}\n{errorMessage}".WithMaxLength(2000));
#endif
            }
        }

        public static async Task OnSlashCommandExecuted(SlashCommandsExtension _, SlashCommandExecutedEventArgs _1)
        {
            //TODO: [BOT] Stats des commandes utilisés
            await Task.Delay(0);
        }
    }
}
