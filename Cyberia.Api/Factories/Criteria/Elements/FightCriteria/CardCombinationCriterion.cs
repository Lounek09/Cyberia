﻿using Cyberia.Api.Values;

namespace Cyberia.Api.Factories.Criteria;

public sealed record CardCombinationCriterion
    : Criterion, ICriterion
{
    public CardCombination CardCombination { get; init; }

    private CardCombinationCriterion(string id, char @operator, CardCombination cardCombination)
        : base(id, @operator)
    {
        CardCombination = cardCombination;
    }

    internal static CardCombinationCriterion? Create(string id, char @operator, params string[] parameters)
    {
        if (parameters.Length > 0 && Enum.TryParse(parameters[0], out CardCombination cardCombination))
        {
            return new(id, @operator, cardCombination);
        }

        return null;
    }

    protected override string GetDescriptionName()
    {
        return $"Criterion.CardCombination.{GetOperatorDescriptionName()}";
    }

    public Description GetDescription()
    {
        return GetDescription(CardCombination.GetDescription());
    }
}