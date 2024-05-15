using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Npcs;

public sealed class NpcsRepository : IDofusRepository
{
    private const string c_fileName = "npc.json";

    [JsonPropertyName("N.a")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, NpcActionData>))]
    public FrozenDictionary<int, NpcActionData> NpcActions { get; init; }

    [JsonPropertyName("N.d")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, NpcData>))]
    public FrozenDictionary<int, NpcData> Npcs { get; init; }

    [JsonConstructor]
    internal NpcsRepository()
    {
        NpcActions = FrozenDictionary<int, NpcActionData>.Empty;
        Npcs = FrozenDictionary<int, NpcData>.Empty;
    }

    internal static NpcsRepository Load(string directoryPath)
    {
        var filePath = Path.Join(directoryPath, c_fileName);

        return Datacenter.LoadRepository<NpcsRepository>(filePath);
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

        return npc is null
            ? PatternDecoder.Description(Resources.Unknown_Data, id)
            : npc.Name;
    }
}
