using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Effects.Localized;

internal sealed class EffectsLocalizedRepository : DofusLocalizedRepository, IDofusRepository
{
    public static string FileName => EffectsRepository.FileName;

    [JsonPropertyName("E")]
    public IReadOnlyList<EffectLocalizedData> Effects { get; init; }

    [JsonConstructor]
    internal EffectsLocalizedRepository()
    {
        Effects = [];
    }
}
