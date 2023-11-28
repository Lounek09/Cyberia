using Cyberia.Api.Data.Effects;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public interface IEffect
{
    int EffectId { get; init; }
    int Duration { get; init; }
    int Probability { get; init; }
    CriteriaCollection Criteria { get; init; }
    EffectArea EffectArea { get; init; }

    EffectData? GetEffectData();

    Description GetDescription();
}

public interface IEffect<T> : IEffect
{
    static abstract T Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea);
}
