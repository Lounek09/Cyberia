﻿namespace Cyberia.Api.Factories.Criteria;

public sealed record FreeWeightCriterion
    : Criterion, ICriterion
{
    public int Value { get; init; }

    private FreeWeightCriterion(string id, char @operator, int value)
        : base(id, @operator)
    {
        Value = value;
    }

    internal static FreeWeightCriterion? Create(string id, char @operator, params string[] parameters)
    {
        if (parameters.Length > 0 && int.TryParse(parameters[0], out var value))
        {
            return new(id, @operator, value);
        }

        return null;
    }

    protected override string GetDescriptionName()
    {
        return $"Criterion.FreeWeight.{GetOperatorDescriptionName()}";
    }

    public Description GetDescription()
    {
        return GetDescription(Value);
    }
}