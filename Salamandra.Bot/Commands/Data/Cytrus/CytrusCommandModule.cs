using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

using Salamandra.Bot.Managers;
using Salamandra.Cytrus.Models;

namespace Salamandra.Bot.Commands.Data
{
    [SlashCommandGroup("cytrus", "Cytrus")]
    public sealed class CytrusCommandModule : ApplicationCommandModule
    {
        [SlashCommand("check", "Lance un check de cytrus")]
        public async Task CheckCytrusCommand(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync($"Lancement du check de Cytrus...");
            await DiscordBot.Instance.Cytrus.Launch();
            await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder().WithContent($"Check de Cytrus terminé"));
        }

        [SlashCommand("show", "Affiche les informations du cytrus actuellement en ligne")]
        public async Task ShowCytrusCommand(InteractionContext ctx)
        {
            CytrusData cytrusData = DiscordBot.Instance.Cytrus.CytrusData;

            DiscordEmbedBuilder embed = EmbedManager.BuildDofusEmbed(DofusEmbedCategory.Tools, "Cytrus");

            embed.AddField("Name", cytrusData.Name.Capitalize(), true);
            embed.AddField("Version", cytrusData.Version.ToString(), true);
            embed.AddField(Constant.ZERO_WIDTH_SPACE, Constant.ZERO_WIDTH_SPACE, true);

            foreach (KeyValuePair<string, Game> game in cytrusData.Games.OrderBy(x => x.Value.Order))
            {
                List<string> content = new();

                foreach (KeyValuePair<string, Dictionary<string, string>> platform in game.Value.Platforms)
                {
                    content.Add(Formatter.Underline($"{platform.Key.Capitalize()} :"));

                    foreach (KeyValuePair<string, string> release in platform.Value)
                        content.Add($" - {release.Key.Capitalize()} : {Formatter.InlineCode(release.Value)}");
                }

                embed.AddField($"{game.Value.Name.Capitalize()} ({game.Value.GameId})", string.Join("\n", content));
            }

            await ctx.CreateResponseAsync(embed);
        }
    }
}
