using Cyberia.Api.Data.Monsters;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record LadderRaceEffect : Effect, IEffect<LadderRaceEffect>
{
    public int MonsterRaceId { get; init; }
    public int Count { get; init; }


    private LadderRaceEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int monsterRaceId, int count)
        : base(id, duration, probability, criteria, effectArea)
    {
        MonsterRaceId = monsterRaceId;
        Count = count;
    }

    public static LadderRaceEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param1, parameters.Param3);
    }

    public MonsterRaceData? GetMonsterRaceData()
    {
        return DofusApi.Datacenter.MonstersData.GetMonsterRaceDataById(MonsterRaceId);
    }

    public Description GetDescription()
    {
        var monsterRaceName = DofusApi.Datacenter.MonstersData.GetMonsterRaceNameById(MonsterRaceId);

        return GetDescription(monsterRaceName, null, Count);
    }
}
