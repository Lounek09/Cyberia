using Cyberia.Api;
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
using Cyberia.Salamandra.DsharpPlus;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

using System.Text;

namespace Cyberia.Salamandra.Commands.Dofus.Item;

public sealed class ItemMessageBuilder : ICustomMessageBuilder
{
    public const string PacketHeader = "I";
    public const int PacketVersion = 2;

    private readonly ItemData _itemData;
    private readonly ItemTypeData? _itemTypeData;
    private readonly ItemSetData? _itemSetData;
    private readonly ItemStatsData? _itemStatsData;
    private readonly PetData? _petData;
    private readonly CraftData? _craftData;
    private readonly IncarnationData? _incarnationData;
    private readonly int _qte;

    public ItemMessageBuilder(ItemData itemData, int qte = 1)
    {
        _itemData = itemData;
        _itemTypeData = itemData.GetItemTypeData();
        _itemSetData = itemData.GetItemSetData();
        _itemStatsData = itemData.GetItemStatsData();
        _petData = _itemData.ItemTypeId == ItemTypeData.Pet ? DofusApi.Datacenter.PetsData.GetPetDataByItemId(_itemData.Id) : null;
        _craftData = itemData.GetCraftData();
        _incarnationData = _itemData.IsWeapon() ? DofusApi.Datacenter.IncarnationsData.GetIncarnationDataByItemId(_itemData.Id) : null;
        _qte = qte;
    }

    public static ItemMessageBuilder? Create(int version, string[] parameters)
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

    public static string GetPacket(int itemId, int craftQte = 1)
    {
        return InteractionManager.ComponentPacketBuilder(PacketHeader, PacketVersion, itemId, craftQte);
    }

    public async Task<T> GetMessageAsync<T>() where T : IDiscordMessageBuilder, new()
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
        var embed = EmbedManager.CreateEmbedBuilder(EmbedCategory.Inventory, "Items")
            .WithTitle($"{Formatter.Sanitize(_itemData.Name)} ({_itemData.Id})")
            .WithDescription(string.IsNullOrEmpty(_itemData.Description) ? string.Empty : Formatter.Italic(_itemData.Description))
            .WithThumbnail(await _itemData.GetImagePath())
            .AddField("Niveau :", _itemData.Level.ToString(), true)
            .AddField("Type :", DofusApi.Datacenter.ItemsData.GetItemTypeNameById(_itemData.ItemTypeId), true);

        if (_itemSetData is not null)
        {
            embed.AddField("Panoplie :", _itemSetData.Name, true);
        }

        if (_itemStatsData is not null)
        {
            embed.AddEffectFields("Effets :", _itemStatsData.Effects);
        }

        if (_itemData.Criteria.Count > 0)
        {
            embed.AddCriteriaFields("Conditions : ", _itemData.Criteria);
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
        miscellaneousBuilder.Append(" pod(s)");

        if (_itemData.Tradeable())
        {
            miscellaneousBuilder.Append(", se vend ");
            miscellaneousBuilder.Append(_itemData.GetNpcRetailPrice().ToFormattedString());
            miscellaneousBuilder.Append(Emojis.Kamas);
            miscellaneousBuilder.Append(" aux pnj");
        }

        if (_itemData.Ceremonial)
        {
            miscellaneousBuilder.Append(", objet d'apparat");
        }

        if (_itemData.IsReallyEnhanceable())
        {
            miscellaneousBuilder.Append(", forgemageable");
        }

        if (_itemData.Ethereal)
        {
            miscellaneousBuilder.Append(", item éthéré");
        }

        if (_itemData.Usable)
        {
            miscellaneousBuilder.Append(", est consommable");
        }

        if (_itemData.Targetable)
        {
            miscellaneousBuilder.Append(", est ciblable");
        }

        if (_itemData.Cursed)
        {
            miscellaneousBuilder.Append(", malédiction");
        }

        embed.AddField("Divers :", miscellaneousBuilder.ToString());

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
            yield return CraftComponentsBuilder.CraftButtonBuilder(_craftData, _qte);
        }

        if (_itemStatsData is not null && _itemStatsData.Effects.Any(x => x is IRuneGeneratorEffect))
        {
            yield return RuneComponentsBuilder.RuneItemButtonBuilder(_itemData, _qte);
        }
    }
}
