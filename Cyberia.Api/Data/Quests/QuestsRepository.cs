using Cyberia.Api.Data.Quests.Custom;
using Cyberia.Api.Factories;
using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Quests;

public sealed class QuestsRepository : IDofusRepository
{
    private const string c_fileName = "quests.json";

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
    internal QuestsRepository()
    {
        Quests = FrozenDictionary<int, QuestData>.Empty;
        QuestSteps = FrozenDictionary<int, QuestStepData>.Empty;
        QuestObjectives = FrozenDictionary<int, QuestObjectiveData>.Empty;
        QuestObjectiveTypes = FrozenDictionary<int, QuestObjectiveTypeData>.Empty;
    }

    internal static QuestsRepository Load(string directoryPath)
    {
        var filePath = Path.Join(directoryPath, c_fileName);
        var customFilePath = Path.Join(DofusApi.CustomPath, c_fileName);

        var data = Datacenter.LoadRepository<QuestsRepository>(filePath);
        var dataCustom = Datacenter.LoadRepository<QuestsCustomRepository>(customFilePath);

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
        var names = name.NormalizeToAscii().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        return Quests.Values.Where(x =>
        {
            return names.All(y =>
            {
                return x.Name.NormalizeToAscii().Contains(y, StringComparison.OrdinalIgnoreCase);
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
