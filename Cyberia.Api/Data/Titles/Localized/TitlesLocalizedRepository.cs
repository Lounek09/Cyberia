using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Titles.Localized;

internal sealed class TitlesLocalizedRepository : DofusLocalizedRepository, IDofusRepository
{
    public static string FileName => TitlesRepository.FileName;

    [JsonPropertyName("PT")]
    public IReadOnlyList<TitleLocalizedData> Titles { get; init; }

    [JsonConstructor]
    internal TitlesLocalizedRepository()
    {
        Titles = [];
    }
}
