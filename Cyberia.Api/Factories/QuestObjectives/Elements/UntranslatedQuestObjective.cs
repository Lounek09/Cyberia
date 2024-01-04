using Cyberia.Api.Data.Quests;

namespace Cyberia.Api.Factories.QuestObjectives;

public sealed record UntranslatedQuestObjective
    : QuestObjective, IQuestObjective
{
    private UntranslatedQuestObjective(QuestObjectiveData questObjectiveData)
        : base(questObjectiveData)
    {

    }

    internal static UntranslatedQuestObjective Create(QuestObjectiveData questObjectiveData)
    {
        return new(questObjectiveData);
    }

    public Description GetDescription()
    {
        return base.GetDescription();
    }
}
