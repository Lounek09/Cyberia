using Cyberia.Api.DatacenterNS;

namespace Cyberia.Api.Factories.QuestObjectives
{
    public sealed record FreeFormQuestObjective(QuestObjective QuestObjective, string Description) :
        BasicQuestObjective(QuestObjective)
    {
        public static new FreeFormQuestObjective? Create(QuestObjective questObjective)
        {
            if (questObjective.Parameters.Count > 0)
                return new(questObjective, questObjective.Parameters[0]);

            return null;
        }

        public override string GetDescription()
        {
            return GetDescriptionFromParameters(Description);
        }
    }
}