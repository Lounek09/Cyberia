using Cyberia.Api;
using Cyberia.Api.Data;
using Cyberia.Api.Data.Crafts;
using Cyberia.Api.Data.Incarnations;
using Cyberia.Api.Data.Items;
using Cyberia.Api.Data.ItemSets;
using Cyberia.Api.Data.ItemStats;
using Cyberia.Api.Data.Pets;
using Cyberia.Api.Factories.Effects.Templates;
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

    public ItemMessageBuilder(EmbedBuilderService embedBuilderService, ItemData itemData, int quantity = 1)
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
    }

    public static ItemMessageBuilder? Create(IServiceProvider provider, int version, string[] parameters)
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

                return new(embedBuilderService, itemData, quantity);
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
        var embed = _embedBuilderService.CreateEmbedBuilder(EmbedCategory.Inventory, BotTranslations.Embed_Item_Author)
            .WithTitle($"{Formatter.Sanitize(_itemData.Name)} ({_itemData.Id})")
            .WithDescription(string.IsNullOrEmpty(_itemData.Description) ? string.Empty : Formatter.Italic(_itemData.Description))
            .WithThumbnail(await _itemData.GetImagePathAsync(CdnImageSize.Size128))
            .AddField(BotTranslations.Embed_Field_Level_Title, _itemData.Level.ToString(), true)
            .AddField(BotTranslations.Embed_Field_ItemType_Title, DofusApi.Datacenter.ItemsRepository.GetItemTypeNameById(_itemData.ItemTypeId), true);

        if (_itemSetData is not null)
        {
            embed.AddField(BotTranslations.Embed_Field_ItemSet_Title, _itemSetData.Name, true);
        }

        if (_itemStatsData is not null)
        {
            embed.AddEffectFields(BotTranslations.Embed_Field_Effects_Title, _itemStatsData.Effects, true);
        }

        if (_itemData.Criteria.Count > 0)
        {
            embed.AddCriteriaFields(_itemData.Criteria);
        }

        if (_itemData.WeaponData is not null)
        {
            embed.AddWeaponInfosField(_itemData.WeaponData, _itemData.TwoHanded, _itemTypeData);
        }

        if (_petData is not null)
        {
            embed.AddPetField(_petData);
        }

        if (_craftData is not null)
        {
            embed.AddCraftField(_craftData, 1);
        }

        StringBuilder miscellaneousBuilder = new();

        miscellaneousBuilder.Append(_itemData.Weight.ToFormattedString());
        miscellaneousBuilder.Append(' ');
        miscellaneousBuilder.Append(BotTranslations.Embed_Field_Miscellaneous_Content_Weight);

        if (_itemData.Tradeable())
        {
            miscellaneousBuilder.Append(", ");
            miscellaneousBuilder.Append(Translation.Format(
                BotTranslations.Embed_Field_Miscellaneous_Content_Price,
                _itemData.GetNpcRetailPrice().ToFormattedString(),
                Emojis.Kamas));
        }

        if (_itemData.Ceremonial)
        {
            miscellaneousBuilder.Append(", ");
            miscellaneousBuilder.Append(BotTranslations.Embed_Field_Miscellaneous_Content_Ceremonial);
        }

        if (_itemData.IsReallyEnhanceable())
        {
            miscellaneousBuilder.Append(", ");
            miscellaneousBuilder.Append(BotTranslations.Embed_Field_Miscellaneous_Content_Enhanceable);
        }

        if (_itemData.Ethereal)
        {
            miscellaneousBuilder.Append(", ");
            miscellaneousBuilder.Append(BotTranslations.Embed_Field_Miscellaneous_Content_Ethereal);
        }

        if (_itemData.Usable)
        {
            miscellaneousBuilder.Append(", ");
            miscellaneousBuilder.Append(BotTranslations.Embed_Field_Miscellaneous_Content_Usable);
        }

        if (_itemData.Targetable)
        {
            miscellaneousBuilder.Append(", ");
            miscellaneousBuilder.Append(BotTranslations.Embed_Field_Miscellaneous_Content_Targetable);
        }

        if (_itemData.Cursed)
        {
            miscellaneousBuilder.Append(", ");
            miscellaneousBuilder.Append(BotTranslations.Embed_Field_Miscellaneous_Content_Cursed);
        }

        embed.AddField(BotTranslations.Embed_Field_Miscellaneous_Title, miscellaneousBuilder.ToString());

        return embed;
    }

    private IEnumerable<DiscordButtonComponent> ButtonsBuilder()
    {
        if (_itemSetData is not null)
        {
            yield return ItemSetComponentsBuilder.ItemSetButtonBuilder(_itemSetData);
        }

        if (_incarnationData is not null)
        {
            yield return IncarnationComponentsBuilder.IncarnationButtonBuilder(_incarnationData);
        }

        if (_craftData is not null)
        {
            yield return CraftComponentsBuilder.CraftButtonBuilder(_craftData, _quantity);
        }

        if (_itemStatsData is not null && _itemStatsData.Effects.Any(x => x is IRuneGeneratorEffect))
        {
            yield return RuneComponentsBuilder.RuneItemButtonBuilder(_itemData, _quantity);
        }
    }
}
