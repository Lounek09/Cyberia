using Cyberia.Api;
using Cyberia.Api.Data;
using Cyberia.Api.Data.Items;
using Cyberia.Api.Managers;
using Cyberia.Salamandra.Commands.Dofus.Item;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Formatters;
using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.Entities;

using Microsoft.Extensions.DependencyInjection;

using System.Globalization;
using System.Text;

namespace Cyberia.Salamandra.Commands.Dofus.Rune;

public sealed class RuneItemMessageBuilder : ICustomMessageBuilder
{
    public const string PacketHeader = "R";
    public const int PacketVersion = 1;
    public const int MaxQuantity = 9999;

    private readonly EmbedBuilderService _embedBuilderService;
    private readonly ItemData _itemData;
    private readonly int _quantity;
    private readonly CultureInfo? _culture;

    public RuneItemMessageBuilder(EmbedBuilderService embedBuilderService, ItemData itemData, int quantity, CultureInfo? culture)
    {
        _embedBuilderService = embedBuilderService;
        _itemData = itemData;
        _quantity = quantity;
        _culture = culture;
    }

    public static RuneItemMessageBuilder? Create(IServiceProvider provider, int version, CultureInfo? culture, params ReadOnlySpan<string> parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 1 &&
            int.TryParse(parameters[0], out var itemId) &&
            int.TryParse(parameters[1], out var quantity))
        {
            var itemData = DofusApi.Datacenter.ItemsRepository.GetItemDataById(itemId);
            if (itemData is not null)
            {
                var embedBuilderService = provider.GetRequiredService<EmbedBuilderService>();

                return new(embedBuilderService, itemData, quantity, culture);
            }
        }

        return null;
    }

    public static string GetPacket(int itemId, int quantity = 1)
    {
        return PacketFormatter.Action(PacketHeader, PacketVersion, itemId, quantity);
    }

    public async Task<T> BuildAsync<T>() where T : IDiscordMessageBuilder, new()
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
        var embed = _embedBuilderService.CreateEmbedBuilder(EmbedCategory.Tools, Translation.Get<BotTranslations>("Embed.Rune.Author", _culture), _culture)
            .WithTitle($"{Translation.Get<BotTranslations>("Embed.RuneItem.Description", _culture)} {_quantity.ToFormattedString(_culture)}x {Formatter.Sanitize(_itemData.Name.ToString(_culture))} ({_itemData.Id})")
            .WithThumbnail(await _itemData.GetImagePathAsync(CdnImageSize.Size128));

        StringBuilder descriptionBuilder = new();

        foreach (var runeBundle in RuneManager.GetRuneBundlesFromItem(_itemData, _quantity))
        {
            descriptionBuilder.Append(Formatter.Bold(runeBundle.BaAmount.ToFormattedString(_culture)));
            descriptionBuilder.Append(' ');
            descriptionBuilder.Append(Emojis.BaRune(runeBundle.RuneData, _culture));

            if (runeBundle.PaAmount > 0)
            {
                descriptionBuilder.Append(" - ");
                descriptionBuilder.Append(Formatter.Bold(runeBundle.PaAmount.ToFormattedString(_culture)));
                descriptionBuilder.Append(' ');
                descriptionBuilder.Append(Emojis.PaRune(runeBundle.RuneData, _culture));
            }

            if (runeBundle.RaAmount > 0)
            {
                descriptionBuilder.Append(" - ");
                descriptionBuilder.Append(Formatter.Bold(runeBundle.RaAmount.ToFormattedString(_culture)));
                descriptionBuilder.Append(' ');
                descriptionBuilder.Append(Emojis.RaRune(runeBundle.RuneData, _culture));
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
        yield return new DiscordButtonComponent(
            DiscordButtonStyle.Primary,
            GetPacket(_itemData.Id, _quantity),
            Translation.Get<BotTranslations>("Button.Resimulate", _culture));

        yield return ItemComponentsBuilder.ItemButtonBuilder(_itemData, _quantity, _culture);
    }
}
