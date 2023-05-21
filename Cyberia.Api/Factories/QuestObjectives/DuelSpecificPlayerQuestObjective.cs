using Cyberia.Api.DatacenterNS;

namespace Cyberia.Api.Factories.QuestObjectives
{
    public sealed record DuelSpecificPlayerQuestObjective(QuestObjective QuestObjective, string SpecificPlayer) :
        BasicQuestObjective(QuestObjective)
    {
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
