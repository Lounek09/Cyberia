using Cyberia.Api.Data.Quests;

using System.Globalization;

namespace Cyberia.Api.Factories.QuestObjectives.Elements;

public sealed record MakeHuntFightQuestObjective : QuestObjective
{
    private MakeHuntFightQuestObjective(QuestObjectiveData questObjectiveData)
        : base(questObjectiveData)
    {

    }

    internal static MakeHuntFightQuestObjective? Create(QuestObjectiveData questObjectiveData)
    {
        return new(questObjectiveData);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture);
    }
}
