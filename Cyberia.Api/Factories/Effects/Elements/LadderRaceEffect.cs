using Cyberia.Api.Data.Monsters;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

namespace Cyberia.Api.Factories.Effects;

public sealed record LadderRaceEffect : Effect
{
    public int MonsterRaceId { get; init; }
    public int Count { get; init; }


    private LadderRaceEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int monsterRaceId, int count)
        : base(id, duration, probability, criteria, effectArea)
    {
        MonsterRaceId = monsterRaceId;
        Count = count;
    }

    internal static LadderRaceEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param1, (int)parameters.Param3);
    }

    public MonsterRaceData? GetMonsterRaceData()
    {
        return DofusApi.Datacenter.MonstersRepository.GetMonsterRaceDataById(MonsterRaceId);
    }

    public override DescriptionString GetDescription()
    {
        var monsterRaceName = DofusApi.Datacenter.MonstersRepository.GetMonsterRaceNameById(MonsterRaceId);

        return GetDescription(monsterRaceName, string.Empty, Count);
    }
}
