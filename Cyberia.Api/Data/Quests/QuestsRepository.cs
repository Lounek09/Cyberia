using Cyberia.Api.Data.Quests.Custom;
using Cyberia.Api.Factories;
using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Quests;

public sealed class QuestsRepository : DofusRepository, IDofusRepository
{
    public static string FileName => "quests.json";

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
                return ExtendString.NormalizeToAscii(x.Name).Contains(y, StringComparison.OrdinalIgnoreCase);
            });
        });
    }

    public string GetQuestNameById(int id)
    {
        var questData = GetQuestDataById(id);

        return questData is null
            ? Translation.Format(ApiTranslations.Unknown_Data, id)
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
            ? Translation.Format(ApiTranslations.Unknown_Data, id)
            : questStepData.Name;
    }

    public QuestObjectiveData? GetQuestObjectiveDataById(int id)
    {
        QuestObjectives.TryGetValue(id, out var questObjectiveData);
        return questObjectiveData;
    }

    public DescriptionString GetQuestObjectiveDescriptionById(int id)
    {
        var questObjectiveData = GetQuestObjectiveDataById(id);

        return questObjectiveData is null
            ? new DescriptionString(ApiTranslations.Unknown_Data, id.ToString())
            : QuestObjectiveFactory.Create(questObjectiveData).GetDescription();
    }

    public QuestObjectiveTypeData? GetQuestObjectiveTypeDataById(int id)
    {
        QuestObjectiveTypes.TryGetValue(id, out var questObjectiveTypeData);
        return questObjectiveTypeData;
    }

    protected override void LoadCustomData()
    {
        var customRepository = DofusCustomRepository.Load<QuestsCustomRepository>();

        foreach (var questCustomData in customRepository.QuestsCustom)
        {
            var questData = GetQuestDataById(questCustomData.Id);
            if (questData is not null)
            {
                questData.Repeatable = questCustomData.Repeatable;
                questData.Account = questCustomData.Account;
                questData.HasDungeon = questCustomData.HasDungeon;
                questData.QuestStepsId = questCustomData.QuestStepsId;
            }
        }

        foreach (var questStepCustomData in customRepository.QuestStepsCustom)
        {
            var questStepData = GetQuestStepDataById(questStepCustomData.Id);
            if (questStepData is not null)
            {
                questStepData.DialogQuestionId = questStepCustomData.DialogQuestionId;
                questStepData.OptimalLevel = questStepCustomData.OptimalLevel;
                questStepData.QuestObjectivesId = questStepCustomData.QuestObjectivesId;
            }
        }

        foreach (var questStepData in QuestSteps.Values)
        {
            List<QuestObjectiveData> questObjectivesData = [];

            foreach (var questObjectiveId in questStepData.QuestObjectivesId)
            {
                var questObjectiveData = GetQuestObjectiveDataById(questObjectiveId);
                if (questObjectiveData is not null)
                {
                    questObjectivesData.Add(questObjectiveData);
                }
            }

            questStepData.QuestObjectives = QuestObjectiveFactory.CreateMany(questObjectivesData);
        }
    }
}
