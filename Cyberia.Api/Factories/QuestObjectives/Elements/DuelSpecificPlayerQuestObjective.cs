using Cyberia.Api.Data.Quests;

namespace Cyberia.Api.Factories.QuestObjectives;

public sealed record DuelSpecificPlayerQuestObjective : QuestObjective, IQuestObjective
{
    public string SpecificPlayer { get; init; }

    private DuelSpecificPlayerQuestObjective(QuestObjectiveData questObjectiveData, string specificPlayer)
        : base(questObjectiveData)
    {
        SpecificPlayer = specificPlayer;
    }

    internal static DuelSpecificPlayerQuestObjective? Create(QuestObjectiveData questObjectiveData)
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
        return GetDescription(SpecificPlayer);
    }
}
