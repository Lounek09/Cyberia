using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data
{
    public sealed class EmoteData : IDofusData<int>
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("s")]
        public string Shortcut { get; init; }

        [JsonConstructor]
        internal EmoteData()
        {
            Name = string.Empty;
            Shortcut = string.Empty;
        }
    }

    public sealed class EmotesData : IDofusData
    {
        private const string FILE_NAME = "emotes.json";

        [JsonPropertyName("EM")]
        [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, EmoteData>))]
        public FrozenDictionary<int, EmoteData> Emotes { get; init; }

        [JsonConstructor]
        internal EmotesData()
        {
            Emotes = FrozenDictionary<int, EmoteData>.Empty;
        }

        internal static EmotesData Load()
        {
            return Datacenter.LoadDataFromFile<EmotesData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
        }

        public EmoteData? GetEmoteById(int id)
        {
            Emotes.TryGetValue(id, out EmoteData? emoteData);
            return emoteData;
        }

        public string GetEmoteNameById(int id)
        {
            EmoteData? emoteData = GetEmoteById(id);

            return emoteData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : emoteData.Name;
        }
    }
}
