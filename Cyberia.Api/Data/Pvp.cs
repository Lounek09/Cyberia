using Cyberia.Api.JsonConverters;

using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data
{
    public sealed class PvpGradeData : IDofusData
    {
        [JsonPropertyName("nc")]
        public string ShortName { get; init; }

        [JsonPropertyName("nl")]
        public string Name { get; init; }

        [JsonConstructor]
        internal PvpGradeData()
        {
            ShortName = string.Empty;
            Name = string.Empty;
        }
    }

    public sealed class PvpData : IDofusData
    {
        private const string FILE_NAME = "pvp.json";

        [JsonPropertyName("PP.hp")]
        [JsonConverter(typeof(ReadOnlyCollectionConverter<int>))]
        public ReadOnlyCollection<int> HonnorPointThresholds { get; init; }

        [JsonPropertyName("PP.maxdp")]
        public int MaxDishonourPoint { get; init; }

        [JsonPropertyName("PP.grds")]
        [JsonConverter(typeof(ReadOnlyCollectionConverter<IEnumerable<PvpGradeData>>))]
        public ReadOnlyCollection<IEnumerable<PvpGradeData>> PvpGrades { get; init; }

        [JsonConstructor]
        internal PvpData()
        {
            HonnorPointThresholds = ReadOnlyCollection<int>.Empty;
            PvpGrades = ReadOnlyCollection<IEnumerable<PvpGradeData>>.Empty;
        }

        internal static PvpData Load()
        {
            return Datacenter.LoadDataFromFile<PvpData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
        }
    }
}
