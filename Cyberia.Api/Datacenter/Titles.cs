using System.Text.Json.Serialization;

namespace Cyberia.Api.Data
{
    public sealed class TitleData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("t")]
        public string Name { get; init; }

        [JsonPropertyName("c")]
        public int Color { get; init; }

        [JsonPropertyName("pt")]
        public int ParametersType { get; init; }

        public TitleData()
        {
            Name = string.Empty;
        }
    }

    public sealed class TitlesData
    {
        private const string FILE_NAME = "titles.json";

        [JsonPropertyName("PT")]
        public List<TitleData> Titles { get; init; }

        public TitlesData()
        {
            Titles = new();
        }

        internal static TitlesData Build()
        {
            return Json.LoadFromFile<TitlesData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
        }

        public TitleData? GetTitleDataById(int id)
        {
            return Titles.Find(x => x.Id == id);
        }

        public string GetTitleNameById(int id)
        {
            TitleData? titleData = GetTitleDataById(id);

            return titleData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : titleData.Name;
        }
    }
}
