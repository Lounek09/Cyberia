﻿using Cyberia.Api.Enums;
using Cyberia.Api.Extensions;

namespace Cyberia.Api.Factories.Criteria;

public sealed record CardCombinationCriterion : Criterion
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

    protected override string GetDescriptionKey()
    {
        return $"Criterion.CardCombination.{GetOperatorDescriptionKey()}";
    }

    public override DescriptionString GetDescription()
    {
        return GetDescription(CardCombination.GetDescription());
    }
}
