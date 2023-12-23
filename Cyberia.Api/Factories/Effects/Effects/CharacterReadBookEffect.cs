using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterReadBookEffect : Effect, IEffect<CharacterReadBookEffect>
{
    public int BookId { get; init; }

    private CharacterReadBookEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int bookId)
        : base(id, duration, probability, criteria, effectArea)
    {
        BookId = bookId;
    }

    public static CharacterReadBookEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(null, null, BookId);
    }
}
