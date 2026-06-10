using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Spells.Localized;

internal sealed class SpellsLocalizedRepository : DofusLocalizedRepository, IDofusRepository
{
    public static string FileName => SpellsRepository.FileName;

    [JsonPropertyName("S")]
    public IReadOnlyList<SpellLocalizedData> Spells { get; init; }

    [JsonConstructor]
    internal SpellsLocalizedRepository()
    {
        Spells = ReadOnlyCollection<SpellLocalizedData>.Empty;
    }
}
