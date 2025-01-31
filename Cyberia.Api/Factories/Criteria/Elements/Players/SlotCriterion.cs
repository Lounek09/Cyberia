﻿using System.Globalization;

namespace Cyberia.Api.Factories.Criteria.Elements.Players;

public sealed record SlotCriterion : Criterion
{
    public int SlotId { get; init; }

    private SlotCriterion(string id, char @operator, int slotId)
        : base(id, @operator)
    {
        SlotId = slotId;
    }

    internal static SlotCriterion? Create(string id, char @operator, params ReadOnlySpan<string> parameters)
    {
        if (parameters.Length > 0 && int.TryParse(parameters[0], out var slotId))
        {
            return new(id, @operator, slotId);
        }

        return null;
    }

    protected override string GetDescriptionKey()
    {
        return $"Criterion.Slot.{GetOperatorDescriptionKey()}";
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, SlotId);
    }
}
