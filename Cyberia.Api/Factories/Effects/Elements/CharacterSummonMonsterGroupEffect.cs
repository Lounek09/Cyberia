using Cyberia.Api.Data.Monsters;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterSummonMonsterGroupEffect : Effect
{
    public IReadOnlyList<int> MonstersId { get; init; }

    private CharacterSummonMonsterGroupEffect(int id, List<int> monstersId)
        : base(id)
    {
        MonstersId = monstersId;
    }

    internal static CharacterSummonMonsterGroupEffect Create(int effectId, EffectParameters parameters)
    {
        //TODO: Use Span
        var monstersId = parameters.Param4.Split(':', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => int.Parse(x, NumberStyles.HexNumber))
            .ToList();

        return new(effectId, monstersId);
    }

    public IEnumerable<MonsterData> GetMonstersData()
    {
        foreach (var monsterId in MonstersId)
        {
            var monster = DofusApi.Datacenter.MonstersRepository.GetMonsterDataById(monsterId);
            if (monster is not null)
            {
                yield return monster;
            }
        }
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var monstersName = string.Join(", ", MonstersId.Select(x => DofusApi.Datacenter.MonstersRepository.GetMonsterNameById(x, culture)));

        return GetDescription(culture, string.Empty, string.Empty, monstersName);
    }
}
