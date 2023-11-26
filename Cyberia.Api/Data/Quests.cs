using Cyberia.Api.Data.Custom;
using Cyberia.Api.Factories;
using Cyberia.Api.Factories.QuestObjectives;
using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data
{
    public sealed class QuestData : IDofusData<int>
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("v")]
        public string Name { get; init; }

        [JsonIgnore]
        public bool Repeatable { get; internal set; }

        [JsonIgnore]
        public bool Account { get; internal set; }

        [JsonIgnore]
        public bool HasDungeon { get; internal set; }

        [JsonIgnore]
        public ReadOnlyCollection<int> QuestStepsId { get; internal set; }

        [JsonConstructor]
        internal QuestData()
        {
            Name = string.Empty;
            QuestStepsId = ReadOnlyCollection<int>.Empty;
        }

        public IEnumerable<QuestStepData> GetQuestStepsData()
        {
            foreach (int questStepId in QuestStepsId)
            {
                QuestStepData? questStepData = DofusApi.Datacenter.QuestsData.GetQuestStepDataById(questStepId);
                if (questStepData is not null)
                {
                    yield return questStepData;
                }
            }
        }
    }

    public sealed class QuestStepRewardsData : IDofusData
    {
        public int Experience { get; init; }

        public int Kamas { get; init; }

        public ReadOnlyDictionary<int, int> ItemsIdQuantities { get; init; }

        public ReadOnlyCollection<int> EmotesId { get; init; }

        public ReadOnlyCollection<int> JobsId { get; init; }

        public ReadOnlyCollection<int> SpellsId { get; init; }

        internal QuestStepRewardsData()
        {
            ItemsIdQuantities = ReadOnlyDictionary<int, int>.Empty;
            EmotesId = ReadOnlyCollection<int>.Empty;
            JobsId = ReadOnlyCollection<int>.Empty;
            SpellsId = ReadOnlyCollection<int>.Empty;
        }

        public IEnumerable<ItemData> GetItemsData()
        {
            foreach (KeyValuePair<int, int> pair in ItemsIdQuantities)
            {
                ItemData? itemData = DofusApi.Datacenter.ItemsData.GetItemDataById(pair.Key);
                if (itemData is not null)
                {
                    yield return itemData;
                }
            }
        }

        public ReadOnlyDictionary<ItemData, int> GetItemsDataQuantities()
        {
            Dictionary<ItemData, int> itemsDataQuantities = [];

            foreach (KeyValuePair<int, int> pair in ItemsIdQuantities)
            {
                ItemData? itemData = DofusApi.Datacenter.ItemsData.GetItemDataById(pair.Key);
                if (itemData is not null)
                {
                    itemsDataQuantities.Add(itemData, pair.Value);
                }
            }

            return itemsDataQuantities.AsReadOnly();
        }

        public IEnumerable<EmoteData> GetEmotesData()
        {
            foreach (int emoteId in EmotesId)
            {
                EmoteData? emoteData = DofusApi.Datacenter.EmotesData.GetEmoteById(emoteId);
                if (emoteData is not null)
                {
                    yield return emoteData;
                }
            }
        }

        public IEnumerable<JobData> GetJobsData()
        {
            foreach (int jobId in JobsId)
            {
                JobData? jobData = DofusApi.Datacenter.JobsData.GetJobDataById(jobId);
                if (jobData is not null)
                {
                    yield return jobData;
                }
            }
        }

        public IEnumerable<SpellData> GetSpellsData()
        {
            foreach (int spellId in SpellsId)
            {
                SpellData? spellData = DofusApi.Datacenter.SpellsData.GetSpellDataById(spellId);
                if (spellData is not null)
                {
                    yield return spellData;
                }
            }
        }
    }

    public sealed class QuestStepData : IDofusData<int>
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("d")]
        public string Description { get; init; }

        [JsonPropertyName("r")]
        [JsonConverter(typeof(QuestStepRewardsDataConverter))]
        public QuestStepRewardsData RewardsData { get; init; }

        [JsonIgnore]
        public int DialogQuestionId { get; internal set; }

        [JsonIgnore]
        public int OptimalLevel { get; internal set; }

        [JsonIgnore]
        public ReadOnlyCollection<int> QuestObjectivesId { get; internal set; }

        [JsonIgnore]
        public ReadOnlyCollection<IQuestObjective> QuestObjectives { get; internal set; }

        [JsonConstructor]
        internal QuestStepData()
        {
            Name = string.Empty;
            Description = string.Empty;
            RewardsData = new();
            QuestObjectivesId = ReadOnlyCollection<int>.Empty;
            QuestObjectives = ReadOnlyCollection<IQuestObjective>.Empty;
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
            return DofusApi.Datacenter.DialogsData.GetDialogQuestionDataById(DialogQuestionId);
        }
    }

    public sealed class QuestObjectiveData : IDofusData<int>
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("t")]
        public int QuestObjectiveTypeId { get; init; }

        [JsonPropertyName("p")]
        [JsonConverter(typeof(ReadOnlyToStringCollectionConverter))]
        public ReadOnlyCollection<string> Parameters { get; init; }

        [JsonPropertyName("x")]
        public int? XCoord { get; init; }

        [JsonPropertyName("y")]
        public int? YCoord { get; init; }

        [JsonConstructor]
        internal QuestObjectiveData()
        {
            Parameters = ReadOnlyCollection<string>.Empty;
        }

        public QuestObjectiveTypeData? GetQuestObjectiveTypeData()
        {
            return DofusApi.Datacenter.QuestsData.GetQuestObjectiveTypeDataById(QuestObjectiveTypeId);
        }

        public string GetCoordinate()
        {
            return XCoord.HasValue && YCoord.HasValue ? $"[{XCoord}, {YCoord}]" : "";
        }
    }

    public sealed class QuestObjectiveTypeData : IDofusData<int>
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("v")]
        public string Description { get; init; }

        [JsonConstructor]
        internal QuestObjectiveTypeData()
        {
            Description = string.Empty;
        }
    }

    public sealed class QuestsData : IDofusData
    {
        private const string FILE_NAME = "quests.json";

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

        internal static QuestsData Load()
        {
            QuestsData data = Datacenter.LoadDataFromFile<QuestsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
            QuestsCustomData dataCustom = Datacenter.LoadDataFromFile<QuestsCustomData>(Path.Combine(DofusApi.CUSTOM_PATH, FILE_NAME));

            foreach (QuestCustomData questCustomData in dataCustom.QuestsCustom)
            {
                QuestData? questData = data.GetQuestDataById(questCustomData.Id);
                if (questData is not null)
                {
                    questData.Repeatable = questCustomData.Repeatable;
                    questData.Account = questCustomData.Account;
                    questData.HasDungeon = questCustomData.HasDungeon;
                    questData.QuestStepsId = questCustomData.QuestStepsId.AsReadOnly();
                }
            }

            foreach (QuestStepCustomData questStepCustomData in dataCustom.QuestStepsCustom)
            {
                QuestStepData? questStepData = data.GetQuestStepDataById(questStepCustomData.Id);
                if (questStepData is not null)
                {
                    questStepData.DialogQuestionId = questStepCustomData.DialogQuestionId;
                    questStepData.OptimalLevel = questStepCustomData.OptimalLevel;
                    questStepData.QuestObjectivesId = questStepCustomData.QuestObjectivesId.AsReadOnly();
                }
            }

            foreach (QuestStepData questStepData in data.QuestSteps.Values)
            {
                List<QuestObjectiveData> questObjectivesData = [];
                foreach (int questObjectiveId in questStepData.QuestObjectivesId)
                {
                    QuestObjectiveData? questObjectiveData = data.GetQuestObjectiveDataById(questObjectiveId);
                    if (questObjectiveData is not null)
                    {
                        questObjectivesData.Add(questObjectiveData);
                    }
                }

                questStepData.QuestObjectives = QuestObjectiveFactory.GetQuestObjectives(questObjectivesData).ToList().AsReadOnly();
            }

            return data;
        }

        public QuestData? GetQuestDataById(int id)
        {
            Quests.TryGetValue(id, out QuestData? questData);
            return questData;
        }

        public IEnumerable<QuestData> GetQuestsDataByName(string name)
        {
            string[] names = name.NormalizeCustom().Split(' ');
            return Quests.Values.Where(x => names.All(x.Name.NormalizeCustom().Contains));
        }

        public string GetQuestNameById(int id)
        {
            QuestData? questData = GetQuestDataById(id);

            return questData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : questData.Name;
        }

        public QuestStepData? GetQuestStepDataById(int id)
        {
            QuestSteps.TryGetValue(id, out QuestStepData? questStepData);
            return questStepData;
        }

        public string GetQuestStepNameById(int id)
        {
            QuestStepData? questStepData = GetQuestStepDataById(id);

            return questStepData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : questStepData.Name;
        }

        public QuestObjectiveData? GetQuestObjectiveDataById(int id)
        {
            QuestObjectives.TryGetValue(id, out QuestObjectiveData? questObjectiveData);
            return questObjectiveData;
        }

        public Description GetQuestObjectiveDescriptionById(int id)
        {
            QuestObjectiveData? questObjectiveData = GetQuestObjectiveDataById(id);

            return questObjectiveData is null ? new(Resources.Unknown_Data, id.ToString()) : QuestObjectiveFactory.GetQuestObjective(questObjectiveData).GetDescription();
        }

        public QuestObjectiveTypeData? GetQuestObjectiveTypeDataById(int id)
        {
            QuestObjectiveTypes.TryGetValue(id, out QuestObjectiveTypeData? questObjectiveTypeData);
            return questObjectiveTypeData;
        }
    }
}
