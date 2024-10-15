using Cyberia.Api.Data.Quests;

using System.Globalization;

namespace Cyberia.Api.Factories.Criteria.Elements.Quests;

public sealed record QuestFinishedCountCriterion : Criterion
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
        return DofusApi.Datacenter.QuestsRepository.GetQuestDataById(QuestId);
    }

    protected override string GetDescriptionKey()
    {
        if (Count == 0)
        {
            var operatorDescriptionName = Operator switch
            {
                '>' => CriterionFactory.GetOperatorDescriptionKey('='),
                '<' => CriterionFactory.GetOperatorDescriptionKey('!'),
                _ => GetOperatorDescriptionKey()
            };

            return $"Criterion.QuestFinished.{operatorDescriptionName}";
        }

        return $"Criterion.QuestFinishedCount.{GetOperatorDescriptionKey()}";
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var questName = DofusApi.Datacenter.QuestsRepository.GetQuestNameById(QuestId, culture);

        return GetDescription(culture, questName, Count);
    }
}
