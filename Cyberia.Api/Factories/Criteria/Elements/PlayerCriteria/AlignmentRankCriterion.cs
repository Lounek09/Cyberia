﻿namespace Cyberia.Api.Factories.Criteria;

public sealed record AlignmentRankCriterion
    : Criterion, ICriterion
{
    public int Rank { get; init; }

    private AlignmentRankCriterion(string id, char @operator, int rank)
        : base(id, @operator)
    {
        Rank = rank;
    }

    internal static AlignmentRankCriterion? Create(string id, char @operator, params string[] parameters)
    {
        if (parameters.Length > 0 && int.TryParse(parameters[0], out var rank))
        {
            return new(id, @operator, rank);
        }

        return null;
    }

    protected override string GetDescriptionName()
    {
        return $"Criterion.AlignmentRank.{GetOperatorDescriptionName()}";
    }

    public Description GetDescription()
    {
        return GetDescription(Rank);
    }
}