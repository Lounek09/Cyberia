﻿using Cyberia.Api.Data.Quests;

namespace Cyberia.Api.Factories.Criteria;

public sealed record QuestStepCriterion
    : Criterion, ICriterion
{
    public int QuestStepId { get; init; }

    private QuestStepCriterion(string id, char @operator, int questStepId)
        : base(id, @operator)
    {
        QuestStepId = questStepId;
    }

    internal static QuestStepCriterion? Create(string id, char @operator, params string[] parameters)
    {
        if (parameters.Length > 0 && int.TryParse(parameters[0], out var questStepId))
        {
            return new(id, @operator, questStepId);
        }

        return null;
    }

    public QuestStepData? GetQuestStepData()
    {
        return DofusApi.Datacenter.QuestsData.GetQuestStepDataById(QuestStepId);
    }

    protected override string GetDescriptionName()
    {
        return $"Criterion.QuestStep.{GetOperatorDescriptionName()}";
    }

    public Description GetDescription()
    {
        var questStepName = DofusApi.Datacenter.QuestsData.GetQuestStepNameById(QuestStepId);

        return GetDescription(questStepName);
    }
}