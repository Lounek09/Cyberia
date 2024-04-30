using Cyberia.Api.Data.Quests;

namespace Cyberia.Api.Factories.Criteria;

public sealed record QuestFinishedCountCriterion : Criterion, ICriterion
{
    public int QuestId { get; init; }
    public int Count { get; init; }

    private QuestFinishedCountCriterion(string id, char @operator, int questId, int count)
        : base(id, @operator)
    {
        QuestId = questId;
        Count = count;
    }

    internal static QuestFinishedCountCriterion? Create(string id, char @operator, params string[] parameters)
    {
        if (parameters.Length > 1 && int.TryParse(parameters[0], out var questId) && int.TryParse(parameters[1], out var count))
        {
            return new(id, @operator, questId, count);
        }

        return null;
    }

    public QuestData? GetQuestData()
    {
        return DofusApi.Datacenter.QuestsData.GetQuestDataById(QuestId);
    }

    protected override string GetDescriptionName()
    {
        if (Count == 0)
        {
            var operatorDescriptionName = Operator switch
            {
                '>' => CriterionFactory.GetCriterionOperatorDescriptionName('='),
                '<' => CriterionFactory.GetCriterionOperatorDescriptionName('!'),
                _ => GetOperatorDescriptionName()
            };

            return $"Criterion.QuestFinished.{operatorDescriptionName}";
        }

        return $"Criterion.QuestFinishedCount.{GetOperatorDescriptionName()}";
    }

    public Description GetDescription()
    {
        var questName = DofusApi.Datacenter.QuestsData.GetQuestNameById(QuestId);

        return GetDescription(questName, Count);
    }
}
