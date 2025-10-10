using Cyberia.Api.Data.Quests;

using System.Globalization;

namespace Cyberia.Api.Factories.QuestObjectives.Elements;

public sealed record ErroredQuestObjective : QuestObjective
{
    internal ErroredQuestObjective(QuestObjectiveData questObjectiveData)
        : base(questObjectiveData) { }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return new DescriptionString(Translation.Get<ApiTranslations>("QuestObjectiveType.Errored", culture),
            QuestObjectiveData.QuestObjectiveTypeId.ToString(),
            string.Join(", ", QuestObjectiveData.Parameters));
    }
}
