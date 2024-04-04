using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Effects;

public sealed class EffectsData
    : IDofusData
{
    private const string FILE_NAME = "effects.json";
    private static readonly string FILE_PATH = Path.Join(DofusApi.OUTPUT_PATH, FILE_NAME);

    [JsonPropertyName("E")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, EffectData>))]
    public FrozenDictionary<int, EffectData> Effects { get; init; }

    [JsonConstructor]
    internal EffectsData()
    {
        Effects = FrozenDictionary<int, EffectData>.Empty;
    }

    internal static async Task<EffectsData> LoadAsync()
    {
        return await Datacenter.LoadDataAsync<EffectsData>(FILE_PATH);
    }

    public EffectData? GetEffectDataById(int id)
    {
        Effects.TryGetValue(id, out var effectData);
        return effectData;
    }
}
