using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Audios;

public sealed class AudiosData : IDofusData
{
    private const string c_fileName = "audio.json";

    private static readonly string s_filePath = Path.Join(DofusApi.OutputPath, c_fileName);

    [JsonPropertyName("AUMC")]
    [JsonInclude]
    internal IReadOnlyDictionary<string, int> AudioMusicsContent { get; init; }

    [JsonPropertyName("AUM")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, AudioMusicData>))]
    public FrozenDictionary<int, AudioMusicData> AudioMusics { get; init; }

    [JsonPropertyName("AUEC")]
    [JsonInclude]
    internal IReadOnlyDictionary<string, int> AudioEffectsContent { get; init; }

    [JsonPropertyName("AUE")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, AudioEffectData>))]
    public FrozenDictionary<int, AudioEffectData> AudioEffects { get; init; }

    [JsonPropertyName("AUAC")]
    [JsonInclude]
    internal IReadOnlyDictionary<string, int> AudioEnvironmentsContent { get; init; }

    [JsonPropertyName("AUA")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, AudioEnvironmentData>))]
    public FrozenDictionary<int, AudioEnvironmentData> AudioEnvironments { get; init; }

    [JsonConstructor]
    internal AudiosData()
    {
        AudioMusicsContent = ReadOnlyDictionary<string, int>.Empty;
        AudioMusics = FrozenDictionary<int, AudioMusicData>.Empty;
        AudioEffectsContent = ReadOnlyDictionary<string, int>.Empty;
        AudioEffects = FrozenDictionary<int, AudioEffectData>.Empty;
        AudioEnvironmentsContent = ReadOnlyDictionary<string, int>.Empty;
        AudioEnvironments = FrozenDictionary<int, AudioEnvironmentData>.Empty;
    }

    internal static async Task<AudiosData> LoadAsync()
    {
        return await Datacenter.LoadDataAsync<AudiosData>(s_filePath);
    }

    public AudioMusicData? GetAudioMusicDataById(int id)
    {
        AudioMusics.TryGetValue(id, out var audioMusicData);
        return audioMusicData;
    }

    public AudioMusicData? GetAudioMusicDataByName(string name)
    {
        AudioMusicsContent.TryGetValue(name, out var id);
        return GetAudioMusicDataById(id);
    }

    public AudioEffectData? GetAudioEffectDataById(int id)
    {
        AudioEffects.TryGetValue(id, out var audioEffectData);
        return audioEffectData;
    }

    public AudioEffectData? GetAudioEffectDataByName(string name)
    {
        AudioEffectsContent.TryGetValue(name, out var id);
        return GetAudioEffectDataById(id);
    }

    public AudioEnvironmentData? GetAudioEnvironmentDataById(int id)
    {
        AudioEnvironments.TryGetValue(id, out var audioEnvironmentData);
        return audioEnvironmentData;
    }

    public AudioEnvironmentData? GetAudioEnvironmentDataByName(string name)
    {
        AudioEnvironmentsContent.TryGetValue(name, out var id);
        return GetAudioEnvironmentDataById(id);
    }
}
