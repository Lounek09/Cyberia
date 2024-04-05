using Cyberia.Api;
using Cyberia.Api.Data.Items;
using Cyberia.Api.Managers;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

using System.Text;

namespace Cyberia.Salamandra.Commands.Dofus;

public sealed class RuneItemMessageBuilder : ICustomMessageBuilder
{
    public const string PacketHeader = "R";
    public const int PacketVersion = 1;
    public const int MaxQte = 9999;

    private readonly ItemData _itemData;
    private readonly int _qte;

    public RuneItemMessageBuilder(ItemData itemData, int qte = 1)
    {
        _itemData = itemData;
        _qte = qte;
    }

    public static RuneItemMessageBuilder? Create(int version, string[] parameters)
    {
        if (version == PacketVersion &&
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
        return InteractionManager.ComponentPacketBuilder(PacketHeader, PacketVersion, itemId, qte);
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
            .WithTitle($"Brisage : {_qte.ToFormattedString()}x {Formatter.Sanitize(_itemData.Name)} ({_itemData.Id})")
            .WithThumbnail(await _itemData.GetImagePath());

        StringBuilder descriptionBuilder = new();

        foreach (var runeBundle in RuneManager.GetRuneBundlesFromItem(_itemData, _qte))
        {
            descriptionBuilder.Append(Formatter.Bold(runeBundle.BaAmount.ToFormattedString()));
            descriptionBuilder.Append(' ');
            descriptionBuilder.Append(Emojis.BaRune(runeBundle.RuneData.Id));

            if (runeBundle.PaAmount > 0)
            {
                descriptionBuilder.Append(" - ");
                descriptionBuilder.Append(Formatter.Bold(runeBundle.PaAmount.ToFormattedString()));
                descriptionBuilder.Append(' ');
                descriptionBuilder.Append(Emojis.PaRune(runeBundle.RuneData.Id));
            }

            if (runeBundle.RaAmount > 0)
            {
                descriptionBuilder.Append(" - ");
                descriptionBuilder.Append(Formatter.Bold(runeBundle.RaAmount.ToFormattedString()));
                descriptionBuilder.Append(' ');
                descriptionBuilder.Append(Emojis.RaRune(runeBundle.RuneData.Id));
            }

            descriptionBuilder.Append('\n');
        }

        return embed.WithDescription(descriptionBuilder.ToString());
    }

    private IEnumerable<DiscordButtonComponent> LessButtonsBuilder()
    {
        yield return new(ButtonStyle.Danger, GetPacket(_itemData.Id, _qte - 1000), "-1000", (_qte - 1000) < 1);
        yield return new(ButtonStyle.Danger, GetPacket(_itemData.Id, _qte - 100), "-100", (_qte - 100) < 1);
        yield return new(ButtonStyle.Danger, GetPacket(_itemData.Id, _qte - 10), "-10", (_qte - 10) < 1);
        yield return new(ButtonStyle.Danger, GetPacket(_itemData.Id, _qte - 1), "-1", (_qte - 1) < 1);
    }

    private IEnumerable<DiscordButtonComponent> MoreButtonsBuilder()
    {
        yield return new(ButtonStyle.Success, GetPacket(_itemData.Id, _qte + 1000), "+1000", _qte + 1000 > MaxQte);
        yield return new(ButtonStyle.Success, GetPacket(_itemData.Id, _qte + 100), "+100", _qte + 100 > MaxQte);
        yield return new(ButtonStyle.Success, GetPacket(_itemData.Id, _qte + 10), "+10", _qte + 10 > MaxQte);
        yield return new(ButtonStyle.Success, GetPacket(_itemData.Id, _qte + 1), "+1", _qte + 1 > MaxQte);
    }

    private IEnumerable<DiscordButtonComponent> ButtonsBuilder()
    {
        yield return new DiscordButtonComponent(ButtonStyle.Primary, GetPacket(_itemData.Id, _qte), "Régénérer");
        yield return ItemComponentsBuilder.ItemButtonBuilder(_itemData, _qte);
    }
}
