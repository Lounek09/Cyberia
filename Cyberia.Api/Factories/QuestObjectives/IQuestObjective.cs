using Cyberia.Api.DatacenterNS;

namespace Cyberia.Api.Factories.QuestObjectives
{
    public interface IQuestObjective
    {
        public QuestObjective QuestObjective { get; init; }

        public string GetDescription();
    }
}
