using Cyberia.Api.Factories.Effects;
using Cyberia.Api.Managers;
using Cyberia.Api.Parser.JsonConverter;

using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class ItemUnicString
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("v")]
        public string Value { get; init; }

        public ItemUnicString()
        {
            Value = string.Empty;
        }
    }

    public sealed class ItemSuperType
    {
        public const int SUPER_TYPE_QUEST = 14;

        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("v")]
        public bool V { get; init; }

        public ItemSuperType()
        {

        }
    }

    public sealed class ItemSuperTypeSlot
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("v")]
        public List<int> SlotsId { get; init; }

        public ItemSuperTypeSlot()
        {
            SlotsId = new();
        }
    }

    public sealed class ItemType
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("t")]
        public int ItemSuperTypeId { get; init; }

        [JsonPropertyName("z")]
        [JsonConverter(typeof(EffectAreaJsonConverter))]
        public Area Area { get; init; }

        public ItemType()
        {
            Name = string.Empty;
        }
    }

    public sealed class ItemWeaponInfos
    {
        public int CriticalBonus { get; init; }

        public int ActionPointCost { get; init; }

        public int MinRange { get; init; }

        public int MaxRange { get; init; }

        public int CriticalHitRate { get; init; }

        public int CriticalFailureRate { get; init; }

        public bool LineOnly { get; init; }

        public bool LineOfSight { get; init; }

        public ItemWeaponInfos()
        {

        }
    }

    public sealed class Item
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
        [JsonConverter(typeof(ItemWeaponInfosJsonConverter))]
        public ItemWeaponInfos? WeaponInfos { get; init; }

        [JsonPropertyName("c")]
        public string Criterion { get; init; }

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

        public Item()
        {
            Name = string.Empty;
            NormalizedName = string.Empty;
            Description = string.Empty;
            Criterion = string.Empty;
        }

        public async Task<string> GetImagePath()
        {
            string url = $"{DofusApi.Instance.Config.CdnUrl}/images/items/{ItemTypeId}/{GfxId}.png";

            if (await DofusApi.Instance.HttpClient.CheckIfPageExistsAsync(url))
                return url;

            return $"{DofusApi.Instance.Config.CdnUrl}/images/items/unknown.png";
        }

        public ItemType? GetItemType()
        {
            return DofusApi.Instance.Datacenter.ItemsData.GetItemTypeById(ItemTypeId);
        }

        public ItemStats? GetItemStat()
        {
            return DofusApi.Instance.Datacenter.ItemStatsData.GetItemStatById(Id);
        }

        public bool IsReallyEnhanceable()
        {
            ItemType? itemType = GetItemType();
            if (itemType is not null)
            {
                int[] enhanceableSuperTypes = { 1, 2, 3, 4, 5, 10, 11 };
                int[] nonEnhanceableWeaponTypes = { 20, 21, 22, 102, 114 };
                return enhanceableSuperTypes.Contains(itemType.ItemSuperTypeId) && !nonEnhanceableWeaponTypes.Contains(itemType.Id) && Enhanceable;
            }

            return false;
        }

        public ItemSet? GetItemSet()
        {
            return DofusApi.Instance.Datacenter.ItemSetsData.GetItemSetById(ItemSetId);
        }

        public bool IsWeapon()
        {
            return WeaponInfos is not null;
        }

        public Craft? GetCraft()
        {
            return DofusApi.Instance.Datacenter.CraftsData.GetCraftById(Id);
        }

        public bool IsExchangeable()
        {
            ItemType? itemType = GetItemType();
            ItemStats? itemStats = GetItemStat();

            return itemType is not null && itemType.ItemSuperTypeId != ItemSuperType.SUPER_TYPE_QUEST &&
                !Cursed &&
                (itemStats is null || !itemStats.Effects.OfType<ExchangeableUntilDateTimeEffect>().Any(x => x.IsLinkedToAccount()));
        }

        public int GetNpcRetailPrice()
        {
            return Price == 0 ? 0 : Math.Max(1, (int)Math.Round(Price / 10d));
        }
    }

    public sealed class ItemsData
    {
        private const string FILE_NAME = "items.json";

        [JsonPropertyName("I.us")]
        public List<ItemUnicString> ItemUnicStrings { get; init; }

        [JsonPropertyName("I.st")]
        public List<ItemSuperType> ItemSuperTypes { get; init; }

        [JsonPropertyName("I.ss")]
        public List<ItemSuperTypeSlot> ItemSuperTypeSlots { get; init; }

        [JsonPropertyName("I.t")]
        public List<ItemType> ItemTypes { get; init; }

        [JsonPropertyName("I.u")]
        public List<Item> Items { get; init; }

        public ItemsData()
        {
            ItemUnicStrings = new();
            ItemSuperTypes = new();
            ItemSuperTypeSlots = new();
            ItemTypes = new();
            Items = new();
        }

        internal static ItemsData Build()
        {
            return Json.LoadFromFile<ItemsData>($"{DofusApi.OUTPUT_PATH}/{FILE_NAME}");
        }

        public ItemType? GetItemTypeById(int id)
        {
            return ItemTypes.Find(x => x.Id == id);
        }

        public string GetItemTypeNameById(int id)
        {
            ItemType? itemType = GetItemTypeById(id);

            return itemType is null ? $"Inconnu ({id})" : itemType.Name;
        }

        public Item? GetItemById(int id)
        {
            return Items.Find(x => x.Id == id);
        }

        public Item? GetItemByName(string name)
        {
            return Items.Find(x => x.Name.RemoveDiacritics().Equals(name.RemoveDiacritics()));
        }

        public List<Item> GetItemsByName(string name)
        {
            string[] names = name.RemoveDiacritics().Split(' ');
            return Items.FindAll(x => names.All(x.Name.RemoveDiacritics().Contains));
        }

        public string GetItemNameById(int id)
        {
            Item? item = GetItemById(id);

            return item is null ? $"Inconnu ({id})" : item.Name;
        }
    }
}
