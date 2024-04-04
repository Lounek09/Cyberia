using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Titles;

public sealed class TitlesData
    : IDofusData
{
    private const string FILE_NAME = "titles.json";
    private static readonly string FILE_PATH = Path.Join(DofusApi.OUTPUT_PATH, FILE_NAME);

    [JsonPropertyName("PT")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, TitleData>))]
    public FrozenDictionary<int, TitleData> Titles { get; init; }

    [JsonConstructor]
    internal TitlesData()
    {
        Titles = FrozenDictionary<int, TitleData>.Empty;
    }

    internal static async Task<TitlesData> LoadAsync()
    {
        return await Datacenter.LoadDataAsync<TitlesData>(FILE_PATH);
    }

    public TitleData? GetTitleDataById(int id)
    {
        Titles.TryGetValue(id, out var titleData);
        return titleData;
    }

    public string GetTitleNameById(int id)
    {
        var titleData = GetTitleDataById(id);

        return titleData is null
            ? PatternDecoder.Description(Resources.Unknown_Data, id)
            : titleData.Name;
    }
}
