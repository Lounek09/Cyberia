using Cyberia.Api.Factories.Effects.Templates;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record BoostCharacterXpBonusCoupleFightEffect : MinMaxEffect
{
    private BoostCharacterXpBonusCoupleFightEffect(int id, int min, int max)
        : base(id, min, max) { }

    internal static BoostCharacterXpBonusCoupleFightEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param1, (int)parameters.Param2);
    }
}
