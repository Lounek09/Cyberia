using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class TTGCard
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

        public TTGCard()
        {

        }

        public Item? GetItem()
        {
            return DofusApi.Instance.Datacenter.ItemsData.GetItemById(ItemId);
        }

        public TTGEntity? GetTTGEntity()
        {
            return DofusApi.Instance.Datacenter.TTGData.GetTTGEntityById(ItemId);
        }
    }

    public sealed class TTGEntity
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

        public TTGEntity()
        {
            Name = string.Empty;
        }

        public TTGFamily? GetTTGFamily()
        {
            return DofusApi.Instance.Datacenter.TTGData.GetTTGFamilyById(TTGFamilyId);
        }
    }

    public sealed class TTGFamily
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("i")]
        public int Index { get; set; }

        [JsonPropertyName("o")]
        public int ItemId { get; set; }

        [JsonPropertyName("n")]
        public string Name { get; set; }

        public TTGFamily()
        {
            Name = string.Empty;
        }

        public Item? GetItem()
        {
            return DofusApi.Instance.Datacenter.ItemsData.GetItemById(ItemId);
        }
    }

    public sealed class TTGData
    {
        private const string FILE_NAME = "ttg.json";

        [JsonPropertyName("TTG.c")]
        public List<TTGCard> TTGCards { get; set; }

        [JsonPropertyName("TTG.e")]
        public List<TTGEntity> TTGEntities { get; set; }

        [JsonPropertyName("TTG.f")]
        public List<TTGFamily> TTGFamilies { get; set; }

        public TTGData()
        {
            TTGCards = new();
            TTGEntities = new();
            TTGFamilies = new();
        }

        internal static TTGData Build()
        {
            return Json.LoadFromFile<TTGData>($"{DofusApi.OUTPUT_PATH}/{FILE_NAME}");
        }

        public TTGCard? GetTTGCardById(int id)
        {
            return TTGCards.Find(x => x.Id == id);
        }

        public TTGEntity? GetTTGEntityById(int id)
        {
            return TTGEntities.Find(x => x.Id == id);
        }

        public string GetTTGEntityNameById(int id)
        {
            TTGEntity? ttgEntity = GetTTGEntityById(id);

            return ttgEntity is null ? $"Inconnu ({id})" : ttgEntity.Name;
        }

        public TTGFamily? GetTTGFamilyById(int id)
        {
            return TTGFamilies.Find(x => x.Id == id);
        }

        public string GetTTGFamilyNameById(int id)
        {
            TTGFamily? ttgFamily = GetTTGFamilyById(id);

            return ttgFamily is null ? $"Inconnu ({id})" : ttgFamily.Name;
        }
    }
}
