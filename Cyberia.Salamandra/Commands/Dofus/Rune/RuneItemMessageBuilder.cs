using Cyberia.Api;
using Cyberia.Api.Data.Items;
using Cyberia.Api.Managers;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

using System.Text;

namespace Cyberia.Salamandra.Commands.Dofus;

public sealed class RuneItemMessageBuilder : ICustomMessageBuilder
{
    public const string PACKET_HEADER = "R";
    public const int PACKET_VERSION = 1;
    public const int MAX_QTE = 9999;

    private readonly ItemData _itemData;
    private readonly int _qte;

    public RuneItemMessageBuilder(ItemData itemData, int qte = 1)
    {
        _itemData = itemData;
        _qte = qte;
    }

    public static RuneItemMessageBuilder? Create(int version, string[] parameters)
    {
        if (version == PACKET_VERSION &&
            parameters.Length > 1 &&
            int.TryParse(parameters[0], out var itemId) &&
            int.TryParse(parameters[1], out var qte))
        {
            var itemData = DofusApi.Datacenter.ItemsData.GetItemDataById(itemId);
            if (itemData is not null)
            {
                return new(itemData, qte);
            }
        }

        return null;
    }

    public static string GetPacket(int itemId, int qte = 1)
    {
        return InteractionManager.ComponentPacketBuilder(PACKET_HEADER, PACKET_VERSION, itemId, qte);
    }

    public async Task<T> GetMessageAsync<T>() where T : IDiscordMessageBuilder, new()
    {
        var message = new T()
            .AddEmbed(await EmbedBuilder())
            .AddComponents(LessButtonsBuilder())
            .AddComponents(MoreButtonsBuilder())
            .AddComponents(ButtonsBuilder());

        return (T)message;
    }

    private async Task<DiscordEmbedBuilder> EmbedBuilder()
    {
        var embed = EmbedManager.CreateEmbedBuilder(EmbedCategory.Tools, "Calculateur de runes")
            .WithTitle($"Brisage : {_qte.ToStringThousandSeparator()}x {Formatter.Sanitize(_itemData.Name)} ({_itemData.Id})")
            .WithThumbnail(await _itemData.GetImagePath());

        var description = new StringBuilder();

        foreach (var runeBundle in RuneManager.GetRuneBundlesFromItem(_itemData, _qte))
        {
            if (runeBundle.BaAmount == 0 && runeBundle.RemainingBaPercent == 0)
            {
                continue;
            }

            description.Append(Formatter.Bold(runeBundle.BaAmount.ToStringThousandSeparator()));
            description.Append(' ');
            description.Append(Emojis.BaRune(runeBundle.RuneData.Id));

            if (runeBundle.PaAmount > 0)
            {
                description.Append(" - ");
                description.Append(Formatter.Bold(runeBundle.PaAmount.ToStringThousandSeparator()));
                description.Append(' ');
                description.Append(Emojis.PaRune(runeBundle.RuneData.Id));
            }

            if (runeBundle.RaAmount > 0)
            {
                description.Append(" - ");
                description.Append(Formatter.Bold(runeBundle.RaAmount.ToStringThousandSeparator()));
                description.Append(' ');
                description.Append(Emojis.RaRune(runeBundle.RuneData.Id));
            }

            description.Append('\n');
        }

        return embed.WithDescription(description.ToString());
    }

    private List<DiscordButtonComponent> LessButtonsBuilder()
    {
        return new List<DiscordButtonComponent>(4)
        {
            new(ButtonStyle.Danger, GetPacket(_itemData.Id, _qte - 1000), "-1000", (_qte - 1000) < 1),
            new(ButtonStyle.Danger, GetPacket(_itemData.Id, _qte - 100), "-100", (_qte - 100) < 1),
            new(ButtonStyle.Danger, GetPacket(_itemData.Id, _qte - 10), "-10", (_qte - 10) < 1),
            new(ButtonStyle.Danger, GetPacket(_itemData.Id, _qte - 1), "-1", (_qte - 1) < 1)
        };
    }

    private List<DiscordButtonComponent> MoreButtonsBuilder()
    {
        return new List<DiscordButtonComponent>(4)
        {
            new(ButtonStyle.Success, GetPacket(_itemData.Id, _qte + 1000), "+1000", _qte + 1000 > MAX_QTE),
            new(ButtonStyle.Success, GetPacket(_itemData.Id, _qte + 100), "+100", _qte + 100 > MAX_QTE),
            new(ButtonStyle.Success, GetPacket(_itemData.Id, _qte + 10), "+10", _qte + 10 > MAX_QTE),
            new(ButtonStyle.Success, GetPacket(_itemData.Id, _qte + 1), "+1", _qte + 1 > MAX_QTE)
        };
    }

    private List<DiscordButtonComponent> ButtonsBuilder()
    {
        return
        [
            new DiscordButtonComponent(ButtonStyle.Primary, GetPacket(_itemData.Id, _qte), "Régénérer"),
            ItemComponentsBuilder.ItemButtonBuilder(_itemData, _qte)
        ];
    }
}
