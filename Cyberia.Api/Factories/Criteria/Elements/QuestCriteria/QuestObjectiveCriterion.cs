using Cyberia.Api.Data.Quests;

namespace Cyberia.Api.Factories.Criteria;

public sealed record QuestObjectiveCriterion : Criterion
{
    public int QuestObjectiveId { get; init; }

    private QuestObjectiveCriterion(string id, char @operator, int questStepId)
        : base(id, @operator)
    {
        QuestObjectiveId = questStepId;
    }

    internal static QuestObjectiveCriterion? Create(string id, char @operator, params string[] parameters)
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

    public override DescriptionString GetDescription()
    {
        var questObjectiveDescription = DofusApi.Datacenter.QuestsRepository.GetQuestObjectiveDescriptionById(QuestObjectiveId);

        return GetDescription(questObjectiveDescription);
    }
}
