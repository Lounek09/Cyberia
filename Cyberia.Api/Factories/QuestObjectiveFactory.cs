using Cyberia.Api.Data.Quests;
using Cyberia.Api.Factories.QuestObjectives;

using System.Collections.Frozen;

namespace Cyberia.Api.Factories;

public static class QuestObjectiveFactory
{
    private static readonly FrozenDictionary<int, Func<QuestObjectiveData, IQuestObjective?>> _factory =
        new Dictionary<int, Func<QuestObjectiveData, IQuestObjective?>>()
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
            { 10, BringItemToNpcQuestObjective.Create }, //Missing parameters in the langs
            { 11, DuelSpecificPlayerQuestObjective.Create },
            { 12, BringSoulToNpcQuestObjective.Create },
            { 13, FightMonsterQuestObjective.Create }
        }.ToFrozenDictionary();

    public static IQuestObjective Create(QuestObjectiveData questObjectiveData)
    {
        if (_factory.TryGetValue(questObjectiveData.QuestObjectiveTypeId, out var builder))
        {
            var questObjective = builder(questObjectiveData);
            if (questObjective is not null)
            {
                return questObjective;
            }

            Log.Error("Failed to create QuestObjective from {@QuestObjectiveData}", questObjectiveData);
            return ErroredQuestObjective.Create(questObjectiveData);
        }

        return UntranslatedQuestObjective.Create(questObjectiveData);
    }

    public static IEnumerable<IQuestObjective> CreateMany(IEnumerable<QuestObjectiveData> questObjectivesData)
    {
        foreach (var questObjective in questObjectivesData)
        {
            yield return Create(questObjective);
        }
    }
}
