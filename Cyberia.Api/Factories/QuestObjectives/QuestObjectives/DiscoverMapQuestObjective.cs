using Cyberia.Api.Data;

using System.Collections.ObjectModel;

namespace Cyberia.Api.Factories.QuestObjectives
{
    public sealed record DiscoverMapQuestObjective : QuestObjective, IQuestObjective<DiscoverMapQuestObjective>
    {
        public string MapDescription { get; init; }

        private DiscoverMapQuestObjective(QuestObjectiveData questObjectiveData, string mapDescription) :
            base(questObjectiveData)
        {
            MapDescription = mapDescription;
        }

        public static DiscoverMapQuestObjective? Create(QuestObjectiveData questObjectiveData)
        {
            ReadOnlyCollection<string> parameters = questObjectiveData.Parameters;
            if (parameters.Count > 0)
            {
                return new(questObjectiveData, parameters[0]);
            }

            return null;
        }

        public Description GetDescription()
        {
            return GetDescription(MapDescription);
        }
    }
}
