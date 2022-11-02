using DSharpPlus.SlashCommands;

namespace Salamandra.Bot.Commands.Other
{
    public sealed class DiscordCommandModule : ApplicationCommandModule
    {
        [SlashCommand("discord", "Lien d'invitation du serveur Discord de support")]
        public async Task Command(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync(DiscordBot.Instance.Config.DiscordInviteUrl, true);
        }
    }
}
