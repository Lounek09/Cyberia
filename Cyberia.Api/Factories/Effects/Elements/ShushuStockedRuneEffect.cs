using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

namespace Cyberia.Api.Factories.Effects;

public sealed record ShushuStockedRuneEffect : Effect
{
    public int Amont { get; init; }

    private ShushuStockedRuneEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int amont)
        : base(id, duration, probability, criteria, effectArea)
    {
        Amont = amont;
    }

    internal static ShushuStockedRuneEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription()
    {
        return GetDescription(string.Empty, string.Empty, Amont);
    }
}
