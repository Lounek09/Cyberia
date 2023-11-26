using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data
{
    internal sealed class AudioMusicContentData : IDofusData<int>
    {
        [JsonPropertyName("id")]
        public string Name { get; init; }

        [JsonPropertyName("v")]
        public int Id { get; init; }

        [JsonConstructor]
        internal AudioMusicContentData()
        {
            Name = string.Empty;
        }
    }

    public sealed class AudioMusicData : IDofusData<int>
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("f")]
        public string FileName { get; init; }

        [JsonPropertyName("v")]
        public int BaseVolume { get; init; }

        [JsonPropertyName("l")]
        public bool Loop { get; init; }

        [JsonPropertyName("s")]
        public bool Streaming { get; init; }

        [JsonPropertyName("o")]
        public int Offset { get; init; }

        [JsonIgnore]
        public string Name { get; internal set; }

        [JsonConstructor]
        internal AudioMusicData()
        {
            FileName = string.Empty;
            Name = string.Empty;
        }
    }

    internal sealed class AudioEffectContentData : IDofusData<int>
    {
        [JsonPropertyName("id")]
        public string Name { get; init; }

        [JsonPropertyName("v")]
        public int Id { get; init; }

        [JsonConstructor]
        internal AudioEffectContentData()
        {
            Name = string.Empty;
        }
    }

    public sealed class AudioEffectData : IDofusData<int>
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("f")]
        public string FileName { get; init; }

        [JsonPropertyName("v")]
        public int BaseVolume { get; init; }

        [JsonPropertyName("l")]
        public bool Loop { get; init; }

        [JsonPropertyName("s")]
        public bool Streaming { get; init; }

        [JsonPropertyName("o")]
        public int Offset { get; init; }

        [JsonIgnore]
        public string Name { get; internal set; }

        [JsonConstructor]
        internal AudioEffectData()
        {
            FileName = string.Empty;
            Name = string.Empty;
        }
    }

    internal sealed class AudioEnvironmentContentData : IDofusData<int>
    {
        [JsonPropertyName("id")]
        public string Name { get; init; }

        [JsonPropertyName("v")]
        public int Id { get; init; }

        [JsonConstructor]
        internal AudioEnvironmentContentData()
        {
            Name = string.Empty;
        }
    }

    public sealed class AudioEnvironmentData : IDofusData<int>
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("bg")]
        [JsonConverter(typeof(ReadOnlyCollectionConverter<int>))]
        public ReadOnlyCollection<int> BackgroundAudioEffectIds { get; init; }

        [JsonPropertyName("mind")]
        public int MinNoiseDelay { get; init; }

        [JsonPropertyName("maxd")]
        public int MaxNoiseDelay { get; init; }

        [JsonPropertyName("n")]
        [JsonConverter(typeof(ReadOnlyCollectionConverter<int>))]
        public ReadOnlyCollection<int> NoiseAudioEffectIds { get; init; }

        [JsonIgnore]
        public string Name { get; internal set; }

        [JsonConstructor]
        internal AudioEnvironmentData()
        {
            BackgroundAudioEffectIds = ReadOnlyCollection<int>.Empty;
            NoiseAudioEffectIds = ReadOnlyCollection<int>.Empty;
            Name = string.Empty;
        }

        public IEnumerable<AudioEffectData> GetBackgroundAudioEffectsData()
        {
            foreach (int id in BackgroundAudioEffectIds)
            {
                AudioEffectData? audioEffectData = DofusApi.Datacenter.AudiosData.GetAudioEffectDataById(id);
                if (audioEffectData is not null)
                {
                    yield return audioEffectData;
                }
            }
        }

        public IEnumerable<AudioEffectData> GetNoiseAudioEffectsData()
        {
            foreach (int id in NoiseAudioEffectIds)
            {
                AudioEffectData? audioEffectData = DofusApi.Datacenter.AudiosData.GetAudioEffectDataById(id);
                if (audioEffectData is not null)
                {
                    yield return audioEffectData;
                }
            }
        }
    }

    public sealed class AudioData : IDofusData
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
        internal AudioData()
        {
            AudioMusicsContent = FrozenDictionary<int, AudioMusicContentData>.Empty;
            AudioMusics = FrozenDictionary<int, AudioMusicData>.Empty;
            AudioEffectsContent = FrozenDictionary<int, AudioEffectContentData>.Empty;
            AudioEffects = FrozenDictionary<int, AudioEffectData>.Empty;
            AudioEnvironmentsContent = FrozenDictionary<int, AudioEnvironmentContentData>.Empty;
            AudioEnvironments = FrozenDictionary<int, AudioEnvironmentData>.Empty;
        }

        internal static AudioData Load()
        {
            AudioData data = Datacenter.LoadDataFromFile<AudioData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));

            foreach (AudioMusicData audioMusicData in data.AudioMusics.Values)
            {
                audioMusicData.Name = data.GetAudioMusicNameById(audioMusicData.Id);
            }

            foreach (AudioEffectData audioEffectData in data.AudioEffects.Values)
            {
                audioEffectData.Name = data.GetAudioEffectNameById(audioEffectData.Id);
            }

            foreach (AudioEnvironmentData audioEnvironmentData in data.AudioEnvironments.Values)
            {
                audioEnvironmentData.Name = data.GetAudioEnvironmentNameById(audioEnvironmentData.Id);
            }

            return data;
        }

        public AudioMusicData? GetAudioMusicDataById(int id)
        {
            AudioMusics.TryGetValue(id, out AudioMusicData? audioMusicData);
            return audioMusicData;
        }

        public string GetAudioMusicNameById(int id)
        {
            AudioMusicsContent.TryGetValue(id, out AudioMusicContentData? audioMusicContentData);
            return audioMusicContentData?.Name ?? PatternDecoder.Description(Resources.Unknown_Data, id);
        }

        public AudioEffectData? GetAudioEffectDataById(int id)
        {
            AudioEffects.TryGetValue(id, out AudioEffectData? audioEffectData);
            return audioEffectData;
        }

        public string GetAudioEffectNameById(int id)
        {
            AudioEffectsContent.TryGetValue(id, out AudioEffectContentData? audioEffectContentData);
            return audioEffectContentData?.Name ?? PatternDecoder.Description(Resources.Unknown_Data, id);
        }

        public AudioEnvironmentData? GetAudioEnvironmentDataById(int id)
        {
            AudioEnvironments.TryGetValue(id, out AudioEnvironmentData? audioEnvironmentData);
            return audioEnvironmentData;
        }

        public string GetAudioEnvironmentNameById(int id)
        {
            AudioEnvironmentsContent.TryGetValue(id, out AudioEnvironmentContentData? audioEnvironmentContentData);
            return audioEnvironmentContentData?.Name ?? PatternDecoder.Description(Resources.Unknown_Data, id);
        }
    }
}
