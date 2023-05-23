using Cyberia.Api.DatacenterNS;

namespace Cyberia.Api.Factories.QuestObjectives
{
    public sealed record DuelSpecificPlayerQuestObjective : BasicQuestObjective
    {
        public string SpecificPlayer { get; init; }

        public DuelSpecificPlayerQuestObjective(QuestObjective questObjective) :
            base(questObjective)
        {
            List<string> parameters = questObjective.Parameters;

            SpecificPlayer = parameters.Count > 0 ? parameters[0] : "";
        }

        public static new DuelSpecificPlayerQuestObjective Create(QuestObjective questObjective)
        {
            return new(questObjective);
        }

        public override string GetDescription()
        {
            return GetDescriptionFromParameters(SpecificPlayer);
        }
    }
}
