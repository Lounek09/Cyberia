using Cyberia.Api.Data.Quests;

using System.Globalization;

namespace Cyberia.Api.Factories.QuestObjectives.Elements;

public sealed record SeekHuntTargetQuestObjective : QuestObjective
{
    private SeekHuntTargetQuestObjective(QuestObjectiveData questObjectiveData)
        : base(questObjectiveData)
    {

    }

    internal static SeekHuntTargetQuestObjective? Create(QuestObjectiveData questObjectiveData)
    {
        return new(questObjectiveData);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture);
    }
}
