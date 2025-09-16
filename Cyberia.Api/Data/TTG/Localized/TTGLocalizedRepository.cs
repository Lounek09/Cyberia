using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.TTG.Localized;

internal sealed class TTGLocalizedRepository : DofusLocalizedRepository, IDofusRepository
{
    public static string FileName => TTGRepository.FileName;

    [JsonPropertyName("TTG.e")]
    public IReadOnlyList<TTGEntityLocalizedData> TTGEntities { get; init; }

    [JsonPropertyName("TTG.f")]
    public IReadOnlyList<TTGFamilyLocalizedData> TTGFamilies { get; init; }

    [JsonConstructor]
    internal TTGLocalizedRepository()
    {
        TTGEntities = ReadOnlyCollection<TTGEntityLocalizedData>.Empty;
        TTGFamilies = ReadOnlyCollection<TTGFamilyLocalizedData>.Empty;
    }
}
