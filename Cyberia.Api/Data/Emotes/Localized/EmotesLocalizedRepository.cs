using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Emotes.Localized;

internal sealed class EmotesLocalizedRepository : DofusLocalizedRepository, IDofusRepository
{
    public static string FileName => EmotesRepository.FileName;

    [JsonPropertyName("EM")]
    public IReadOnlyList<EmoteLocalizedData> Emotes { get; init; }

    [JsonConstructor]
    internal EmotesLocalizedRepository()
    {
        Emotes = [];
    }
}
