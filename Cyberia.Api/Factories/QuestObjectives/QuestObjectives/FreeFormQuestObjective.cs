using Cyberia.Api.Data;

namespace Cyberia.Api.Factories.QuestObjectives;

public sealed record FreeFormQuestObjective : QuestObjective, IQuestObjective<FreeFormQuestObjective>
{
    public string Description { get; init; }

    private FreeFormQuestObjective(QuestObjectiveData questObjectiveData, string description)
        : base(questObjectiveData)
    {
        Description = description;
    }

    public static FreeFormQuestObjective? Create(QuestObjectiveData questObjectiveData)
    {
        var parameters = questObjectiveData.Parameters;
        if (parameters.Count > 0)
        {
            return new(questObjectiveData, parameters[0]);
        }

        return null;
    }

    public Description GetDescription()
    {
        return GetDescription(Description);
    }
}
