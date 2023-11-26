using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects;
using Cyberia.Api.JsonConverters;
using Cyberia.Api.Managers;

using System.Collections.Frozen;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data
{
    public sealed class ItemUnicStringData : IDofusData<int>
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("v")]
        public string Value { get; init; }

        [JsonConstructor]
        internal ItemUnicStringData()
        {
            Value = string.Empty;
        }
    }

    public sealed class ItemSuperTypeData : IDofusData<int>
    {
        public const int SUPER_TYPE_QUEST = 14;

        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("v")]
        [JsonInclude]
        internal bool V { get; init; }

        [JsonIgnore]
        public ReadOnlyCollection<int> SlotsId { get; internal set; }

        [JsonConstructor]
        internal ItemSuperTypeData()
        {
            SlotsId = ReadOnlyCollection<int>.Empty;
        }
    }

    internal sealed class ItemSuperTypeSlotData : IDofusData<int>
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("v")]
        [JsonConverter(typeof(ReadOnlyCollectionConverter<int>))]
        public ReadOnlyCollection<int> SlotsId { get; init; }

        [JsonConstructor]
        internal ItemSuperTypeSlotData()
        {
            SlotsId = ReadOnlyCollection<int>.Empty;
        }
    }

    public sealed class ItemTypeData : IDofusData<int>
    {
        public const int TYPE_PET = 18;

        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("t")]
        public int ItemSuperTypeId { get; init; }

        [JsonPropertyName("z")]
        [JsonConverter(typeof(EffectAreaConverter))]
        public EffectArea EffectArea { get; init; }

        [JsonConstructor]
        internal ItemTypeData()
        {
            Name = string.Empty;
        }
    }

    public sealed class ItemWeaponData : IDofusData
    {
        public int CriticalBonus { get; init; }

        public int ActionPointCost { get; init; }

        public int MinRange { get; init; }

        public int MaxRange { get; init; }

        public int CriticalHitRate { get; init; }

        public int CriticalFailureRate { get; init; }

        public bool LineOnly { get; init; }

        public bool LineOfSight { get; init; }

        internal ItemWeaponData()
        {

        }
    }

    public sealed class ItemData : IDofusData<int>
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("nn")]
        public string NormalizedName { get; init; }

        [JsonPropertyName("t")]
        public int ItemTypeId { get; init; }

        [JsonPropertyName("d")]
        public string Description { get; init; }

        [JsonPropertyName("ep")]
        public int Episode { get; init; }

        [JsonPropertyName("g")]
        public int GfxId { get; init; }

        [JsonPropertyName("l")]
        public int Level { get; init; }

        [JsonPropertyName("wd")]
        public bool Wieldable { get; init; }

        [JsonPropertyName("fm")]
        public bool Enhanceable { get; init; }

        [JsonPropertyName("w")]
        public int Weight { get; init; }

        [JsonPropertyName("et")]
        public bool Ethereal { get; init; }

        [JsonPropertyName("an")]
        public int AnimationId { get; init; }

        [JsonPropertyName("tw")]
        public bool TwoHanded { get; init; }

        [JsonPropertyName("e")]
        [JsonConverter(typeof(ItemWeaponDataConverter))]
        public ItemWeaponData? WeaponData { get; init; }

        [JsonPropertyName("c")]
        [JsonConverter(typeof(CriteriaCollectionConverter))]
        public CriteriaCollection Criteria { get; init; }

        [JsonPropertyName("s")]
        public int ItemSetId { get; init; }

        [JsonPropertyName("u")]
        public bool Usable { get; init; }

        [JsonPropertyName("ut")]
        public bool Targetable { get; init; }

        [JsonPropertyName("m")]
        public bool Cursed { get; init; }

        [JsonPropertyName("ce")]
        public bool Ceremonial { get; init; }

        [JsonPropertyName("p")]
        public int Price { get; init; }

        [JsonConstructor]
        internal ItemData()
        {
            Name = string.Empty;
            NormalizedName = string.Empty;
            Description = string.Empty;
            Criteria = [];
        }

        public async Task<string> GetImagePath()
        {
            string url = $"{DofusApi.Config.CdnUrl}/images/items/{ItemTypeId}/{GfxId}.png";

            if (await DofusApi.HttpClient.ExistsAsync(url))
            {
                return url;
            }

            return $"{DofusApi.Config.CdnUrl}/images/items/unknown.png";
        }

        public ItemTypeData? GetItemTypeData()
        {
            return DofusApi.Datacenter.ItemsData.GetItemTypeDataById(ItemTypeId);
        }

        public ItemStatsData? GetItemStatsData()
        {
            return DofusApi.Datacenter.ItemsStatsData.GetItemStatDataById(Id);
        }

        public bool IsReallyEnhanceable()
        {
            ItemTypeData? itemTypeData = GetItemTypeData();
            if (itemTypeData is not null)
            {
                int[] enhanceableSuperTypes = [ 1, 2, 3, 4, 5, 10, 11 ];
                int[] nonEnhanceableWeaponTypes = [ 20, 21, 22, 102, 114 ];
                return enhanceableSuperTypes.Contains(itemTypeData.ItemSuperTypeId) && !nonEnhanceableWeaponTypes.Contains(itemTypeData.Id) && Enhanceable;
            }

            return false;
        }

        public ItemSetData? GetItemSetData()
        {
            return DofusApi.Datacenter.ItemSetsData.GetItemSetDataById(ItemSetId);
        }

        public bool IsWeapon()
        {
            return WeaponData is not null;
        }

        public CraftData? GetCraftData()
        {
            return DofusApi.Datacenter.CraftsData.GetCraftDataById(Id);
        }

        public bool Tradeable()
        {
            ItemTypeData? itemTypeData = GetItemTypeData();
            ItemStatsData? itemStatsData = GetItemStatsData();

            bool isQuestItem = itemTypeData is not null && itemTypeData.ItemSuperTypeId == ItemSuperTypeData.SUPER_TYPE_QUEST;
            bool isLinkedToAccount = itemStatsData is not null && itemStatsData.Effects.OfType<ExchangeableEffect>().Any(x => x.IsLinkedToAccount());
            bool isUnbreakable = itemStatsData is not null && itemStatsData.Effects.OfType<UnbreakableEffect>().Any();

            return !isQuestItem && !Cursed && !isLinkedToAccount && !isUnbreakable;
        }

        public int GetNpcRetailPrice()
        {
            return Price == 0 ? 0 : Math.Max(1, (int)Math.Round(Price / 10d));
        }
    }

    public sealed class ItemsData : IDofusData
    {
        private const string FILE_NAME = "items.json";

        [JsonPropertyName("I.us")]
        [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, ItemUnicStringData>))]
        public FrozenDictionary<int, ItemUnicStringData> ItemUnicStrings { get; init; }

        [JsonPropertyName("I.st")]
        [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, ItemSuperTypeData>))]
        public FrozenDictionary<int, ItemSuperTypeData> ItemSuperTypes { get; init; }

        [JsonPropertyName("I.ss")]
        [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, ItemSuperTypeSlotData>))]
        internal FrozenDictionary<int, ItemSuperTypeSlotData> ItemSuperTypeSlots { get; init; }

        [JsonPropertyName("I.t")]
        [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, ItemTypeData>))]
        public FrozenDictionary<int, ItemTypeData> ItemTypes { get; init; }

        [JsonPropertyName("I.u")]
        [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, ItemData>))]
        public FrozenDictionary<int, ItemData> Items { get; init; }

        [JsonConstructor]
        internal ItemsData()
        {
            ItemUnicStrings = FrozenDictionary<int, ItemUnicStringData>.Empty;
            ItemSuperTypes = FrozenDictionary<int, ItemSuperTypeData>.Empty;
            ItemSuperTypeSlots = FrozenDictionary<int, ItemSuperTypeSlotData>.Empty;
            ItemTypes = FrozenDictionary<int, ItemTypeData>.Empty;
            Items = FrozenDictionary<int, ItemData>.Empty;
        }

        internal static ItemsData Load()
        {
            ItemsData data = Datacenter.LoadDataFromFile<ItemsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));

            foreach (ItemSuperTypeSlotData itemSuperTypeSlotData in data.ItemSuperTypeSlots.Values)
            {
                ItemSuperTypeData? itemSuperTypeData = data.GetItemSuperTypeDataById(itemSuperTypeSlotData.Id);
                if (itemSuperTypeData is not null)
                {
                    itemSuperTypeData.SlotsId = itemSuperTypeSlotData.SlotsId;
                }
            }

            return data;
        }

        public ItemSuperTypeData? GetItemSuperTypeDataById(int id)
        {
            ItemSuperTypes.TryGetValue(id, out ItemSuperTypeData? itemSuperTypeData);
            return itemSuperTypeData;
        }

        internal ItemSuperTypeSlotData? GetItemSuperTypeSlotDataById(int id)
        {
            ItemSuperTypeSlots.TryGetValue(id, out ItemSuperTypeSlotData? itemSuperTypeSlotData);
            return itemSuperTypeSlotData;
        }

        public ItemTypeData? GetItemTypeDataById(int id)
        {
            ItemTypes.TryGetValue(id, out ItemTypeData? itemTypeData);
            return itemTypeData;
        }

        public string GetItemTypeNameById(int id)
        {
            ItemTypeData? itemTypeData = GetItemTypeDataById(id);

            return itemTypeData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : itemTypeData.Name;
        }

        public ItemData? GetItemDataById(int id)
        {
            Items.TryGetValue(id, out ItemData? itemData);
            return itemData;
        }

        public IEnumerable<ItemData> GetItemsData(string name)
        {
            string[] names = name.NormalizeCustom().Split(' ');
            return Items.Values.Where(x => names.All(x.NormalizedName.Contains));
        }

        public string GetItemNameById(int id)
        {
            ItemData? itemData = GetItemDataById(id);

            return itemData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : itemData.Name;
        }
    }
}
