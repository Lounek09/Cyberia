using Cyberia.Api;
using Cyberia.Api.Data;
using Cyberia.Api.Data.Crafts;
using Cyberia.Api.Data.Items;
using Cyberia.Salamandra.Commands.Dofus.Item;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Extensions.DSharpPlus;
using Cyberia.Salamandra.Formatters;
using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.Entities;

using Microsoft.Extensions.DependencyInjection;

using System.Globalization;

namespace Cyberia.Salamandra.Commands.Dofus.Craft;

public sealed class CraftMessageBuilder : ICustomMessageBuilder
{
    public const string PacketHeader = "CR";
    public const int PacketVersion = 1;
    public const int MaxQuantity = 99999;

    private readonly EmbedBuilderService _embedBuilderService;
    private readonly CraftData _craftData;
    private readonly ItemData? _itemData;
    private readonly int _quantity;
    private readonly bool _recursive;
    private readonly CultureInfo? _culture;

    public CraftMessageBuilder(EmbedBuilderService embedBuilderService, CraftData craftData, int quantity, bool recursive, CultureInfo? culture)
    {
        _embedBuilderService = embedBuilderService;
        _craftData = craftData;
        _itemData = craftData.GetItemData();
        _quantity = quantity;
        _recursive = recursive;
        _culture = culture;
    }

    public static CraftMessageBuilder? Create(IServiceProvider provider, int version, CultureInfo? culture, params ReadOnlySpan<string> parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 2 &&
            int.TryParse(parameters[0], out var craftId) &&
            int.TryParse(parameters[1], out var quantity) &&
            bool.TryParse(parameters[2], out var recursive))
        {
            var craftData = DofusApi.Datacenter.CraftsRepository.GetCraftDataById(craftId);
            if (craftData is not null)
            {
                var embedBuilderService = provider.GetRequiredService<EmbedBuilderService>();

                return new(embedBuilderService, craftData, quantity, recursive, culture);
            }
        }

        return null;
    }

    public static string GetPacket(int craftId, int quantity = 1, bool recursive = false)
    {
        return PacketFormatter.Action(PacketHeader, PacketVersion, craftId, quantity, recursive);
    }

    public async Task<T> BuildAsync<T>() where T : IDiscordMessageBuilder, new()
    {
        var message = new T()
            .AddEmbed(await EmbedBuilder())
            .AddComponents(LessButtonsBuilder())
            .AddComponents(MoreButtonsBuilder());

        var buttons = ButtonsBuilder();
        if (buttons.Any())
        {
            message.AddComponents(buttons);
        }

        return (T)message;
    }

    private async Task<DiscordEmbedBuilder> EmbedBuilder()
    {
        var embed = _embedBuilderService.CreateEmbedBuilder(EmbedCategory.Jobs, Translation.Get<BotTranslations>("Embed.Craft.Author", _culture), _culture)
            .WithCraftDescription(_craftData, _quantity, _recursive, _culture);

        if (_itemData is not null)
        {
            embed.WithTitle($"{Translation.Get<BotTranslations>("Embed.Craft.Title", _culture)} {_quantity.ToFormattedString(_culture)}x {Formatter.Sanitize(_itemData.Name.ToString(_culture))} ({_craftData.Id})")
                .WithThumbnail(await _itemData.GetImagePathAsync(CdnImageSize.Size128));
        }

        var weight = _recursive ? _craftData.GetWeightWithSubCraft() : _craftData.GetWeight();
        var formattedWeight = Formatter.Bold(weight.ToFormattedString(_culture));

        var totalWeight = weight * _quantity;
        var formattedTotalWeight = _quantity > 1 ? Formatter.Bold(totalWeight.ToFormattedString(_culture)) : string.Empty;

        var time = _recursive ? _craftData.GetTimePerCraftWithSubCraft(1) : _craftData.GetTimePerCraft(1);
        var formattedTime = Formatter.Bold(time.ToString(@"mm\mss\sfff"));

        var formattedTotalTime = string.Empty;
        if (_quantity > 1)
        {
            var totalTime = _recursive ? _craftData.GetTimePerCraftWithSubCraft(_quantity) : _craftData.GetTimePerCraft(_quantity);
            var format = (totalTime.TotalDays < 1 ? string.Empty : @"%d\d") + (totalTime.TotalHours < 1 ? string.Empty : @"hh\h") + @"mm\mss\sfff";

            formattedTotalTime = Formatter.Bold(totalTime.ToString(format));
        }

        embed.AddField(Translation.Get<BotTranslations>("Embed.Field.Weight.Title", _culture),
            Translation.Format(Translation.Get<BotTranslations>("Embed.Field.Weight.Content", _culture), formattedWeight, formattedTotalWeight));

        embed.AddField(Translation.Get<BotTranslations>("Embed.Field.CraftTime.Title", _culture),
            Translation.Format(Translation.Get<BotTranslations>("Embed.Field.CraftTime.Content", _culture), formattedTime, formattedTotalTime));

        return embed;
    }

    private IEnumerable<DiscordButtonComponent> LessButtonsBuilder()
    {
        yield return new(DiscordButtonStyle.Danger, GetPacket(_craftData.Id, _quantity - 1000, _recursive), "-1000", (_quantity - 1000) < 1);
        yield return new(DiscordButtonStyle.Danger, GetPacket(_craftData.Id, _quantity - 100, _recursive), "-100", (_quantity - 100) < 1);
        yield return new(DiscordButtonStyle.Danger, GetPacket(_craftData.Id, _quantity - 10, _recursive), "-10", (_quantity - 10) < 1);
        yield return new(DiscordButtonStyle.Danger, GetPacket(_craftData.Id, _quantity - 1, _recursive), "-1", (_quantity - 1) < 1);
    }

    private IEnumerable<DiscordButtonComponent> MoreButtonsBuilder()
    {
        yield return new(DiscordButtonStyle.Success, GetPacket(_craftData.Id, _quantity + 1000, _recursive), "+1000", _quantity + 1000 > MaxQuantity);
        yield return new(DiscordButtonStyle.Success, GetPacket(_craftData.Id, _quantity + 100, _recursive), "+100", _quantity + 100 > MaxQuantity);
        yield return new(DiscordButtonStyle.Success, GetPacket(_craftData.Id, _quantity + 10, _recursive), "+10", _quantity + 10 > MaxQuantity);
        yield return new(DiscordButtonStyle.Success, GetPacket(_craftData.Id, _quantity + 1, _recursive), "+1", _quantity + 1 > MaxQuantity);
    }

    private IEnumerable<DiscordButtonComponent> ButtonsBuilder()
    {
        var subCraftButtonlabel = _recursive
            ? Translation.Get<BotTranslations>("Button.SubCraft.Hide", _culture)
            : Translation.Get<BotTranslations>("Button.SubCraft.Display", _culture);

        yield return new DiscordButtonComponent(
            DiscordButtonStyle.Primary,
            GetPacket(_craftData.Id, _quantity, !_recursive),
            subCraftButtonlabel,
            !_craftData.HasSubCraft());

        if (_itemData is not null)
        {
            yield return ItemComponentsBuilder.ItemButtonBuilder(_itemData, _quantity, _culture);
        }
    }
}
