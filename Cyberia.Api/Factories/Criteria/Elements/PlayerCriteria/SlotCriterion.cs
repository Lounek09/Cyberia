﻿namespace Cyberia.Api.Factories.Criteria;

public sealed record SlotCriterion
    : Criterion, ICriterion
{
    public int SlotId { get; init; }

    private SlotCriterion(string id, char @operator, int slotId)
        : base(id, @operator)
    {
        SlotId = slotId;
    }

    internal static SlotCriterion? Create(string id, char @operator, params string[] parameters)
    {
        if (parameters.Length > 0 && int.TryParse(parameters[0], out var slotId))
        {
            return new(id, @operator, slotId);
        }

        return null;
    }

    protected override string GetDescriptionName()
    {
        return $"Criterion.Slot.{GetOperatorDescriptionName()}";
    }

    public Description GetDescription()
    {
        return GetDescription(SlotId);
    }
}