using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Alignments.Localized;

internal sealed class AlignmentsLocalizedRepository : DofusLocalizedRepository, IDofusRepository
{
    public static string FileName => AlignmentsRepository.FileName;

    [JsonPropertyName("A.a")]
    public IReadOnlyList<AlignmentLocalizedData> Alignments { get; init; }

    [JsonPropertyName("A.o")]
    public IReadOnlyList<AlignmentOrderLocalizedData> AlignmentOrders { get; init; }

    [JsonPropertyName("A.f")]
    public IReadOnlyList<AlignmentFeatLocalizedData> AlignmentFeats { get; init; }

    [JsonPropertyName("A.fe")]
    public IReadOnlyList<AlignmentFeatEffectLocalizedData> AlignmentFeatEffects { get; init; }

    [JsonPropertyName("A.b")]
    public IReadOnlyList<AlignmentBalanceLocalizedData> AlignmentBalances { get; init; }

    [JsonPropertyName("A.s")]
    public IReadOnlyList<AlignmentSpecializationLocalizedData> AlignmentSpecializations { get; init; }

    [JsonConstructor]
    internal AlignmentsLocalizedRepository()
    {
        Alignments = ReadOnlyCollection<AlignmentLocalizedData>.Empty;
        AlignmentOrders = ReadOnlyCollection<AlignmentOrderLocalizedData>.Empty;
        AlignmentFeats = ReadOnlyCollection<AlignmentFeatLocalizedData>.Empty;
        AlignmentFeatEffects = ReadOnlyCollection<AlignmentFeatEffectLocalizedData>.Empty;
        AlignmentBalances = ReadOnlyCollection<AlignmentBalanceLocalizedData>.Empty;
        AlignmentSpecializations = ReadOnlyCollection<AlignmentSpecializationLocalizedData>.Empty;
    }
}
