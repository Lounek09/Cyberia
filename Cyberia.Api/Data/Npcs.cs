using System.Text.Json.Serialization;

namespace Cyberia.Api.Data
{
    public sealed class NpcActionData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("v")]
        public string Name { get; init; }

        [JsonConstructor]
        internal NpcActionData()
        {
            Name = string.Empty;
        }
    }

    public sealed class NpcData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("a")]
        public List<int> NpcActionsId { get; init; }

        [JsonConstructor]
        internal NpcData()
        {
            Name = string.Empty;
            NpcActionsId = [];
        }

        public IEnumerable<NpcActionData> GetNpcActionsData()
        {
            foreach (int npcActionId in NpcActionsId)
            {
                NpcActionData? npcActionData = DofusApi.Datacenter.NpcsData.GetNpcActionDataById(npcActionId);
                if (npcActionData is not null)
                {
                    yield return npcActionData;
                }
            }
        }
    }

    public sealed class NpcsData
    {
        private const string FILE_NAME = "npc.json";

        [JsonPropertyName("N.a")]
        public List<NpcActionData> NpcActions { get; init; }

        [JsonPropertyName("N.d")]
        public List<NpcData> Npcs { get; init; }

        [JsonConstructor]
        public NpcsData()
        {
            NpcActions = [];
            Npcs = [];
        }

        internal static NpcsData Load()
        {
            return Json.LoadFromFile<NpcsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
        }

        public NpcActionData? GetNpcActionDataById(int id)
        {
            return NpcActions.Find(x => x.Id == id);
        }

        public NpcData? GetNpcDataById(int id)
        {
            return Npcs.Find(x => x.Id == id);
        }

        public string GetNpcNameById(int id)
        {
            NpcData? npc = GetNpcDataById(id);

            return npc is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : npc.Name;
        }
    }
}
