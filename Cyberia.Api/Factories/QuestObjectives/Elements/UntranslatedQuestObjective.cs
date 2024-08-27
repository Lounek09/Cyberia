using Cyberia.Api.Data.Quests;

namespace Cyberia.Api.Factories.QuestObjectives;

public sealed record UntranslatedQuestObjective : QuestObjective
{
    internal UntranslatedQuestObjective(QuestObjectiveData questObjectiveData)
        : base(questObjectiveData)
    {

    }

    public override DescriptionString GetDescription()
    {
        return GetDescription([]);
    }
}
