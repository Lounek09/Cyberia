using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Other
{
#pragma warning disable CA1822 // Mark members as static
    public sealed class DiscordCommandModule : ApplicationCommandModule
    {
        [SlashCommand("discord", "Lien d'invitation du serveur Discord de support")]
        public async Task Command(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync(Bot.Instance.Config.DiscordInviteUrl, true);
        }
    }
}
