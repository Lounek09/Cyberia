using Cyberia.Api.Data.Monsters;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Interfaces;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterSummonMonsterGroupDynamicEffect : Effect, IMonsterEffect
{
    public int MonsterId { get; init; }
    public int Grade { get; init; }

    private CharacterSummonMonsterGroupDynamicEffect(int id, int monsterId, int grade)
        : base(id)
    {
        MonsterId = monsterId;
        Grade = grade;
    }

    internal static CharacterSummonMonsterGroupDynamicEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param3, (int)parameters.Param1);
    }

    public MonsterData? GetMonsterData()
    {
        return DofusApi.Datacenter.MonstersRepository.GetMonsterDataById(MonsterId);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var monsterName = DofusApi.Datacenter.MonstersRepository.GetMonsterNameById(MonsterId, culture);

        return GetDescription(culture, Grade, string.Empty, monsterName);
    }
}
