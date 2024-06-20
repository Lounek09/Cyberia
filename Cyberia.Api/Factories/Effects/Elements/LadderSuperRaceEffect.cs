using Cyberia.Api.Data.Monsters;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

namespace Cyberia.Api.Factories.Effects;

public sealed record LadderSuperRaceEffect : Effect, IEffect
{
    public int MonsterSuperRaceId { get; init; }
    public int Count { get; init; }

    private LadderSuperRaceEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int monsterSuperRaceId, int count)
        : base(id, duration, probability, criteria, effectArea)
    {
        MonsterSuperRaceId = monsterSuperRaceId;
        Count = count;
    }

    internal static LadderSuperRaceEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param1, (int)parameters.Param3);
    }

    public MonsterSuperRaceData? GetMonsterSuperRaceData()
    {
        return DofusApi.Datacenter.MonstersRepository.GetMonsterSuperRaceDataById(MonsterSuperRaceId);
    }

    public Description GetDescription()
    {
        var monsterSuperRace = DofusApi.Datacenter.MonstersRepository.GetMonsterSuperRaceNameById(MonsterSuperRaceId);

        return GetDescription(monsterSuperRace, string.Empty, Count);
    }
}
