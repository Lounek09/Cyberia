using Cyberia.Api.Data.Quests.Custom;
using Cyberia.Api.Data.Quests.Localized;
using Cyberia.Api.Factories;
using Cyberia.Api.Factories.QuestObjectives;
using Cyberia.Api.JsonConverters;
using Cyberia.Langzilla.Enums;

using System.Collections.Frozen;
using System.Globalization;
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

    public QuestData? GetQuestDataByQuestStepId(int questStepId, out int questStepIndex)
    {
        foreach (var quest in Quests.Values)
        {
            var questStepsId = quest.QuestStepsId;

            for (var i = 0; i < questStepsId.Count; i++)
            {
                if (questStepsId[i] == questStepId)
                {
                    questStepIndex = i;
                    return quest;
                }
            }
        }

        questStepIndex = -1;
        return null;
    }

    public IEnumerable<QuestData> GetQuestDataByName(string name, Language language)
    {
        return GetQuestsDataByName(name, language.ToCulture());
    }

    public IEnumerable<QuestData> GetQuestsDataByName(string name, CultureInfo? culture = null)
    {
        var names = name.NormalizeToAscii().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        return Quests.Values.Where(x =>
        {
            return names.All(y =>
            {
                return x.Name.ToString(culture).NormalizeToAscii().Contains(y, StringComparison.OrdinalIgnoreCase);
            });
        });
    }

    public string GetQuestNameById(int id, Language language)
    {
        return GetQuestNameById(id, language.ToCulture());
    }

    public string GetQuestNameById(int id, CultureInfo? culture = null)
    {
        var questData = GetQuestDataById(id);

        return questData is null
            ? Translation.UnknownData(id, culture)
            : questData.Name.ToString(culture);
    }

    public QuestStepData? GetQuestStepDataById(int id)
    {
        QuestSteps.TryGetValue(id, out var questStepData);
        return questStepData;
    }

    public string GetQuestStepNameById(int id, Language language)
    {
        return GetQuestStepNameById(id, language.ToCulture());
    }

    public string GetQuestStepNameById(int id, CultureInfo? culture = null)
    {
        var questStepData = GetQuestStepDataById(id);

        return questStepData is null
            ? Translation.UnknownData(id, culture)
            : questStepData.Name.ToString(culture);
    }

    public QuestObjectiveData? GetQuestObjectiveDataById(int id)
    {
        QuestObjectives.TryGetValue(id, out var questObjectiveData);
        return questObjectiveData;
    }

    public DescriptionString GetQuestObjectiveDescriptionById(int id, Language language)
    {
        return GetQuestObjectiveDescriptionById(id, language.ToCulture());
    }

    public DescriptionString GetQuestObjectiveDescriptionById(int id, CultureInfo? culture = null)
    {
        var questObjectiveData = GetQuestObjectiveDataById(id);

        return questObjectiveData is null
            ? new DescriptionString(Translation.UnknownData(id, culture))
            : QuestObjectiveFactory.Create(questObjectiveData).GetDescription(culture);
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
    }

    protected override void LoadLocalizedData(LangType type, Language language)
    {
        var twoLetterISOLanguageName = language.ToStringFast();
        var localizedRepository = DofusLocalizedRepository.Load<QuestsLocalizedRepository>(type, language);

        foreach (var questLocalizedData in localizedRepository.Quests)
        {
            var questData = GetQuestDataById(questLocalizedData.Id);
            questData?.Name.TryAdd(twoLetterISOLanguageName, questLocalizedData.Name);
        }

        foreach (var questStepLocalizedData in localizedRepository.QuestSteps)
        {
            var questStepData = GetQuestStepDataById(questStepLocalizedData.Id);
            if (questStepData is not null)
            {
                questStepData.Name.TryAdd(twoLetterISOLanguageName, questStepLocalizedData.Name);
                questStepData.Description.TryAdd(twoLetterISOLanguageName, questStepLocalizedData.Description);
            }
        }

        foreach (var questObjectiveLocalizedData in localizedRepository.QuestObjectives)
        {
            var questObjectiveData = GetQuestObjectiveDataById(questObjectiveLocalizedData.Id);
            if (questObjectiveData is not null && questObjectiveLocalizedData.Parameters.Count == questObjectiveData.Parameters.Count)
            {
                for (var i = 0; i < questObjectiveData.Parameters.Count; i++)
                {
                    questObjectiveData.Parameters[i].TryAdd(twoLetterISOLanguageName, questObjectiveLocalizedData.Parameters[i]);
                }
            }
        }

        foreach (var questObjectiveTypeLocalizedData in localizedRepository.QuestObjectiveTypes)
        {
            var questObjectiveTypeData = GetQuestObjectiveTypeDataById(questObjectiveTypeLocalizedData.Id);
            questObjectiveTypeData?.Description.TryAdd(twoLetterISOLanguageName, questObjectiveTypeLocalizedData.Description);
        }
    }

    protected override void FinalizeLoading()
    {
        foreach (var questStepData in QuestSteps.Values)
        {
            List<IQuestObjective> questObjectives = [];

            foreach (var questObjectiveId in questStepData.QuestObjectivesId)
            {
                var questObjectiveData = GetQuestObjectiveDataById(questObjectiveId);
                if (questObjectiveData is not null)
                {
                    var questObjective = QuestObjectiveFactory.Create(questObjectiveData);
                    questObjectives.Add(questObjective);
                }
            }

            questStepData.QuestObjectives = questObjectives.AsReadOnly();
        }
    }
}
