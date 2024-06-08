using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Titles;

public sealed class TitlesRepository : IDofusRepository
{
    private const string c_fileName = "titles.json";

    [JsonPropertyName("PT")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, TitleData>))]
    public FrozenDictionary<int, TitleData> Titles { get; init; }

    [JsonConstructor]
    internal TitlesRepository()
    {
        Titles = FrozenDictionary<int, TitleData>.Empty;
    }

    internal static TitlesRepository Load(string directoryPath)
    {
        var filePath = Path.Join(directoryPath, c_fileName);

        return Datacenter.LoadRepository<TitlesRepository>(filePath);
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
            ? Translation.Format(ApiTranslations.Unknown_Data, id)
            : titleData.Name;
    }
}
