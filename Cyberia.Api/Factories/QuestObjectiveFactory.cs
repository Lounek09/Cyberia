using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Factories.QuestObjectives;

namespace Cyberia.Api.Factories
{
    public static class QuestObjectiveFactory
    {
        private static readonly Dictionary<int, Func<QuestObjective, IQuestObjective>> _factory = new()
        {
            { 0, FreeFormQuestObjective.Create },
            { 1, GoToNpcQuestObjective.Create },
            { 2, BringItemToNpcQuestObjective.Create },
            { 3, BringItemToNpcQuestObjective.Create },
            { 4, DiscoverMapQuestObjective.Create },
            { 5, DiscoverMapSubAreaQuestObjective.Create },
            { 6, MultiFightMonsterQuestObjective.Create },
            { 7, FightMonsterQuestObjective.Create },
            { 8, FreeFormQuestObjective.Create },
            { 9, GoToNpcQuestObjective.Create },
            { 10, BringItemToNpcQuestObjective.Create },
            { 11, DuelSpecificPlayerQuestObjective.Create },
            { 12, BringSoulToNpcQuestObjective.Create },
            { 13, FightMonsterQuestObjective.Create }
        };

        public static IQuestObjective GetQuestObjective(QuestObjective questObjective)
        {
            if (_factory.TryGetValue(questObjective.QuestObjectiveTypeId, out Func<QuestObjective, IQuestObjective>? builder))
                return builder(questObjective);

            return BasicQuestObjective.Create(questObjective);
        }

        public static IEnumerable<IQuestObjective> GetQuestObjectives(IEnumerable<QuestObjective> questObjectives)
        {
            foreach (QuestObjective questObjective in questObjectives)
                yield return GetQuestObjective(questObjective);
        }
    }
}
