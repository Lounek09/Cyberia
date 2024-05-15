using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Effects.Custom;

public sealed class EffectsCustomRepository : IDofusRepository
{
    [JsonPropertyName("CE")]
    public IReadOnlyList<EffectCustomData> Effects { get; init; }

    [JsonConstructor]
    internal EffectsCustomRepository()
    {
        Effects = [];
    }
}
