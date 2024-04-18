using Cyberia.Api;
using Cyberia.Api.Managers;
using Cyberia.Salamandra.Enums;

using DSharpPlus.Entities;

using System.Text;

namespace Cyberia.Salamandra.Managers;

public static class EmbedManager
{
    private static readonly string s_baseIconUrl = $"{DofusApi.Config.CdnUrl}/images/embed_categories";
    private static readonly string s_footerIconUrl = $"{DofusApi.Config.CdnUrl}/images/mini-salamandra.png";
    private static readonly DiscordColor s_embedColor = new(Bot.Config.EmbedColor);

    public static DiscordEmbedBuilder? HelpEmbed { get; private set; }

    public static DiscordEmbedBuilder CreateEmbedBuilder(EmbedCategory category, string authorText)
    {
        return new DiscordEmbedBuilder()
            .WithColor(s_embedColor)
            .WithFooter(
                $"{Bot.Client.CurrentUser.Username} • {DateTime.Now.ToRolePlayString()} - {DateTime.Now:HH:mm}",
                s_footerIconUrl)
            .WithAuthor(authorText, iconUrl: GetIconUrl(category));
    }

    private static string GetIconUrl(EmbedCategory category)
    {
        //TODO: change the name of the images with just the id of the enum in the new Salamandra.Cdn
        return $"{s_baseIconUrl}/category_{category.ToString().ToLower()}.png";
    }

    internal static async Task CacheHelpEmbed()
    {
        StringBuilder descriptionBuilder = new();

        var commands = await Bot.Client.GetGlobalApplicationCommandsAsync();

        foreach (var command in commands)
        {
            if (command.Name.Equals("help"))
            {
                continue;
            }

            if (command.Options is not null)
            {
                var subCommands = command.Options
                    .Where(x => x.Type is DiscordApplicationCommandOptionType.SubCommand);

                if (subCommands.Any())
                {
                    descriptionBuilder.Append("- ");
                    descriptionBuilder.Append(string.Join(" - ", subCommands.Select(x => command.GetSubcommandMention(x.Name))));
                    descriptionBuilder.Append(" : ");
                    descriptionBuilder.Append(command.Description);
                    descriptionBuilder.Append('\n');

                    continue;
                }
            }

            descriptionBuilder.Append("- ");
            descriptionBuilder.Append(command.Mention);
            descriptionBuilder.Append(" : ");
            descriptionBuilder.Append(command.Description);
            descriptionBuilder.Append('\n');
        }

        HelpEmbed = CreateEmbedBuilder(EmbedCategory.Tools, "Help")
            .WithDescription(descriptionBuilder.ToString());
    }
}
