using Cyberia.Api.DatacenterNS;

namespace Cyberia.Api.Factories.QuestObjectives
{
    public sealed record DuelSpecificPlayerQuestObjective : QuestObjective, IQuestObjective<DuelSpecificPlayerQuestObjective>
    {
        public string SpecificPlayer { get; init; }

        private DuelSpecificPlayerQuestObjective(QuestObjectiveData questObjectiveData, string specificPlayer) :
            base(questObjectiveData)
        {
            SpecificPlayer = specificPlayer;
        }

        public static DuelSpecificPlayerQuestObjective? Create(QuestObjectiveData questObjectiveData)
        {
            List<string> parameters = questObjectiveData.Parameters;
            if (parameters.Count > 0)
                return new(questObjectiveData, parameters[0]);

            return null;
        }

        public Description GetDescription()
        {
            return GetDescription(SpecificPlayer);
        }
    }
}
