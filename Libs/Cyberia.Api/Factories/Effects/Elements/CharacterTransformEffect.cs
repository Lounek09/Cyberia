using Cyberia.Api.Data.Monsters;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Interfaces;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterTransformEffect : Effect, IMonsterEffect
{
    public int MonsterId { get; init; }

    private CharacterTransformEffect(int id, int monsterId)
        : base(id)
    {
        MonsterId = monsterId;
    }

    internal static CharacterTransformEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param1);
    }

    public MonsterData? GetMonsterData()
    {
        return DofusApi.Datacenter.MonstersRepository.GetMonsterDataById(MonsterId);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var monsterName = DofusApi.Datacenter.MonstersRepository.GetMonsterNameById(MonsterId, culture);

        return GetDescription(culture, monsterName);
    }
}
