using Cyberia.Api.DatacenterNS;

namespace Cyberia.Api.Factories.QuestObjectives
{
    public sealed record DiscoverMapQuestObjective : BasicQuestObjective
    {
        public string MapDescription { get; init; }

        public DiscoverMapQuestObjective(QuestObjective questObjective) :
            base(questObjective)
        {
            List<string> parameters = questObjective.Parameters;

            MapDescription = parameters.Count > 0 ? parameters[0] : "";
        }

        public static new DiscoverMapQuestObjective Create(QuestObjective questObjective)
        {
            return new(questObjective);
        }

        public override string GetDescription()
        {
            return GetDescriptionFromParameters(MapDescription);
        }
    }
}
