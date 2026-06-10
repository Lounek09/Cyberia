using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Effects.Custom;

internal sealed class EffectsCustomRepository : DofusCustomRepository, IDofusRepository
{
    public static string FileName => EffectsRepository.FileName;

    [JsonPropertyName("CE")]
    public IReadOnlyList<EffectCustomData> Effects { get; init; }

    [JsonConstructor]
    internal EffectsCustomRepository()
    {
        Effects = ReadOnlyCollection<EffectCustomData>.Empty;
    }
}
