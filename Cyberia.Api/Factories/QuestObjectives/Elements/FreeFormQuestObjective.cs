using Cyberia.Api.Data.Quests;

namespace Cyberia.Api.Factories.QuestObjectives;

public sealed record FreeFormQuestObjective : QuestObjective
{
    public string Description { get; init; }

    private FreeFormQuestObjective(QuestObjectiveData questObjectiveData, string description)
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

    public override DescriptionString GetDescription()
    {
        return GetDescription(Description);
    }
}
