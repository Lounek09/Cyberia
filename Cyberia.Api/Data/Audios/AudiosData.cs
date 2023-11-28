using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Audios;

public sealed class AudiosData : IDofusData
{
    private const string FILE_NAME = "audio.json";

    [JsonPropertyName("AUMC")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, AudioMusicContentData>))]
    internal FrozenDictionary<int, AudioMusicContentData> AudioMusicsContent { get; init; }

    [JsonPropertyName("AUM")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, AudioMusicData>))]
    public FrozenDictionary<int, AudioMusicData> AudioMusics { get; init; }

    [JsonPropertyName("AUEC")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, AudioEffectContentData>))]
    internal FrozenDictionary<int, AudioEffectContentData> AudioEffectsContent { get; init; }

    [JsonPropertyName("AUE")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, AudioEffectData>))]
    public FrozenDictionary<int, AudioEffectData> AudioEffects { get; init; }

    [JsonPropertyName("AUAC")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, AudioEnvironmentContentData>))]
    internal FrozenDictionary<int, AudioEnvironmentContentData> AudioEnvironmentsContent { get; init; }

    [JsonPropertyName("AUA")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, AudioEnvironmentData>))]
    public FrozenDictionary<int, AudioEnvironmentData> AudioEnvironments { get; init; }

    [JsonConstructor]
    internal AudiosData()
    {
        AudioMusicsContent = FrozenDictionary<int, AudioMusicContentData>.Empty;
        AudioMusics = FrozenDictionary<int, AudioMusicData>.Empty;
        AudioEffectsContent = FrozenDictionary<int, AudioEffectContentData>.Empty;
        AudioEffects = FrozenDictionary<int, AudioEffectData>.Empty;
        AudioEnvironmentsContent = FrozenDictionary<int, AudioEnvironmentContentData>.Empty;
        AudioEnvironments = FrozenDictionary<int, AudioEnvironmentData>.Empty;
    }

    internal static AudiosData Load()
    {
        var data = Datacenter.LoadDataFromFile<AudiosData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));

        foreach (var audioMusicData in data.AudioMusics.Values)
        {
            audioMusicData.Name = data.GetAudioMusicNameById(audioMusicData.Id);
        }

        foreach (var audioEffectData in data.AudioEffects.Values)
        {
            audioEffectData.Name = data.GetAudioEffectNameById(audioEffectData.Id);
        }

        foreach (var audioEnvironmentData in data.AudioEnvironments.Values)
        {
            audioEnvironmentData.Name = data.GetAudioEnvironmentNameById(audioEnvironmentData.Id);
        }

        return data;
    }

    public AudioMusicData? GetAudioMusicDataById(int id)
    {
        AudioMusics.TryGetValue(id, out var audioMusicData);
        return audioMusicData;
    }

    public string GetAudioMusicNameById(int id)
    {
        AudioMusicsContent.TryGetValue(id, out var audioMusicContentData);
        return audioMusicContentData?.Name ?? PatternDecoder.Description(Resources.Unknown_Data, id);
    }

    public AudioEffectData? GetAudioEffectDataById(int id)
    {
        AudioEffects.TryGetValue(id, out var audioEffectData);
        return audioEffectData;
    }

    public string GetAudioEffectNameById(int id)
    {
        AudioEffectsContent.TryGetValue(id, out var audioEffectContentData);
        return audioEffectContentData?.Name ?? PatternDecoder.Description(Resources.Unknown_Data, id);
    }

    public AudioEnvironmentData? GetAudioEnvironmentDataById(int id)
    {
        AudioEnvironments.TryGetValue(id, out var audioEnvironmentData);
        return audioEnvironmentData;
    }

    public string GetAudioEnvironmentNameById(int id)
    {
        AudioEnvironmentsContent.TryGetValue(id, out var audioEnvironmentContentData);
        return audioEnvironmentContentData?.Name ?? PatternDecoder.Description(Resources.Unknown_Data, id);
    }
}
