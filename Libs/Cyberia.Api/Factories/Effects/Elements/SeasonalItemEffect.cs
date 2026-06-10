using Cyberia.Api.Factories.Effects.Templates;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record SeasonalItemEffect : ParameterlessEffect
{
    private SeasonalItemEffect(int id)
        : base(id) { }

    internal static SeasonalItemEffect Create(int effectId, EffectParameters _)
    {
        return new(effectId);
    }
}
