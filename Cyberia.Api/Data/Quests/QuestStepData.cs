using Cyberia.Api.Data.Dialogs;
using Cyberia.Api.Factories.QuestObjectives;

using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Quests;

public sealed class QuestStepData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public LocalizedString Name { get; init; }

    [JsonPropertyName("d")]
    public LocalizedString Description { get; init; }

    [JsonPropertyName("r")]
    public QuestStepRewardsData RewardsData { get; init; }

    [JsonPropertyName("rbl")]
    public IReadOnlyList<QuestStepRewardsBaseLevelData> RewardsBaseLevelsData { get; init; }

    [JsonIgnore]
    public int DialogQuestionId { get; internal set; }

    [JsonIgnore]
    public int OptimalLevel { get; internal set; }

    [JsonIgnore]
    public IReadOnlyList<int> QuestObjectivesId { get; internal set; }

    [JsonIgnore]
    public IReadOnlyList<IQuestObjective> QuestObjectives { get; internal set; }

    [JsonConstructor]
    internal QuestStepData()
    {
        Name = LocalizedString.Empty;
        Description = LocalizedString.Empty;
        RewardsData = new();
        RewardsBaseLevelsData = ReadOnlyCollection<QuestStepRewardsBaseLevelData>.Empty;
        QuestObjectivesId = ReadOnlyCollection<int>.Empty;
        QuestObjectives = ReadOnlyCollection<IQuestObjective>.Empty;
    }

    public bool HasRewards()
    {
        return RewardsData.Experience > 0 ||
            RewardsData.Kamas > 0 ||
            RewardsData.ItemsIdQuantities.Count > 0 ||
            RewardsData.EmotesId.Count > 0 ||
            RewardsData.JobsId.Count > 0 ||
            RewardsData.SpellsId.Count > 0;
    }

    public bool HasRewardsBaseLevel()
    {
        return RewardsBaseLevelsData.Count > 0;
    }

    public QuestStepRewardsBaseLevelData? GetRewardsBaseLevelDataByLevel(int level)
    {
        return RewardsBaseLevelsData.FirstOrDefault(x => level >= x.MinLevel || level <= x.MaxLevel);
    }

    public DialogQuestionData? GetDialogQuestionData()
    {
        return DofusApi.Datacenter.DialogsRepository.GetDialogQuestionDataById(DialogQuestionId);
    }
}
