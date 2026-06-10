using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Templates;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterDeboostPermanentDamagePercentEffect : MinMaxEffect
{
    private CharacterDeboostPermanentDamagePercentEffect(int id, int min, int max)
        : base(id, min, max) { }

    internal static CharacterDeboostPermanentDamagePercentEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param1, (int)parameters.Param2);
    }
}
