using Cyberia.Api.Data.Quests;

namespace Cyberia.Api.Factories.QuestObjectives;

public sealed record DiscoverMapQuestObjective : QuestObjective, IQuestObjective
{
    public string MapDescription { get; init; }

    private DiscoverMapQuestObjective(QuestObjectiveData questObjectiveData, string mapDescription)
        : base(questObjectiveData)
    {
        MapDescription = mapDescription;
    }

    internal static DiscoverMapQuestObjective? Create(QuestObjectiveData questObjectiveData)
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
        return GetDescription(MapDescription);
    }
}
