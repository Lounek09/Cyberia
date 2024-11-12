using Cyberia.Api.Data.Quests;

using System.Globalization;

namespace Cyberia.Api.Factories.Criteria.Elements.Quests;

public sealed record QuestObjectiveCriterion : Criterion
{
    public int QuestObjectiveId { get; init; }

    private QuestObjectiveCriterion(string id, char @operator, int questStepId)
        : base(id, @operator)
    {
        QuestObjectiveId = questStepId;
    }

    internal static QuestObjectiveCriterion? Create(string id, char @operator, params ReadOnlySpan<string> parameters)
    {
        if (parameters.Length > 0 && int.TryParse(parameters[0], out var questObjectiveId))
        {
            return new(id, @operator, questObjectiveId);
        }

        return null;
    }

    public QuestObjectiveData? GetQuestObjectiveData()
    {
        return DofusApi.Datacenter.QuestsRepository.GetQuestObjectiveDataById(QuestObjectiveId);
    }

    protected override string GetDescriptionKey()
    {
        return $"Criterion.QuestObjective.{GetOperatorDescriptionKey()}";
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var questObjectiveDescription = DofusApi.Datacenter.QuestsRepository.GetQuestObjectiveDescriptionById(QuestObjectiveId, culture);

        return GetDescription(culture, questObjectiveDescription);
    }
}
