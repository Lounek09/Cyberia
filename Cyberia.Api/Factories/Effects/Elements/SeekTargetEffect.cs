using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

namespace Cyberia.Api.Factories.Effects;

public sealed record SeekTargetEffect : Effect
{
    public string Name { get; init; }

    private SeekTargetEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, string name)
        : base(id, duration, probability, criteria, effectArea)
    {
        Name = name;
    }

    internal static SeekTargetEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param4);
    }

    public override Description GetDescription()
    {
        return GetDescription(string.Empty, string.Empty, string.Empty, Name);
    }
}
