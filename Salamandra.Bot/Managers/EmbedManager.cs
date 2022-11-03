using DSharpPlus.Entities;

namespace Salamandra.Bot.Managers
{
    public static class EmbedManager
    {
        public static DiscordEmbedBuilder BuildDofusEmbed(DofusEmbedCategory category, string authorText)
        {
            DiscordEmbedBuilder embed = new DiscordEmbedBuilder()
                                        .WithColor(new DiscordColor(DiscordBot.Instance.Config.EmbedColor))
                                        .WithFooter($"{DiscordBot.Instance.Client.CurrentUser.Username}  •  {/*DiscordBot.Instance.Api.Datacenter.TimeZones.GetDate()*/"pas de date hihi"} - {DateTime.Now:HH:mm}"/*, $"{DiscordBot.Instance.Config.CdnUrl}/images/mini_salamandra.png"*/);

            string iconUrl = $"{DiscordBot.Instance.Config.CdnUrl}/images/embed_categories";
            switch (category)
            {
                case DofusEmbedCategory.Bestiary:
                    iconUrl = $"{iconUrl}/category_bestiary.png";
                    break;
                case DofusEmbedCategory.Classes:
                    iconUrl = $"{iconUrl}/category_classes.png";
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

            return embed.WithAuthor(authorText/*, iconUrl: iconUrl*/);
        }
    }

    public enum DofusEmbedCategory
    {
        Bestiary,
        Classes,
        Houses,
        Inventory,
        Jobs,
        Map,
        Quests,
        Spells,
        Tools
    }
}
