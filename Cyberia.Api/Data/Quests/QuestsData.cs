using Cyberia.Api.Data.Quests.Custom;
using Cyberia.Api.Factories;
using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Quests;

public sealed class QuestsData
    : IDofusData
{
    private const string FILE_NAME = "quests.json";
    private static readonly string FILE_PATH = Path.Join(DofusApi.OUTPUT_PATH, FILE_NAME);
    private static readonly string CUSTOM_FILE_PATH = Path.Join(DofusApi.CUSTOM_PATH, FILE_NAME);

    [JsonPropertyName("Q.q")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, QuestData>))]
    public FrozenDictionary<int, QuestData> Quests { get; init; }

    [JsonPropertyName("Q.s")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, QuestStepData>))]
    public FrozenDictionary<int, QuestStepData> QuestSteps { get; init; }

    [JsonPropertyName("Q.o")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, QuestObjectiveData>))]
    public FrozenDictionary<int, QuestObjectiveData> QuestObjectives { get; init; }

    [JsonPropertyName("Q.t")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, QuestObjectiveTypeData>))]
    public FrozenDictionary<int, QuestObjectiveTypeData> QuestObjectiveTypes { get; init; }

    [JsonConstructor]
    internal QuestsData()
    {
        Quests = FrozenDictionary<int, QuestData>.Empty;
        QuestSteps = FrozenDictionary<int, QuestStepData>.Empty;
        QuestObjectives = FrozenDictionary<int, QuestObjectiveData>.Empty;
        QuestObjectiveTypes = FrozenDictionary<int, QuestObjectiveTypeData>.Empty;
    }

    internal static async Task<QuestsData> LoadAsync()
    {
        var data = await Datacenter.LoadDataAsync<QuestsData>(FILE_PATH);
        var dataCustom = await Datacenter.LoadDataAsync<QuestsCustomData>(CUSTOM_FILE_PATH);

        foreach (var questCustomData in dataCustom.QuestsCustom)
        {
            var questData = data.GetQuestDataById(questCustomData.Id);
            if (questData is not null)
            {
                questData.Repeatable = questCustomData.Repeatable;
                questData.Account = questCustomData.Account;
                questData.HasDungeon = questCustomData.HasDungeon;
                questData.QuestStepsId = questCustomData.QuestStepsId;
            }
        }

        foreach (var questStepCustomData in dataCustom.QuestStepsCustom)
        {
            var questStepData = data.GetQuestStepDataById(questStepCustomData.Id);
            if (questStepData is not null)
            {
                questStepData.DialogQuestionId = questStepCustomData.DialogQuestionId;
                questStepData.OptimalLevel = questStepCustomData.OptimalLevel;
                questStepData.QuestObjectivesId = questStepCustomData.QuestObjectivesId;
            }
        }

        foreach (var questStepData in data.QuestSteps.Values)
        {
            List<QuestObjectiveData> questObjectivesData = [];
            foreach (var questObjectiveId in questStepData.QuestObjectivesId)
            {
                var questObjectiveData = data.GetQuestObjectiveDataById(questObjectiveId);
                if (questObjectiveData is not null)
                {
                    questObjectivesData.Add(questObjectiveData);
                }
            }

            questStepData.QuestObjectives = QuestObjectiveFactory.CreateMany(questObjectivesData).ToList();
        }

        return data;
    }

    public QuestData? GetQuestDataById(int id)
    {
        Quests.TryGetValue(id, out var questData);
        return questData;
    }

    public IEnumerable<QuestData> GetQuestsDataByName(string name)
    {
        var names = name.NormalizeCustom().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        return Quests.Values.Where(x =>
        {
            return names.All(y =>
            {
                return x.Name.NormalizeCustom().Contains(y, StringComparison.OrdinalIgnoreCase);
            });
        });
    }

    public string GetQuestNameById(int id)
    {
        var questData = GetQuestDataById(id);

        return questData is null
            ? PatternDecoder.Description(Resources.Unknown_Data, id)
            : questData.Name;
    }

    public QuestStepData? GetQuestStepDataById(int id)
    {
        QuestSteps.TryGetValue(id, out var questStepData);
        return questStepData;
    }

    public string GetQuestStepNameById(int id)
    {
        var questStepData = GetQuestStepDataById(id);

        return questStepData is null
            ? PatternDecoder.Description(Resources.Unknown_Data, id)
            : questStepData.Name;
    }

    public QuestObjectiveData? GetQuestObjectiveDataById(int id)
    {
        QuestObjectives.TryGetValue(id, out var questObjectiveData);
        return questObjectiveData;
    }

    public Description GetQuestObjectiveDescriptionById(int id)
    {
        var questObjectiveData = GetQuestObjectiveDataById(id);

        return questObjectiveData is null
            ? new Description(Resources.Unknown_Data, id.ToString())
            : QuestObjectiveFactory.Create(questObjectiveData).GetDescription();
    }

    public QuestObjectiveTypeData? GetQuestObjectiveTypeDataById(int id)
    {
        QuestObjectiveTypes.TryGetValue(id, out var questObjectiveTypeData);
        return questObjectiveTypeData;
    }
}
