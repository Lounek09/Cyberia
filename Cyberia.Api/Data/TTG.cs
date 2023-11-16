using System.Text.Json.Serialization;

namespace Cyberia.Api.Data
{
    public sealed class TTGCardData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("i")]
        public int Index { get; init; }

        [JsonPropertyName("o")]
        public int ItemId { get; init; }

        [JsonPropertyName("e")]
        public int TTGEntityId { get; init; }

        [JsonPropertyName("v")]
        public int Variant { get; init; }

        [JsonConstructor]
        internal TTGCardData()
        {

        }

        public ItemData? GetItemData()
        {
            return DofusApi.Datacenter.ItemsData.GetItemDataById(ItemId);
        }

        public TTGEntityData? GetTTGEntityData()
        {
            return DofusApi.Datacenter.TTGData.GetTTGEntityDataById(ItemId);
        }
    }

    public sealed class TTGEntityData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("i")]
        public int Index { get; init; }

        [JsonPropertyName("a")]
        public int Rarity { get; init; }

        [JsonPropertyName("f")]
        public int TTGFamilyId { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonConstructor]
        internal TTGEntityData()
        {
            Name = string.Empty;
        }

        public TTGFamilyData? GetTTGFamilyData()
        {
            return DofusApi.Datacenter.TTGData.GetTTGFamilyDataById(TTGFamilyId);
        }
    }

    public sealed class TTGFamilyData
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("i")]
        public int Index { get; set; }

        [JsonPropertyName("o")]
        public int ItemId { get; set; }

        [JsonPropertyName("n")]
        public string Name { get; set; }

        [JsonConstructor]
        internal TTGFamilyData()
        {
            Name = string.Empty;
        }

        public ItemData? GetItemData()
        {
            return DofusApi.Datacenter.ItemsData.GetItemDataById(ItemId);
        }
    }

    public sealed class TTGData
    {
        private const string FILE_NAME = "ttg.json";

        [JsonPropertyName("TTG.c")]
        public List<TTGCardData> TTGCards { get; set; }

        [JsonPropertyName("TTG.e")]
        public List<TTGEntityData> TTGEntities { get; set; }

        [JsonPropertyName("TTG.f")]
        public List<TTGFamilyData> TTGFamilies { get; set; }

        [JsonConstructor]
        public TTGData()
        {
            TTGCards = [];
            TTGEntities = [];
            TTGFamilies = [];
        }

        internal static TTGData Load()
        {
            return Json.LoadFromFile<TTGData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
        }

        public TTGCardData? GetTTGCardDataById(int id)
        {
            return TTGCards.Find(x => x.Id == id);
        }

        public TTGEntityData? GetTTGEntityDataById(int id)
        {
            return TTGEntities.Find(x => x.Id == id);
        }

        public string GetTTGEntityNameById(int id)
        {
            TTGEntityData? ttgEntityData = GetTTGEntityDataById(id);

            return ttgEntityData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : ttgEntityData.Name;
        }

        public TTGFamilyData? GetTTGFamilyDataById(int id)
        {
            return TTGFamilies.Find(x => x.Id == id);
        }

        public string GetTTGFamilyNameById(int id)
        {
            TTGFamilyData? ttgFamilyData = GetTTGFamilyDataById(id);

            return ttgFamilyData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : ttgFamilyData.Name;
        }
    }
}
