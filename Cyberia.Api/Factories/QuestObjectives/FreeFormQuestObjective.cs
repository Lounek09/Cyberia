using Cyberia.Api.DatacenterNS;

namespace Cyberia.Api.Factories.QuestObjectives
{
    public sealed record FreeFormQuestObjective : BasicQuestObjective
    {
        public string Description { get; init; }

        public FreeFormQuestObjective(QuestObjective questObjective) :
            base(questObjective)
        {
            List<string> parameters = questObjective.Parameters;

            Description = parameters.Count > 0 ? parameters[0] : 0;
        }

        public static new FreeFormQuestObjective Create(QuestObjective questObjective)
        {
            return new(questObjective);
        }

        public override string GetDescription()
        {
            return GetDescriptionFromParameters(Description);
        }
    }
}