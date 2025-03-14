﻿using Cyberia.Api.Data.Monsters;

using System.Globalization;

namespace Cyberia.Api.Factories.Criteria.Elements.Fights;

public sealed record MonsterSummonCriterion : Criterion
{
    public int MonsterId { get; init; }

    public MonsterSummonCriterion(string id, char @operator, int monsterId)
        : base(id, @operator)
    {
        MonsterId = monsterId;
    }

    internal static MonsterSummonCriterion? Create(string id, char @operator, params ReadOnlySpan<string> parameters)
    {
        if (parameters.Length > 0 && int.TryParse(parameters[0], out var monsterId))
        {
            return new(id, @operator, monsterId);
        }

        return null;
    }

    public MonsterData? GetMonsterData()
    {
        return DofusApi.Datacenter.MonstersRepository.GetMonsterDataById(MonsterId);
    }

    protected override string GetDescriptionKey()
    {
        return $"Criterion.MonsterSummon.{GetOperatorDescriptionKey()}";
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var monsterName = DofusApi.Datacenter.MonstersRepository.GetMonsterNameById(MonsterId, culture);

        return GetDescription(culture, monsterName);
    }
}
