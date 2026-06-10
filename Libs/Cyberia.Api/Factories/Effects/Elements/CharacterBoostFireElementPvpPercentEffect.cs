using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Templates;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterBoostFireElementPvpPercentEffect : MinMaxEffect
{
    private CharacterBoostFireElementPvpPercentEffect(int id, int min, int max)
        : base(id, min, max) { }

    internal static CharacterBoostFireElementPvpPercentEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param1, (int)parameters.Param2);
    }
}
