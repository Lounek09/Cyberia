using Cyberia.Api.Data.Runes;
using Cyberia.Api.Enums;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Interfaces;
using Cyberia.Api.Factories.Effects.Templates;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterBoostDamagesPercentEffect : MinMaxEffect, IRuneGeneratorEffect
{
    public Rune Rune => Rune.DoPer;

    private CharacterBoostDamagesPercentEffect(int id, int min, int max)
        : base(id, min, max) { }

    internal static CharacterBoostDamagesPercentEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param1, (int)parameters.Param2);
    }

    public RuneData? GetRuneData()
    {
        return DofusApi.Datacenter.RunesRepository.GetRuneDataById((int)Rune);
    }
}
