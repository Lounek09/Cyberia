using Cyberia.Api.Data.Quests;

namespace Cyberia.Api.Factories.QuestObjectives;

public sealed record ErroredQuestObjective : QuestObjective
{
    internal ErroredQuestObjective(QuestObjectiveData questObjectiveData)
        : base(questObjectiveData)
    {

    }

    public override DescriptionString GetDescription()
    {
        return new DescriptionString(ApiTranslations.QuestObjectiveType_Errored,
            QuestObjectiveData.QuestObjectiveTypeId.ToString(),
            string.Join(", ", QuestObjectiveData.Parameters));
    }
}
