using Cyberia.Api.Data.Monsters;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterSummonMonsterGroupDynamicEffect : Effect, IEffect<CharacterSummonMonsterGroupDynamicEffect>
{
    public int MonsterId { get; init; }
    public int Grade { get; init; }

    private CharacterSummonMonsterGroupDynamicEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int monsterId, int grade)
        : base(effectId, duration, probability, criteria, effectArea)
    {
        MonsterId = monsterId;
        Grade = grade;
    }

    public static CharacterSummonMonsterGroupDynamicEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param3, parameters.Param1);
    }

    public MonsterData? GetMonsterData()
    {
        return DofusApi.Datacenter.MonstersData.GetMonsterDataById(MonsterId);
    }

    public Description GetDescription()
    {
        var monsterName = DofusApi.Datacenter.MonstersData.GetMonsterNameById(MonsterId);

        return GetDescription(monsterName, Grade);
    }
}
