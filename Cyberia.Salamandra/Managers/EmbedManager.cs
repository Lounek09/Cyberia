using Cyberia.Api;
using Cyberia.Api.Managers;

using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Managers;

public static class EmbedManager
{
    public static DiscordEmbedBuilder BuildDofusEmbed(DofusEmbedCategory category, string authorText)
    {
        var embed = new DiscordEmbedBuilder()
            .WithColor(new DiscordColor(Bot.Config.EmbedColor))
            .WithFooter($"{Bot.Client.CurrentUser.Username} • {DateTime.Now.ToRolePlayString()} - {DateTime.Now:HH:mm}", $"{DofusApi.Config.CdnUrl}/images/mini-salamandra.png");

        var iconUrl = $"{DofusApi.Config.CdnUrl}/images/embed_categories";
        switch (category)
        {
            case DofusEmbedCategory.Bestiary:
                iconUrl = $"{iconUrl}/category_bestiary.png";
                break;
            case DofusEmbedCategory.Breeds:
                iconUrl = $"{iconUrl}/category_breeds.png";
                break;
            case DofusEmbedCategory.Houses:
                iconUrl = $"{iconUrl}/category_houses.png";
                break;
            case DofusEmbedCategory.Inventory:
                iconUrl = $"{iconUrl}/category_inventory.png";
                break;
            case DofusEmbedCategory.Jobs:
                iconUrl = $"{iconUrl}/category_jobs.png";
                break;
            case DofusEmbedCategory.Map:
                iconUrl = $"{iconUrl}/category_map.png";
                break;
            case DofusEmbedCategory.Quests:
                iconUrl = $"{iconUrl}/category_quests.png";
                break;
            case DofusEmbedCategory.Spells:
                iconUrl = $"{iconUrl}/category_spells.png";
                break;
            case DofusEmbedCategory.Tools:
                iconUrl = $"{iconUrl}/category_tools.png";
                break;
            default:
                return embed;
        }

        return embed.WithAuthor(authorText, iconUrl: iconUrl);
    }
}

public enum DofusEmbedCategory
{
    Bestiary,
    Breeds,
    Houses,
    Inventory,
    Jobs,
    Map,
    Quests,
    Spells,
    Tools
}
