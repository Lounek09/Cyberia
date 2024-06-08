using Cyberia.Api.Data.Quests;

namespace Cyberia.Api.Factories.QuestObjectives;

public sealed record ErroredQuestObjective : QuestObjective, IQuestObjective
{
    private ErroredQuestObjective(QuestObjectiveData questObjectiveData)
        : base(questObjectiveData)
    {

    }

    internal static ErroredQuestObjective Create(QuestObjectiveData questObjectiveData)
    {
        return new(questObjectiveData);
    }

    public Description GetDescription()
    {
        return new Description(ApiTranslations.QuestObjectiveType_Errored,
            QuestObjectiveData.QuestObjectiveTypeId.ToString(),
            string.Join(", ", QuestObjectiveData.Parameters));
    }
}
