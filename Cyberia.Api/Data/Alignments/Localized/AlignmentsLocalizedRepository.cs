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
        Alignments = [];
        AlignmentOrders = [];
        AlignmentFeats = [];
        AlignmentFeatEffects = [];
        AlignmentBalances = [];
        AlignmentSpecializations = [];
    }
}
