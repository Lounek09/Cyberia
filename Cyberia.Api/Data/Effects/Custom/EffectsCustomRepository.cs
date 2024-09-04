using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Effects.Custom;

public sealed class EffectsCustomRepository : DofusCustomRepository, IDofusRepository
{
    public static string FileName => EffectsRepository.FileName;

    [JsonPropertyName("CE")]
    public IReadOnlyList<EffectCustomData> Effects { get; init; }

    [JsonConstructor]
    internal EffectsCustomRepository()
    {
        Effects = [];
    }
}
