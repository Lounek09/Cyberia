using Cyberia.Api.Data.Quests;

namespace Cyberia.Api.Factories.Criteria;

public sealed record QuestAvailableCriterion : Criterion
{
    public int QuestId { get; init; }

    private QuestAvailableCriterion(string id, char @operator, int questId)
        : base(id, @operator)
    {
        QuestId = questId;
    }

    internal static QuestAvailableCriterion? Create(string id, char @operator, params string[] parameters)
    {
        if (parameters.Length > 0 && int.TryParse(parameters[0], out var questId))
        {
            return new(id, @operator, questId);
        }

        return null;
    }

    public QuestData? GetQuestData()
    {
        return DofusApi.Datacenter.QuestsRepository.GetQuestDataById(QuestId);
    }

    protected override string GetDescriptionKey()
    {
        return $"Criterion.QuestAvailable.{GetOperatorDescriptionKey()}";
    }

    public override DescriptionString GetDescription()
    {
        var questName = DofusApi.Datacenter.QuestsRepository.GetQuestNameById(QuestId);

        return GetDescription(questName);
    }
}
