using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.InteractiveObjects;

public sealed class InteractiveObjectsData
    : IDofusData
{
    private const string c_fileName = "interactiveobjects.json";

    private static readonly string s_filePath = Path.Join(DofusApi.OutputPath, c_fileName);

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

    internal static async Task<InteractiveObjectsData> LoadAsync()
    {
        var data = await Datacenter.LoadDataAsync<InteractiveObjectsData>(s_filePath);

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
