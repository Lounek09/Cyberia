using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;

namespace Cyberia.Salamandra.Commands.Data
{
#pragma warning disable CA1822 // Mark members as static
    public sealed class ReloadCommandModule : ApplicationCommandModule
    {
        [SlashCommand("reload", "[RequireOwner] Recharge les données de Salamandra")]
        [SlashRequireOwner]
        public async Task Command(InteractionContext ctx)
        {
            await ctx.DeferAsync();

            Bot.Instance.Api.Reload();

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Api reload !"));
        }
    }
}
