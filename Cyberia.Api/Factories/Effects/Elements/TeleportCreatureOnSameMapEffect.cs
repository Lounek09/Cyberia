using Cyberia.Api.Data.Monsters;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Interfaces;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record TeleportCreatureOnSameMapEffect : Effect, IMonsterEffect
{
    public int MonsterId { get; init; }
    public int MaximumDistance { get; init; }

    private TeleportCreatureOnSameMapEffect(int id, int monsterId, int maximumDistance)
        : base(id)
    {
        MonsterId = monsterId;
        MaximumDistance = maximumDistance;
    }

    internal static TeleportCreatureOnSameMapEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param3, (int)parameters.Param1);
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
