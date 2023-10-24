using Cyberia.Api.DatacenterNS.Custom;
using Cyberia.Api.Factories;
using Cyberia.Api.Factories.QuestObjectives;
using Cyberia.Api.Parser.JsonConverter;

using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class QuestData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("v")]
        public string Name { get; init; }

        public bool Repeatable { get; internal set; }

        public bool Account { get; internal set; }

        public bool HasDungeon { get; internal set; }

        public List<int> QuestStepsId { get; internal set; }

        public QuestData()
        {
            Name = string.Empty;
            QuestStepsId = new();
        }

        public IEnumerable<QuestStepData> GetQuestStepsData()
        {
            foreach (int questStepId in QuestStepsId)
            {
                QuestStepData? questStepData = DofusApi.Instance.Datacenter.QuestsData.GetQuestStepDataById(questStepId);
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

        public QuestStepRewardsData()
        {
            ItemsIdQuantities = new();
            EmotesId = new();
            JobsId = new();
            SpellsId = new();
        }

        public IEnumerable<ItemData> GetItemsData()
        {
            foreach (KeyValuePair<int, int> pair in ItemsIdQuantities)
            {
                ItemData? itemData = DofusApi.Instance.Datacenter.ItemsData.GetItemDataById(pair.Key);
                if (itemData is not null)
                {
                    yield return itemData;
                }
            }
        }

        public Dictionary<ItemData, int> GetItemsDataQuantities()
        {
            Dictionary<ItemData, int> itemsDataQuantities = new();

            foreach (KeyValuePair<int, int> pair in ItemsIdQuantities)
            {
                ItemData? itemData = DofusApi.Instance.Datacenter.ItemsData.GetItemDataById(pair.Key);
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
                EmoteData? emoteData = DofusApi.Instance.Datacenter.EmotesData.GetEmoteById(emoteId);
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
                JobData? jobData = DofusApi.Instance.Datacenter.JobsData.GetJobDataById(jobId);
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
                SpellData? spellData = DofusApi.Instance.Datacenter.SpellsData.GetSpellDataById(spellId);
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
        [JsonConverter(typeof(QuestStepRewardsDataJsonConverter))]
        public QuestStepRewardsData RewardsData { get; init; }

        public int DialogQuestionId { get; internal set; }

        public int OptimalLevel { get; internal set; }

        public List<int> QuestObjectivesId { get; internal set; }

        public List<IQuestObjective> QuestObjectives { get; internal set; }

        public QuestStepData()
        {
            Name = string.Empty;
            Description = string.Empty;
            RewardsData = new();
            QuestObjectivesId = new();
            QuestObjectives = new();
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
            return DofusApi.Instance.Datacenter.DialogsData.GetDialogQuestionDataById(DialogQuestionId);
        }
    }

    public sealed class QuestObjectiveData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("t")]
        public int QuestObjectiveTypeId { get; init; }

        [JsonPropertyName("p")]
        [JsonConverter(typeof(StringListJsonConverter))]
        public List<string> Parameters { get; init; }

        [JsonPropertyName("x")]
        public int? XCoord { get; init; }

        [JsonPropertyName("y")]
        public int? YCoord { get; init; }

        public QuestObjectiveData()
        {
            Parameters = new();
        }

        public QuestObjectiveTypeData? GetQuestObjectiveTypeData()
        {
            return DofusApi.Instance.Datacenter.QuestsData.GetQuestObjectiveTypeDataById(QuestObjectiveTypeId);
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

        public QuestObjectiveTypeData()
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

        public QuestsData()
        {
            Quests = new();
            QuestSteps = new();
            QuestObjectives = new();
            QuestObjectiveTypes = new();
        }

        internal static QuestsData Build()
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
                List<QuestObjectiveData> questObjectivesData = new();
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
