using Cyberia.Api.Data.Monsters;
using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record LadderSuperRaceEffect : Effect
{
    public int MonsterSuperRaceId { get; init; }
    public int Count { get; init; }

    private LadderSuperRaceEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea, int monsterSuperRaceId, int count)
        : base(id, duration, probability, criteria, dispellable, effectArea)
    {
        MonsterSuperRaceId = monsterSuperRaceId;
        Count = count;
    }

    internal static LadderSuperRaceEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, dispellable, effectArea, (int)parameters.Param1, (int)parameters.Param3);
    }

    public MonsterSuperRaceData? GetMonsterSuperRaceData()
    {
        return DofusApi.Datacenter.MonstersRepository.GetMonsterSuperRaceDataById(MonsterSuperRaceId);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var monsterSuperRace = DofusApi.Datacenter.MonstersRepository.GetMonsterSuperRaceNameById(MonsterSuperRaceId, culture);

        return GetDescription(culture, monsterSuperRace, string.Empty, Count);
    }
}
