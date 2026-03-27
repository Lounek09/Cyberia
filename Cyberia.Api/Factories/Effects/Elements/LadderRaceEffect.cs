using Cyberia.Api.Data.Monsters;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Interfaces;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record LadderRaceEffect : Effect, IMonsterRaceEffect
{
    public int MonsterRaceId { get; init; }
    public int Count { get; init; }


    private LadderRaceEffect(int id, int monsterRaceId, int count)
        : base(id)
    {
        MonsterRaceId = monsterRaceId;
        Count = count;
    }

    internal static LadderRaceEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param1, (int)parameters.Param3);
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
