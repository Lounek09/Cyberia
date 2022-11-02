using DSharpPlus.SlashCommands;

namespace Salamandra.Bot.Commands.Other
{
    public sealed class InvitationCommandModule : ApplicationCommandModule
    {
        [SlashCommand("invitation", "Lien d'invitation du Bot")]
        public async Task Command(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync($"{DiscordBot.Instance.Config.InviteUrl}", true);
        }
    }
}
