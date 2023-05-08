using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class Rune
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

        public Rune()
        {
            Name = string.Empty;
        }

        public string GetFullName()
        {
            return $"Rune {Name}";
        }
    }
    public sealed class RunesData
    {
        private const string FILE_NAME = "runes.json";

        [JsonPropertyName("RU")]
        public List<Rune> Runes { get; init; }

        public RunesData()
        {
            Runes = new();
        }

        internal static RunesData Build()
        {
            return Json.LoadFromFile<RunesData>($"{DofusApi.OUTPUT_PATH}/{FILE_NAME}");
        }

        public Rune? GetRuneById(int id)
        {
            return Runes.Find(x => x.Id == id);
        }

        public Rune? GetRuneByName(string name)
        {
            return Runes.Find(x => x.Name.RemoveDiacritics().Equals(name.RemoveDiacritics()));
        }

        public List<Rune> GetRunesByName(string name)
        {
            string[] names = name.RemoveDiacritics().Split(' ');
            return Runes.FindAll(x => names.All(x.Name.RemoveDiacritics().Contains));
        }

        public string GetAllRuneName()
        {
            List<string> runesName = new();

            foreach (Rune rune in Runes)
                runesName.Add(rune.Name);

            return string.Join(", ", runesName);
        }
    }
}
