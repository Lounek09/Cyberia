using Cyberia.Api.Data.Runes;
using Cyberia.Api.Enums;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Interfaces;
using Cyberia.Api.Factories.Effects.Templates;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record HuntToolEffect : ParameterlessEffect, IRuneGeneratorEffect
{
    public Rune Rune => Rune.Hunt;

    private HuntToolEffect(int id)
        : base(id) { }

    public int GetRandomValue()
    {
        return 1;
    }

    internal static HuntToolEffect Create(int effectId, EffectParameters _)
    {
        return new(effectId);
    }

    public RuneData? GetRuneData()
    {
        return DofusApi.Datacenter.RunesRepository.GetRuneDataById((int)Rune);
    }
}
