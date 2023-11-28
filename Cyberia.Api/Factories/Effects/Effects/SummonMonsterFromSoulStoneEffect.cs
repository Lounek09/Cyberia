using Cyberia.Api.Data.Monsters;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects;

public sealed record SummonMonsterFromSoulStoneEffect : Effect, IEffect<SummonMonsterFromSoulStoneEffect>
{
    public List<int> MonstersId { get; init; }

    private SummonMonsterFromSoulStoneEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, List<int> monstersId)
        : base(effectId, duration, probability, criteria, effectArea)
    {
        MonstersId = monstersId;
    }

    public static SummonMonsterFromSoulStoneEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        var monstersId = parameters.Param4.Split(":").Select(x => int.Parse(x, NumberStyles.HexNumber)).ToList();

        return new(effectId, duration, probability, criteria, effectArea, monstersId);
    }

    public IEnumerable<MonsterData> GetMonstersData()
    {
        foreach (var monsterId in MonstersId)
        {
            var monster = DofusApi.Datacenter.MonstersData.GetMonsterDataById(monsterId);
            if (monster is not null)
            {
                yield return monster;
            }
        }
    }

    public Description GetDescription()
    {
        var monstersName = string.Join(", ", MonstersId.Select(x => DofusApi.Datacenter.MonstersData.GetMonsterNameById(x)));

        return GetDescription(monstersName);
    }
}
