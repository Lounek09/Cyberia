using Cyberia.Api;
using Cyberia.Api.Data;
using Cyberia.Api.Data.Items;
using Cyberia.Api.Managers;
using Cyberia.Salamandra.Commands.Dofus.Item;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

using System.Text;

namespace Cyberia.Salamandra.Commands.Dofus.Rune;

public sealed class RuneItemMessageBuilder : ICustomMessageBuilder
{
    public const string PacketHeader = "R";
    public const int PacketVersion = 1;
    public const int MaxQuantity = 9999;

    private readonly ItemData _itemData;
    private readonly int _quantity;

    public RuneItemMessageBuilder(ItemData itemData, int quantity = 1)
    {
        _itemData = itemData;
        _quantity = quantity;
    }

    public static RuneItemMessageBuilder? Create(int version, string[] parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 1 &&
            int.TryParse(parameters[0], out var itemId) &&
            int.TryParse(parameters[1], out var quantity))
        {
            var itemData = DofusApi.Datacenter.ItemsRepository.GetItemDataById(itemId);
            if (itemData is not null)
            {
                return new(itemData, quantity);
            }
        }

        return null;
    }

    public static string GetPacket(int itemId, int quantity = 1)
    {
        return InteractionManager.ComponentPacketBuilder(PacketHeader, PacketVersion, itemId, quantity);
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
        var embed = EmbedManager.CreateEmbedBuilder(EmbedCategory.Tools, BotTranslations.Embed_Rune_Author)
            .WithTitle($"{BotTranslations.Embed_RuneITem_Description}{_quantity.ToFormattedString()}x {Formatter.Sanitize(_itemData.Name)} ({_itemData.Id})")
            .WithThumbnail(await _itemData.GetImagePathAsync(CdnImageSize.Size128));

        StringBuilder descriptionBuilder = new();

        foreach (var runeBundle in RuneManager.GetRuneBundlesFromItem(_itemData, _quantity))
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
        yield return new(DiscordButtonStyle.Danger, GetPacket(_itemData.Id, _quantity - 1000), "-1000", (_quantity - 1000) < 1);
        yield return new(DiscordButtonStyle.Danger, GetPacket(_itemData.Id, _quantity - 100), "-100", (_quantity - 100) < 1);
        yield return new(DiscordButtonStyle.Danger, GetPacket(_itemData.Id, _quantity - 10), "-10", (_quantity - 10) < 1);
        yield return new(DiscordButtonStyle.Danger, GetPacket(_itemData.Id, _quantity - 1), "-1", (_quantity - 1) < 1);
    }

    private IEnumerable<DiscordButtonComponent> MoreButtonsBuilder()
    {
        yield return new(DiscordButtonStyle.Success, GetPacket(_itemData.Id, _quantity + 1000), "+1000", _quantity + 1000 > MaxQuantity);
        yield return new(DiscordButtonStyle.Success, GetPacket(_itemData.Id, _quantity + 100), "+100", _quantity + 100 > MaxQuantity);
        yield return new(DiscordButtonStyle.Success, GetPacket(_itemData.Id, _quantity + 10), "+10", _quantity + 10 > MaxQuantity);
        yield return new(DiscordButtonStyle.Success, GetPacket(_itemData.Id, _quantity + 1), "+1", _quantity + 1 > MaxQuantity);
    }

    private IEnumerable<DiscordButtonComponent> ButtonsBuilder()
    {
        yield return new DiscordButtonComponent(DiscordButtonStyle.Primary, GetPacket(_itemData.Id, _quantity), BotTranslations.Button_Resimulate);
        yield return ItemComponentsBuilder.ItemButtonBuilder(_itemData, _quantity);
    }
}
