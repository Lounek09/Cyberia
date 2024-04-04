using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Effects;

public sealed class EffectsData
    : IDofusData
{
    private const string c_fileName = "effects.json";

    private static readonly string s_filePath = Path.Join(DofusApi.OutputPath, c_fileName);

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
        return await Datacenter.LoadDataAsync<EffectsData>(s_filePath);
    }

    public EffectData? GetEffectDataById(int id)
    {
        Effects.TryGetValue(id, out var effectData);
        return effectData;
    }
}
