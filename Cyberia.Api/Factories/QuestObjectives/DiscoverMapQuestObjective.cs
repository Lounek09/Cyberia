using Cyberia.Api.DatacenterNS;

namespace Cyberia.Api.Factories.QuestObjectives
{
    public sealed class DiscoverMapQuestObjective : BasicQuestObjective
    {
        public string MapDescription { get; init; }

        public DiscoverMapQuestObjective(QuestObjective questObjective, string mapDescription) :
            base(questObjective)
        {
            MapDescription = mapDescription;
        }

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
