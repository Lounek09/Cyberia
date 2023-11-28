using Cyberia.Api;
using Cyberia.Api.Managers;

using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Managers;

public static class EmbedManager
{
    public static DiscordEmbedBuilder CreateEmbedBuilder(EmbedCategory category, string authorText)
    {
        var embed = new DiscordEmbedBuilder()
            .WithColor(new DiscordColor(Bot.Config.EmbedColor))
            .WithFooter($"{Bot.Client.CurrentUser.Username} • {DateTime.Now.ToRolePlayString()} - {DateTime.Now:HH:mm}", $"{DofusApi.Config.CdnUrl}/images/mini-salamandra.png");

        var iconUrl = $"{DofusApi.Config.CdnUrl}/images/embed_categories";
        switch (category)
        {
            case EmbedCategory.Bestiary:
                iconUrl = $"{iconUrl}/category_bestiary.png";
                break;
            case EmbedCategory.Breeds:
                iconUrl = $"{iconUrl}/category_breeds.png";
                break;
            case EmbedCategory.Houses:
                iconUrl = $"{iconUrl}/category_houses.png";
                break;
            case EmbedCategory.Inventory:
                iconUrl = $"{iconUrl}/category_inventory.png";
                break;
            case EmbedCategory.Jobs:
                iconUrl = $"{iconUrl}/category_jobs.png";
                break;
            case EmbedCategory.Map:
                iconUrl = $"{iconUrl}/category_map.png";
                break;
            case EmbedCategory.Quests:
                iconUrl = $"{iconUrl}/category_quests.png";
                break;
            case EmbedCategory.Spells:
                iconUrl = $"{iconUrl}/category_spells.png";
                break;
            case EmbedCategory.Tools:
                iconUrl = $"{iconUrl}/category_tools.png";
                break;
            default:
                return embed;
        }

        return embed.WithAuthor(authorText, iconUrl: iconUrl);
    }
}

public enum EmbedCategory
{
    None,
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
