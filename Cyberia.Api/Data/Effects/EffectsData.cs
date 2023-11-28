using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Effects;

public sealed class EffectsData : IDofusData
{
    private const string FILE_NAME = "effects.json";

    [JsonPropertyName("E")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, EffectData>))]
    public FrozenDictionary<int, EffectData> Effects { get; init; }

    [JsonConstructor]
    internal EffectsData()
    {
        Effects = FrozenDictionary<int, EffectData>.Empty;
    }

    internal static EffectsData Load()
    {
        return Datacenter.LoadDataFromFile<EffectsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
    }

    public EffectData? GetEffectDataById(int id)
    {
        Effects.TryGetValue(id, out var effectData);
        return effectData;
    }
}
