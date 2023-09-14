using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Managers;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class CraftMessageBuilder : ICustomMessageBuilder
    {
        public const string PACKET_HEADER = "CR";
        public const int PACKET_VERSION = 1;

        private readonly Craft _craft;
        private readonly Item? _item;
        private readonly int _qte;
        private readonly bool _recursive;

        public CraftMessageBuilder(Craft craft, int qte = 1, bool recursive = false)
        {
            _craft = craft;
            _item = craft.GetItem();
            _qte = qte;
            _recursive = recursive;
        }

        public static CraftMessageBuilder? Create(int version, string[] parameters)
        {
            if (version == PACKET_VERSION &&
                parameters.Length > 2 &&
                int.TryParse(parameters[0], out int craftId) &&
                int.TryParse(parameters[1], out int qte) &&
                bool.TryParse(parameters[2], out bool recursive))
            {
                Craft? craft = Bot.Instance.Api.Datacenter.CraftsData.GetCraftById(craftId);
                if (craft is not null)
                    return new(craft, qte, recursive);
            }

            return null;
        }

        public static string GetPacket(int craftId, int qte = 1, bool recursive = false)
        {
            return InteractionManager.ComponentPacketBuilder(PACKET_HEADER, PACKET_VERSION, craftId, qte, recursive);
        }

        public async Task<T> GetMessageAsync<T>() where T : IDiscordMessageBuilder, new()
        {
            IDiscordMessageBuilder message = new T()
                .AddEmbed(await EmbedBuilder())
                .AddComponents(LessButtonsBuilder())
                .AddComponents(MoreButtonsBuilder());

            List<DiscordButtonComponent> buttons = ButtonsBuilder();
            if (buttons.Count > 0)
                message.AddComponents(buttons);

            return (T)message;
        }

        private async Task<DiscordEmbedBuilder> EmbedBuilder()
        {
            DiscordEmbedBuilder embed = EmbedManager.BuildDofusEmbed(DofusEmbedCategory.Jobs, "Calculateur de crafts")
                .WithDescription(string.Join('\n', _craft.GetCraftToString(_qte, _recursive, true)));

            if (_item is not null)
            {
                embed.WithTitle($"Craft : {_qte.ToStringThousandSeparator()}x {Formatter.Sanitize(_item.Name)} ({_craft.Id})")
                    .WithThumbnail(await _item.GetImagePath());
            }

            int weight = _recursive ? _craft.GetRecursiveWeight() : _craft.GetWeight();
            embed.AddField("Poids :", $"{Formatter.Bold(weight.ToStringThousandSeparator())} pod{(weight > 1 ? "s" : "")} par craft" + (_qte > 1 ? $", {Formatter.Bold((weight * _qte).ToStringThousandSeparator())} au total" : ""));

            string time = $"{Formatter.Bold((_recursive ? _craft.GetRecursiveTimeForMultipleCraft(1) : _craft.GetTimeForMultipleCraft(1)).ToString(@"mm\mss\sfff"))} par craft";
            if (_qte > 1)
            {
                TimeSpan totalTime = _recursive ? _craft.GetRecursiveTimeForMultipleCraft(_qte) : _craft.GetTimeForMultipleCraft(_qte);
                string format = (totalTime.TotalDays < 1 ? "" : @"%d\d") + (totalTime.TotalHours < 1 ? "" : @"hh\h") + @"mm\mss\sfff";
                time += $"\n{Formatter.Bold(totalTime.ToString(format))} au total";
            }
            embed.AddField("Temps de craft approximatif:", time);

            return embed;
        }

        private List<DiscordButtonComponent> LessButtonsBuilder()
        {
            return new List<DiscordButtonComponent>(3)
            {
                new(ButtonStyle.Danger, GetPacket(_craft.Id, _qte - 100, _recursive), "-100", (_qte - 100) < 1),
                new(ButtonStyle.Danger, GetPacket(_craft.Id, _qte - 10, _recursive), "-10", (_qte - 10) < 1),
                new(ButtonStyle.Danger, GetPacket(_craft.Id, _qte - 1, _recursive), "-1", (_qte - 1) < 1)
            };
        }

        private List<DiscordButtonComponent> MoreButtonsBuilder()
        {
            return new List<DiscordButtonComponent>(3)
            {
                new(ButtonStyle.Success, GetPacket(_craft.Id, _qte + 100, _recursive), "+100"),
                new(ButtonStyle.Success, GetPacket(_craft.Id, _qte + 10, _recursive), "+10"),
                new(ButtonStyle.Success, GetPacket(_craft.Id, _qte + 1, _recursive), "+1")
            };
        }

        private List<DiscordButtonComponent> ButtonsBuilder()
        {
            List<DiscordButtonComponent> buttons = new()
            {
                new(ButtonStyle.Primary, GetPacket(_craft.Id, _qte, !_recursive), $"{(_recursive ? "Masquer" : "Afficher")} les sous crafts", !_craft.HasSubCraft())
            };

            if (_item is not null)
                buttons.Add(ItemComponentsBuilder.ItemButtonBuilder(_item, _qte));

            return buttons;
        }
    }
}
