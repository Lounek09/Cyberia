using Cyberia.Api.Data.Monsters;
using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record LadderRaceEffect : Effect
{
    public int MonsterRaceId { get; init; }
    public int Count { get; init; }


    private LadderRaceEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea, int monsterRaceId, int count)
        : base(id, duration, probability, criteria, dispellable, effectArea)
    {
        MonsterRaceId = monsterRaceId;
        Count = count;
    }

    internal static LadderRaceEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, dispellable, effectArea, (int)parameters.Param1, (int)parameters.Param3);
    }

    public MonsterRaceData? GetMonsterRaceData()
    {
        return DofusApi.Datacenter.MonstersRepository.GetMonsterRaceDataById(MonsterRaceId);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var monsterRaceName = DofusApi.Datacenter.MonstersRepository.GetMonsterRaceNameById(MonsterRaceId, culture);

        return GetDescription(culture, monsterRaceName, string.Empty, Count);
    }
}
