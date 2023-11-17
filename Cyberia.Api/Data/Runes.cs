using System.Text.Json.Serialization;

namespace Cyberia.Api.Data
{
    public sealed class RuneData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("w")]
        public double Weight { get; init; }

        [JsonPropertyName("p")]
        public int Power { get; init; }

        [JsonPropertyName("pa")]
        public bool HasPa { get; init; }

        [JsonPropertyName("ra")]
        public bool HasRa { get; init; }

        [JsonConstructor]
        internal RuneData()
        {
            Name = string.Empty;
        }

        public string GetFullName()
        {
            return PatternDecoder.Description(Resources.Rune, Name);
        }
    }
    public sealed class RunesData
    {
        private const string FILE_NAME = "runes.json";

        [JsonPropertyName("RU")]
        public List<RuneData> Runes { get; init; }

        [JsonConstructor]
        internal RunesData()
        {
            Runes = [];
        }

        internal static RunesData Load()
        {
            return Datacenter.LoadDataFromFile<RunesData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
        }

        public RuneData? GetRuneDataById(int id)
        {
            return Runes.Find(x => x.Id == id);
        }

        public RuneData? GetRuneDataByName(string name)
        {
            return Runes.Find(x => x.Name.NormalizeCustom().Equals(name.NormalizeCustom()));
        }

        public List<RuneData> GetRunesDataByName(string name)
        {
            string[] names = name.NormalizeCustom().Split(' ');
            return Runes.FindAll(x => names.All(x.Name.NormalizeCustom().Contains));
        }

        public string GetAllRuneName()
        {
            return string.Join(", ", Runes.Select(x => x.Name));
        }
    }
}
