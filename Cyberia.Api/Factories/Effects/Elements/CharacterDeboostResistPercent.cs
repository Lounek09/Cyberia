using Cyberia.Api.Factories.Effects.Templates;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterDeboostResistPercent : MinMaxEffect
{
    private CharacterDeboostResistPercent(int id, int min, int max)
        : base(id, min, max) { }

    internal static CharacterDeboostResistPercent Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param1, (int)parameters.Param2);
    }
}
