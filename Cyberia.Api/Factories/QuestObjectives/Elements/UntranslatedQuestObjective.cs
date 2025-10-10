using Cyberia.Api.Data.Quests;

using System.Globalization;

namespace Cyberia.Api.Factories.QuestObjectives.Elements;

public sealed record UntranslatedQuestObjective : QuestObjective
{
    internal UntranslatedQuestObjective(QuestObjectiveData questObjectiveData)
        : base(questObjectiveData) { }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, QuestObjectiveData.Parameters);
    }
}
