using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterChangeColorEffect : Effect, IEffect<CharacterChangeColorEffect>
{
    public int Position { get; init; }
    public int Color { get; init; }

    private CharacterChangeColorEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int position, int color)
        : base(id, duration, probability, criteria, effectArea)
    {
        Position = position;
        Color = color;
    }

    public static CharacterChangeColorEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param1, parameters.Param2);
    }

    public Description GetDescription()
    {
        return GetDescription(Position, Color);
    }
}
