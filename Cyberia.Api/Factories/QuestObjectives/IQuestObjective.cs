using Cyberia.Api.DatacenterNS;

namespace Cyberia.Api.Factories.QuestObjectives
{
    public interface IQuestObjective
    {
        QuestObjective QuestObjectiveData { get; init; }

        string GetDescription();
    }
}
