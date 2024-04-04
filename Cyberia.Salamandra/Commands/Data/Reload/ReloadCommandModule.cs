using Cyberia.Api;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;

namespace Cyberia.Salamandra.Commands.Data;

public sealed class ReloadCommandModule : ApplicationCommandModule
{
    [SlashCommand("reload", "[Owner] Recharge les données de Salamandra")]
    [SlashRequireOwner]
    public async Task Command(InteractionContext ctx)
    {
        await ctx.DeferAsync();

        await DofusApi.ReloadAsync();

        await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Api reload !"));
    }
}
