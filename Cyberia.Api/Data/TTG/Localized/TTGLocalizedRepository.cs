using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.TTG.Localized;

internal sealed class TTGLocalizedRepository : DofusLocalizedRepository, IDofusRepository
{
    public static string FileName => TTGRepository.FileName;

    [JsonPropertyName("TTG.e")]
    public IReadOnlyList<TTGEntityLocalizedData> TTGEntities { get; set; }

    [JsonPropertyName("TTG.f")]
    public IReadOnlyList<TTGFamilyLocalizedData> TTGFamilies { get; set; }

    [JsonConstructor]
    internal TTGLocalizedRepository()
    {
        TTGEntities = [];
        TTGFamilies = [];
    }
}
