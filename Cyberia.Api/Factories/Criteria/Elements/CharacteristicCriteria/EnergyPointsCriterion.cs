﻿namespace Cyberia.Api.Factories.Criteria;

public sealed record EnergyPointsCriterion
    : Criterion, ICriterion
{
    public int Value { get; init; }

    private EnergyPointsCriterion(string id, char @operator, int value)
        : base(id, @operator)
    {
        Value = value;
    }

    internal static EnergyPointsCriterion? Create(string id, char @operator, params string[] parameters)
    {
        if (parameters.Length > 0 && int.TryParse(parameters[0], out var value))
        {
            return new(id, @operator, value);
        }

        return null;
    }

    protected override string GetDescriptionName()
    {
        return $"Criterion.EnergyPoints.{GetOperatorDescriptionName()}";
    }

    public Description GetDescription()
    {
        return GetDescription(Value);
    }
}