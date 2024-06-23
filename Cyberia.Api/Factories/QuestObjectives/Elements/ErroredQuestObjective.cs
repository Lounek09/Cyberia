using Cyberia.Api.Data.Quests;

namespace Cyberia.Api.Factories.QuestObjectives;

public sealed record ErroredQuestObjective : QuestObjective
{
    internal ErroredQuestObjective(QuestObjectiveData questObjectiveData)
        : base(questObjectiveData)
    {

    }

    public override Description GetDescription()
    {
        return new Description(ApiTranslations.QuestObjectiveType_Errored,
            QuestObjectiveData.QuestObjectiveTypeId.ToString(),
            string.Join(", ", QuestObjectiveData.Parameters));
    }
}
