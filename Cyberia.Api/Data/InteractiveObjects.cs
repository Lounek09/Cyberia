using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data;

internal sealed class InteractiveObjectGfxData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int GfxId { get; init; }

    [JsonPropertyName("v")]
    public int Id { get; init; }

    [JsonConstructor]
    internal InteractiveObjectGfxData()
    {

    }
}

public sealed class InteractiveObjectData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonPropertyName("t")]
    public int Type { get; init; }

    [JsonPropertyName("sk")]
    [JsonConverter(typeof(ReadOnlyCollectionConverter<int>))]
    public ReadOnlyCollection<int> SkillsId { get; init; }

    [JsonIgnore]
    public int GfxId { get; internal set; }

    [JsonConstructor]
    internal InteractiveObjectData()
    {
        Name = string.Empty;
        SkillsId = ReadOnlyCollection<int>.Empty;
    }

    public IEnumerable<SkillData> GetSkillsData()
    {
        foreach (var skillId in SkillsId)
        {
            var skillData = DofusApi.Datacenter.SkillsData.GetSkillDataById(skillId);
            if (skillData is not null)
            {
                yield return skillData;
            }
        }
    }
}

public sealed class InteractiveObjectsData : IDofusData
{
    private const string FILE_NAME = "interactiveobjects.json";

    [JsonPropertyName("IO.g")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, InteractiveObjectGfxData>))]
    internal FrozenDictionary<int, InteractiveObjectGfxData> InteractiveObjectsGfx { get; init; }

    [JsonPropertyName("IO.d")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, InteractiveObjectData>))]
    public FrozenDictionary<int, InteractiveObjectData> InteractiveObjects { get; init; }

    [JsonConstructor]
    internal InteractiveObjectsData()
    {
        InteractiveObjectsGfx = FrozenDictionary<int, InteractiveObjectGfxData>.Empty;
        InteractiveObjects = FrozenDictionary<int, InteractiveObjectData>.Empty;
    }

    internal static InteractiveObjectsData Load()
    {
        var data = Datacenter.LoadDataFromFile<InteractiveObjectsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));

        foreach (var interactiveObjectData in data.InteractiveObjects.Values)
        {
            var interactiveObjectGfxData = data.GetInteractiveObjectGfxDataById(interactiveObjectData.GfxId);
            if (interactiveObjectGfxData is not null)
            {
                interactiveObjectData.GfxId = interactiveObjectGfxData.GfxId;
            }
        }

        return data;
    }

    internal InteractiveObjectGfxData? GetInteractiveObjectGfxDataById(int id)
    {
        InteractiveObjectsGfx.TryGetValue(id, out var interactiveObjectGfxData);
        return interactiveObjectGfxData;
    }

    public InteractiveObjectData? GetInteractiveObjectDataById(int id)
    {
        InteractiveObjects.TryGetValue(id, out var interactiveObjectData);
        return interactiveObjectData;
    }
}
