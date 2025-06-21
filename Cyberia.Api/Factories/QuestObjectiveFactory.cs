using Cyberia.Api.Data.Quests;
using Cyberia.Api.Factories.QuestObjectives;
using Cyberia.Api.Factories.QuestObjectives.Elements;

using System.Collections.Frozen;

namespace Cyberia.Api.Factories;

/// <summary>
/// Provides factory methods for creating <see cref="IQuestObjective"/>.
/// </summary>
public static class QuestObjectiveFactory
{
    /// <summary>
    /// A dictionary mapping quest objective type identifiers to their factory methods.
    /// </summary>
    private static readonly FrozenDictionary<int, Func<QuestObjectiveData, IQuestObjective?>> s_factories =
        new Dictionary<int, Func<QuestObjectiveData, IQuestObjective?>>()
        {
            { 0, FreeFormQuestObjective.Create },
            { 1, GoToNpcQuestObjective.Create },
            { 2, ShowItemToNpcQuestObjective.Create },
            { 3, BringItemToNpcQuestObjective.Create },
            { 4, DiscoverMapQuestObjective.Create },
            { 5, DiscoverMapSubAreaQuestObjective.Create },
            { 6, MultiFightMonsterQuestObjective.Create },
            { 7, FightMonsterQuestObjective.Create },
            { 8, UseItemQuestObjective.Create },
            { 9, GoToNpcQuestObjective.Create },
            { 10, EscortQuestObjective.Create }, // Missing parameters in the langs
            { 11, DuelSpecificPlayerQuestObjective.Create }, // Missing parameters in the langs
            { 12, BringSoulToNpcQuestObjective.Create },
            { 13, SeekHuntTargetQuestObjective.Create },
            { 14, MakeHuntFightQuestObjective.Create },
            { 15, CreateMagicFragmentQuestObjective.Create },
            { 16, MultiFightFamilyMonsterQuestObjective.Create },
            { 17, FinishDailyQuestQuestObjective.Create },
            { 18, BringItemToNpcInAreaQuestObjective.Create },
        }.ToFrozenDictionary();

    /// <summary>
    /// Creates a new <see cref="IQuestObjective"/> from the given <see cref="QuestObjectiveData"/>.
    /// </summary>
    /// <param name="questObjectiveData">The data of the quest objective to create.</param>
    /// <returns>The created <see cref="IQuestObjective"/> if successful; otherwise, an <see cref="ErroredQuestObjective"/> or <see cref="UntranslatedQuestObjective"/> instance.</returns>
    public static IQuestObjective Create(QuestObjectiveData questObjectiveData)
    {
        if (!s_factories.TryGetValue(questObjectiveData.QuestObjectiveTypeId, out var builder))
        {
            Log.Warning("Unknown QuestObjectiveType from {@QuestObjectiveData}", questObjectiveData);

            return new UntranslatedQuestObjective(questObjectiveData);
        }

        var questObjective = builder(questObjectiveData);
        if (questObjective is null)
        {
            Log.Error("Failed to create QuestObjective from {@QuestObjectiveData}", questObjectiveData);

            return new ErroredQuestObjective(questObjectiveData);
        }

        return questObjective;
    }
}
