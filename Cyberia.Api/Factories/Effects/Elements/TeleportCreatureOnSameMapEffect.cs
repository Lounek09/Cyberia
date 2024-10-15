using Cyberia.Api.Data.Monsters;
using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record TeleportCreatureOnSameMapEffect : Effect
{
    public int MonsterId { get; init; }
    public int MaximumDistance { get; init; }

    private TeleportCreatureOnSameMapEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int monsterId, int maximumDistance)
        : base(id, duration, probability, criteria, effectArea)
    {
        MonsterId = monsterId;
        MaximumDistance = maximumDistance;
    }

    internal static TeleportCreatureOnSameMapEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3, (int)parameters.Param1);
    }

    public MonsterData? GetMonsterData()
    {
        return DofusApi.Datacenter.MonstersRepository.GetMonsterDataById(MonsterId);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var monsterName = DofusApi.Datacenter.MonstersRepository.GetMonsterNameById(MonsterId, culture);

        return GetDescription(culture, MaximumDistance, string.Empty, monsterName);
    }
}
