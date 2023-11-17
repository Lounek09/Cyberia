using Cyberia.Api.Data.Custom;
using Cyberia.Api.Factories;
using Cyberia.Api.Factories.QuestObjectives;
using Cyberia.Api.JsonConverters;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data
{
    public sealed class QuestData
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
        public List<int> QuestStepsId { get; internal set; }

        [JsonConstructor]
        internal QuestData()
        {
            Name = string.Empty;
            QuestStepsId = [];
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

    public sealed class QuestStepRewardsData
    {
        public int Experience { get; init; }

        public int Kamas { get; init; }

        public Dictionary<int, int> ItemsIdQuantities { get; init; }

        public List<int> EmotesId { get; init; }

        public List<int> JobsId { get; init; }

        public List<int> SpellsId { get; init; }

        internal QuestStepRewardsData()
        {
            ItemsIdQuantities = [];
            EmotesId = [];
            JobsId = [];
            SpellsId = [];
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

        public Dictionary<ItemData, int> GetItemsDataQuantities()
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

            return itemsDataQuantities;
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

    public sealed class QuestStepData
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
        public List<int> QuestObjectivesId { get; internal set; }

        [JsonIgnore]
        public List<IQuestObjective> QuestObjectives { get; internal set; }

        [JsonConstructor]
        internal QuestStepData()
        {
            Name = string.Empty;
            Description = string.Empty;
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
            return DofusApi.Datacenter.DialogsData.GetDialogQuestionDataById(DialogQuestionId);
        }
    }

    public sealed class QuestObjectiveData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("t")]
        public int QuestObjectiveTypeId { get; init; }

        [JsonPropertyName("p")]
        [JsonConverter(typeof(StringListConverter))]
        public List<string> Parameters { get; init; }

        [JsonPropertyName("x")]
        public int? XCoord { get; init; }

        [JsonPropertyName("y")]
        public int? YCoord { get; init; }

        [JsonConstructor]
        internal QuestObjectiveData()
        {
            Parameters = [];
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

    public sealed class QuestObjectiveTypeData
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

    public sealed class QuestsData
    {
        private const string FILE_NAME = "quests.json";

        [JsonPropertyName("Q.q")]
        public List<QuestData> Quests { get; init; }

        [JsonPropertyName("Q.s")]
        public List<QuestStepData> QuestSteps { get; init; }

        [JsonPropertyName("Q.o")]
        public List<QuestObjectiveData> QuestObjectives { get; init; }

        [JsonPropertyName("Q.t")]
        public List<QuestObjectiveTypeData> QuestObjectiveTypes { get; init; }

        [JsonConstructor]
        public QuestsData()
        {
            Quests = [];
            QuestSteps = [];
            QuestObjectives = [];
            QuestObjectiveTypes = [];
        }

        internal static QuestsData Load()
        {
            QuestsData data = Json.LoadFromFile<QuestsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
            QuestsCustomData dataCustom = Json.LoadFromFile<QuestsCustomData>(Path.Combine(DofusApi.CUSTOM_PATH, FILE_NAME));

            foreach (QuestCustomData questCustomData in dataCustom.QuestsCustom)
            {
                QuestData? questData = data.GetQuestDataById(questCustomData.Id);
                if (questData is not null)
                {
                    questData.Repeatable = questCustomData.Repeatable;
                    questData.Account = questCustomData.Account;
                    questData.HasDungeon = questCustomData.HasDungeon;
                    questData.QuestStepsId = questCustomData.QuestStepsId;
                }
            }

            foreach (QuestStepCustomData questStepCustomData in dataCustom.QuestStepsCustom)
            {
                QuestStepData? questStepData = data.GetQuestStepDataById(questStepCustomData.Id);
                if (questStepData is not null)
                {
                    questStepData.DialogQuestionId = questStepCustomData.DialogQuestionId;
                    questStepData.OptimalLevel = questStepCustomData.OptimalLevel;
                    questStepData.QuestObjectivesId = questStepCustomData.QuestObjectivesId;
                }
            }

            foreach (QuestStepData questStepData in data.QuestSteps)
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

                questStepData.QuestObjectives = QuestObjectiveFactory.GetQuestObjectives(questObjectivesData).ToList();
            }

            return data;
        }

        public QuestData? GetQuestDataById(int id)
        {
            return Quests.Find(x => x.Id == id);
        }

        public QuestData? GetQuestDataByName(string name)
        {
            return Quests.Find(x => x.Name.NormalizeCustom().Equals(name.NormalizeCustom()));
        }

        public List<QuestData> GetQuestsDataByName(string name)
        {
            string[] names = name.NormalizeCustom().Split(' ');
            return Quests.FindAll(x => names.All(x.Name.NormalizeCustom().Contains));
        }

        public string GetQuestNameById(int id)
        {
            QuestData? questData = GetQuestDataById(id);

            return questData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : questData.Name;
        }

        public QuestStepData? GetQuestStepDataById(int id)
        {
            return QuestSteps.Find(x => x.Id == id);
        }

        public string GetQuestStepNameById(int id)
        {
            QuestStepData? questStepData = GetQuestStepDataById(id);

            return questStepData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : questStepData.Name;
        }

        public QuestObjectiveData? GetQuestObjectiveDataById(int id)
        {
            return QuestObjectives.Find(x => x.Id == id);
        }

        public Description GetQuestObjectiveDescriptionById(int id)
        {
            QuestObjectiveData? questObjectiveData = GetQuestObjectiveDataById(id);

            return questObjectiveData is null ? new(Resources.Unknown_Data, id.ToString()) : QuestObjectiveFactory.GetQuestObjective(questObjectiveData).GetDescription();
        }

        public QuestObjectiveTypeData? GetQuestObjectiveTypeDataById(int id)
        {
            return QuestObjectiveTypes.Find(x => x.Id == id);
        }
    }
}
