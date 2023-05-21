using Cyberia.Api.DatacenterNS;

namespace Cyberia.Api.Factories.QuestObjectives
{
    public sealed record DiscoverMapQuestObjective(QuestObjective QuestObjective, string MapDescription) :
        BasicQuestObjective(QuestObjective)
    {
        public static new DiscoverMapQuestObjective? Create(QuestObjective questObjective)
        {
            if (questObjective.Parameters.Count > 0)
                return new(questObjective, questObjective.Parameters[0]);

            return null;
        }

        public override string GetDescription()
        {
            return GetDescriptionFromParameters(MapDescription);
        }
    }
}
