using Cyberia.Api.Data.Quests;

namespace Cyberia.Api.Factories.Criteria;

public sealed record QuestCriterion : Criterion
{
    public int QuestId { get; init; }

    private QuestCriterion(string id, char @operator, int questId)
        : base(id, @operator)
    {
        QuestId = questId;
    }

    internal static QuestCriterion? Create(string id, char @operator, params string[] parameters)
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
        return $"Criterion.Quest.{GetOperatorDescriptionKey()}";
    }

    public override Description GetDescription()
    {
        var questName = DofusApi.Datacenter.QuestsRepository.GetQuestNameById(QuestId);

        return GetDescription(questName);
    }
}
