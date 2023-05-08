using Cyberia.Api.DatacenterNS;

namespace Cyberia.Api.Factories.QuestObjectives
{
    public sealed class DuelSpecificPlayerQuestObjective : BasicQuestObjective
    {
        public string SpecificPlayer { get; init; }

        public DuelSpecificPlayerQuestObjective(QuestObjective questObjective, string specificPlayer) :
            base(questObjective)
        {
            SpecificPlayer = specificPlayer;
        }

        public static new DuelSpecificPlayerQuestObjective? Create(QuestObjective questObjective)
        {
            if (questObjective.Parameters.Count > 0)
                return new(questObjective, questObjective.Parameters[0]);

            return null;
        }

        public override string GetDescription()
        {
            return GetDescriptionFromParameters(SpecificPlayer);
        }
    }
}
