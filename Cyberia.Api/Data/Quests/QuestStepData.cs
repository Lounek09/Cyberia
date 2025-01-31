﻿using Cyberia.Api.Data.Dialogs;
using Cyberia.Api.Factories.QuestObjectives;

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
        QuestObjectivesId = [];
        QuestObjectives = [];
    }

    public bool HasReward()
    {
        return RewardsData.Experience > 0 ||
            RewardsData.Kamas > 0 ||
            RewardsData.ItemsIdQuantities.Count > 0 ||
            RewardsData.EmotesId.Count > 0 ||
            RewardsData.JobsId.Count > 0 ||
            RewardsData.SpellsId.Count > 0;
    }

    public DialogQuestionData? GetDialogQuestionData()
    {
        return DofusApi.Datacenter.DialogsRepository.GetDialogQuestionDataById(DialogQuestionId);
    }
}
