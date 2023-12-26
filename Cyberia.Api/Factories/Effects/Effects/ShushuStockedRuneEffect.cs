using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record ShushuStockedRuneEffect : Effect, IEffect<ShushuStockedRuneEffect>
{
    public int Amont { get; init; }

    private ShushuStockedRuneEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int amont)
        : base(id, duration, probability, criteria, effectArea)
    {
        Amont = amont;
    }

    public static ShushuStockedRuneEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(Amont);
    }
}
