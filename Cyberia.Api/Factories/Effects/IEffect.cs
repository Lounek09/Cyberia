using Cyberia.Api.Data.Effects;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public interface IEffect
{
    int Id { get; init; }
    int Duration { get; init; }
    int Probability { get; init; }
    CriteriaCollection Criteria { get; init; }
    EffectArea EffectArea { get; init; }

    EffectData? GetEffectData();

    Description GetDescription();
}
