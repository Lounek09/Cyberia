using Cyberia.Api.JsonConverters;

using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Audios;

public sealed class AudioEnvironmentData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("bg")]
    public IReadOnlyList<int> BackgroundAudioEffectIds { get; init; }

    [JsonPropertyName("mind")]
    public int MinNoiseDelay { get; init; }

    [JsonPropertyName("maxd")]
    public int MaxNoiseDelay { get; init; }

    [JsonPropertyName("n")]
    public IReadOnlyList<int> NoiseAudioEffectIds { get; init; }

    [JsonIgnore]
    public string Name { get; internal set; }

    [JsonConstructor]
    internal AudioEnvironmentData()
    {
        BackgroundAudioEffectIds = [];
        NoiseAudioEffectIds = [];
        Name = string.Empty;
    }

    public IEnumerable<AudioEffectData> GetBackgroundAudioEffectsData()
    {
        foreach (var id in BackgroundAudioEffectIds)
        {
            var audioEffectData = DofusApi.Datacenter.AudiosData.GetAudioEffectDataById(id);
            if (audioEffectData is not null)
            {
                yield return audioEffectData;
            }
        }
    }

    public IEnumerable<AudioEffectData> GetNoiseAudioEffectsData()
    {
        foreach (var id in NoiseAudioEffectIds)
        {
            var audioEffectData = DofusApi.Datacenter.AudiosData.GetAudioEffectDataById(id);
            if (audioEffectData is not null)
            {
                yield return audioEffectData;
            }
        }
    }
}
