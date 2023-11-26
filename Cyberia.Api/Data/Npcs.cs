using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data
{
    public sealed class NpcActionData : IDofusData<int>
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

    public sealed class NpcData : IDofusData<int>
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("a")]
        [JsonConverter(typeof(ReadOnlyCollectionConverter<int>))]
        public ReadOnlyCollection<int> NpcActionsId { get; init; }

        [JsonConstructor]
        internal NpcData()
        {
            Name = string.Empty;
            NpcActionsId = ReadOnlyCollection<int>.Empty;
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

    public sealed class NpcsData : IDofusData
    {
        private const string FILE_NAME = "npc.json";

        [JsonPropertyName("N.a")]
        [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, NpcActionData>))]
        public FrozenDictionary<int, NpcActionData> NpcActions { get; init; }

        [JsonPropertyName("N.d")]
        [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, NpcData>))]
        public FrozenDictionary<int, NpcData> Npcs { get; init; }

        [JsonConstructor]
        internal NpcsData()
        {
            NpcActions = FrozenDictionary<int, NpcActionData>.Empty;
            Npcs = FrozenDictionary<int, NpcData>.Empty;
        }

        internal static NpcsData Load()
        {
            return Datacenter.LoadDataFromFile<NpcsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
        }

        public NpcActionData? GetNpcActionDataById(int id)
        {
            NpcActions.TryGetValue(id, out NpcActionData? npcActionData);
            return npcActionData;
        }

        public NpcData? GetNpcDataById(int id)
        {
            Npcs.TryGetValue(id, out NpcData? npcData);
            return npcData;
        }

        public string GetNpcNameById(int id)
        {
            NpcData? npc = GetNpcDataById(id);

            return npc is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : npc.Name;
        }
    }
}
