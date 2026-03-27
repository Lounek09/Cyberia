using Cyberia.Api.Factories.Effects.Templates;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterLifePointsWinFromBestElement : MinMaxEffect
{
    private CharacterLifePointsWinFromBestElement(int id, int min, int max)
        : base(id, min, max) { }

    internal static CharacterLifePointsWinFromBestElement Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param1, (int)parameters.Param2);
    }
}
