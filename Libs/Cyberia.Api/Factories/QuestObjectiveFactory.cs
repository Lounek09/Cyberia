using Cyberia.Api.Data.Quests;
using Cyberia.Api.Factories.QuestObjectives;
using Cyberia.Api.Factories.QuestObjectives.Elements;

using System.Collections.Frozen;

namespace Cyberia.Api.Factories;

/// <summary>
/// Provides factory methods for creating <see cref="QuestObjective"/>.
/// </summary>
public static class QuestObjectiveFactory
{
    /// <summary>
    /// A dictionary mapping quest objective type identifiers to their factory methods.
    /// </summary>
    private static readonly FrozenDictionary<int, Func<QuestObjectiveData, QuestObjective?>> s_factories =
        new Dictionary<int, Func<QuestObjectiveData, QuestObjective?>>()
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
    /// Creates a new <see cref="QuestObjective"/> from the given <see cref="QuestObjectiveData"/>.
    /// </summary>
    /// <param name="data">The data of the quest objective to create.</param>
    /// <returns>The created <see cref="QuestObjective"/> if successful; otherwise, an <see cref="ErroredQuestObjective"/> or <see cref="UntranslatedQuestObjective"/> instance.</returns>
    public static QuestObjective Create(QuestObjectiveData data)
    {
        if (!s_factories.TryGetValue(data.QuestObjectiveTypeId, out var builder))
        {
            Log.Warning("Unknown QuestObjectiveType from {@QuestObjectiveData}", data);

            return new UntranslatedQuestObjective(data);
        }

        var questObjective = builder(data);
        if (questObjective is null)
        {
            Log.Error("Failed to create QuestObjective from {@QuestObjectiveData}", data);

            return new ErroredQuestObjective(data);
        }

        return questObjective;
    }
}
