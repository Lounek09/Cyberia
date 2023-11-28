using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Npcs;

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
        NpcActions.TryGetValue(id, out var npcActionData);
        return npcActionData;
    }

    public NpcData? GetNpcDataById(int id)
    {
        Npcs.TryGetValue(id, out var npcData);
        return npcData;
    }

    public string GetNpcNameById(int id)
    {
        var npc = GetNpcDataById(id);

        return npc is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : npc.Name;
    }
}
