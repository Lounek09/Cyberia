using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class Title
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("t")]
        public string Name { get; init; }

        [JsonPropertyName("c")]
        public int Color { get; init; }

        [JsonPropertyName("pt")]
        public int ParametersType { get; init; }

        public Title()
        {
            Name = string.Empty;
        }
    }

    public sealed class TitlesData
    {
        private const string FILE_NAME = "titles.json";

        [JsonPropertyName("PT")]
        public List<Title> Titles { get; init; }

        public TitlesData()
        {
            Titles = new();
        }

        internal static TitlesData Build()
        {
            return Json.LoadFromFile<TitlesData>($"{DofusApi.OUTPUT_PATH}/{FILE_NAME}");
        }

        public Title? GetTitleById(int id)
        {
            return Titles.Find(x => x.Id == id);
        }

        public string GetTitleNameById(int id)
        {
            Title? title = GetTitleById(id);

            return title is null ? $"Inconnu ({id})" : title.Name;
        }
    }
}
