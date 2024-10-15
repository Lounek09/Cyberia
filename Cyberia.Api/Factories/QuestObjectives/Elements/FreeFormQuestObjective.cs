using Cyberia.Api.Data.Quests;

using System.Globalization;

namespace Cyberia.Api.Factories.QuestObjectives.Elements;

public sealed record FreeFormQuestObjective : QuestObjective
{
    public LocalizedString Description { get; init; }

    private FreeFormQuestObjective(QuestObjectiveData questObjectiveData, LocalizedString description)
        : base(questObjectiveData)
    {
        Description = description;
    }

    internal static FreeFormQuestObjective? Create(QuestObjectiveData questObjectiveData)
    {
        var parameters = questObjectiveData.Parameters;
        if (parameters.Count > 0)
        {
            return new(questObjectiveData, parameters[0]);
        }

        return null;
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, Description);
    }
}
