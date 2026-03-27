using Cyberia.Api.Factories.Effects.Templates;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterBoostResistPercent : MinMaxEffect
{
    private CharacterBoostResistPercent(int id, int min, int max)
        : base(id, min, max) { }

    internal static CharacterBoostResistPercent Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param1, (int)parameters.Param2);
    }
}
