﻿using Cyberia.Api.Data.Quests;

namespace Cyberia.Api.Factories.Criteria;

public sealed record QuestFinishedCriterion : Criterion, ICriterion
{
    public int QuestId { get; init; }

    private QuestFinishedCriterion(string id, char @operator, int questId)
        : base(id, @operator)
    {
        QuestId = questId;
    }

    internal static QuestFinishedCriterion? Create(string id, char @operator, params string[] parameters)
    {
        if (parameters.Length > 0 && int.TryParse(parameters[0], out var questId))
        {
            return new(id, @operator, questId);
        }

        return null;
    }

    public QuestData? GetQuestData()
    {
        return DofusApi.Datacenter.QuestsData.GetQuestDataById(QuestId);
    }

    protected override string GetDescriptionName()
    {
        return $"Criterion.QuestFinished.{GetOperatorDescriptionName()}";
    }

    public Description GetDescription()
    {
        var questName = DofusApi.Datacenter.QuestsData.GetQuestNameById(QuestId);

        return GetDescription(questName);
    }
}