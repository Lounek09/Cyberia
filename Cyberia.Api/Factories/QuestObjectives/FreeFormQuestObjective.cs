using Cyberia.Api.DatacenterNS;

namespace Cyberia.Api.Factories.QuestObjectives
{
    public sealed class FreeFormQuestObjective : BasicQuestObjective
    {
        public string Description { get; init; }

        public FreeFormQuestObjective(QuestObjective questObjective, string description) :
            base(questObjective)
        {
            Description = description;
        }

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