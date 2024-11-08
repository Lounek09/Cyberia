using Cyberia.Api;
using Cyberia.Api.Data;
using Cyberia.Api.Data.Crafts;
using Cyberia.Api.Data.Incarnations;
using Cyberia.Api.Data.Items;
using Cyberia.Api.Data.ItemSets;
using Cyberia.Api.Data.ItemStats;
using Cyberia.Api.Data.Pets;
using Cyberia.Api.Factories.Effects;
using Cyberia.Salamandra.Commands.Dofus.Craft;
using Cyberia.Salamandra.Commands.Dofus.Incarnation;
using Cyberia.Salamandra.Commands.Dofus.ItemSet;
using Cyberia.Salamandra.Commands.Dofus.Rune;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Extensions.DSharpPlus;
using Cyberia.Salamandra.Formatters;
using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.Entities;

using Microsoft.Extensions.DependencyInjection;

using System.Globalization;
using System.Text;

namespace Cyberia.Salamandra.Commands.Dofus.Item;

public sealed class ItemMessageBuilder : ICustomMessageBuilder
{
    public const string PacketHeader = "I";
    public const int PacketVersion = 2;

    private readonly EmbedBuilderService _embedBuilderService;
    private readonly ItemData _itemData;
    private readonly ItemTypeData? _itemTypeData;
    private readonly ItemSetData? _itemSetData;
    private readonly ItemStatsData? _itemStatsData;
    private readonly PetData? _petData;
    private readonly CraftData? _craftData;
    private readonly IncarnationData? _incarnationData;
    private readonly int _quantity;
    private readonly CultureInfo? _culture;

    public ItemMessageBuilder(EmbedBuilderService embedBuilderService, ItemData itemData, int quantity, CultureInfo? culture)
    {
        _embedBuilderService = embedBuilderService;
        _itemData = itemData;
        _itemTypeData = itemData.GetItemTypeData();
        _itemSetData = itemData.GetItemSetData();
        _itemStatsData = itemData.GetItemStatsData();
        _petData = itemData.GetPetData();
        _craftData = itemData.GetCraftData();
        _incarnationData = itemData.GetIncarnationData();
        _quantity = quantity;
        _culture = culture;
    }

    public static ItemMessageBuilder? Create(IServiceProvider provider, int version, CultureInfo? culture, string[] parameters)
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
            .AddEmbed(await EmbedBuilder());

        var buttons = ButtonsBuilder();
        if (buttons.Any())
        {
            message.AddComponents(buttons);
        }

        return (T)message;
    }

    private async Task<DiscordEmbedBuilder> EmbedBuilder()
    {
        var embed = _embedBuilderService.CreateEmbedBuilder(EmbedCategory.Inventory, Translation.Get<BotTranslations>("Embed.Item.Author", _culture))
            .WithTitle($"{Formatter.Sanitize(_itemData.Name.ToString(_culture))} ({_itemData.Id})")
            .WithThumbnail(await _itemData.GetImagePathAsync(CdnImageSize.Size128))
            .AddField(Translation.Get<BotTranslations>("Embed.Field.Level.Title", _culture), _itemData.Level.ToString(), true)
            .AddField(
                Translation.Get<BotTranslations>("Embed.Field.ItemType.Title", _culture),
                DofusApi.Datacenter.ItemsRepository.GetItemTypeNameById(_itemData.ItemTypeId, _culture),
                true);

        var description = _itemData.Description.ToString(_culture);
        if (!string.IsNullOrEmpty(description))
        {
            embed.WithDescription(Formatter.Italic(description));
        }

        if (_itemSetData is not null)
        {
            embed.AddField(Translation.Get<BotTranslations>("Embed.Field.ItemSet.Title", _culture), _itemSetData.Name.ToString(_culture), true);
        }

        var effects = _itemStatsData?.Effects ?? Enumerable.Empty<IEffect>();
        embed.AddEffectFields(Translation.Get<BotTranslations>("Embed.Field.Effects.Title", _culture), effects, true, _culture);

        if (_itemData.Criteria.Count > 0)
        {
            embed.AddCriteriaFields(_itemData.Criteria, _culture);
        }

        if (_itemData.WeaponData is not null)
        {
            embed.AddWeaponInfosField(_itemData.WeaponData, _itemData.TwoHanded, _itemTypeData, _culture);
        }

        if (_petData is not null)
        {
            embed.AddPetField(_petData, _culture);
        }

        if (_craftData is not null)
        {
            embed.AddCraftField(_craftData, 1, _culture);
        }

        StringBuilder miscellaneousBuilder = new();

        miscellaneousBuilder.Append(_itemData.Weight.ToFormattedString(_culture));
        miscellaneousBuilder.Append(' ');
        miscellaneousBuilder.Append(Translation.Get<BotTranslations>("Embed.Field.Miscellaneous.Content.Weight", _culture));

        if (_itemData.Tradeable())
        {
            miscellaneousBuilder.Append(", ");
            miscellaneousBuilder.Append(Translation.Format(
                Translation.Get<BotTranslations>("Embed.Field.Miscellaneous.Content.Price", _culture),
                _itemData.GetNpcRetailPrice().ToFormattedString(_culture),
                Emojis.Kamas));
        }

        if (_itemData.Ceremonial)
        {
            miscellaneousBuilder.Append(", ");
            miscellaneousBuilder.Append(Translation.Get<BotTranslations>("Embed.Field.Miscellaneous.Content.Ceremonial", _culture));
        }

        if (_itemData.IsReallyEnhanceable())
        {
            miscellaneousBuilder.Append(", ");
            miscellaneousBuilder.Append(Translation.Get<BotTranslations>("Embed.Field.Miscellaneous.Content.Enhanceable", _culture));
        }

        if (_itemData.Ethereal)
        {
            miscellaneousBuilder.Append(", ");
            miscellaneousBuilder.Append(Translation.Get<BotTranslations>("Embed.Field.Miscellaneous.Content.Ethereal", _culture));
        }

        if (_itemData.Usable)
        {
            miscellaneousBuilder.Append(", ");
            miscellaneousBuilder.Append(Translation.Get<BotTranslations>("Embed.Field.Miscellaneous.Content.Usable", _culture));
        }

        if (_itemData.Targetable)
        {
            miscellaneousBuilder.Append(", ");
            miscellaneousBuilder.Append(Translation.Get<BotTranslations>("Embed.Field.Miscellaneous.Content.Targetable", _culture));
        }

        if (_itemData.Cursed)
        {
            miscellaneousBuilder.Append(", ");
            miscellaneousBuilder.Append(Translation.Get<BotTranslations>("Embed.Field.Miscellaneous.Content.Cursed", _culture));
        }

        embed.AddField(Translation.Get<BotTranslations>("Embed.Field.Miscellaneous.Title", _culture), miscellaneousBuilder.ToString());

        return embed;
    }

    private IEnumerable<DiscordButtonComponent> ButtonsBuilder()
    {
        if (_itemSetData is not null)
        {
            yield return ItemSetComponentsBuilder.ItemSetButtonBuilder(_itemSetData, _culture);
        }

        if (_incarnationData is not null)
        {
            yield return IncarnationComponentsBuilder.IncarnationButtonBuilder(_incarnationData, _culture);
        }

        if (_craftData is not null)
        {
            yield return CraftComponentsBuilder.CraftButtonBuilder(_craftData, _quantity, _culture);
        }

        if (_itemStatsData is not null && _itemStatsData.Effects.Any(x => x is IRuneGeneratorEffect))
        {
            yield return RuneComponentsBuilder.RuneItemButtonBuilder(_itemData, _quantity, _culture);
        }
    }
}
