using Cyberia.Api.Factories;
using Cyberia.Api.Factories.QuestObjectives;
using Cyberia.Api.Parser.JsonConverter;

using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class Quest
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("v")]
        public string Name { get; init; }

        public bool Repeatable { get; internal set; }

        public bool Account { get; internal set; }

        public bool HasDungeon { get; internal set; }

        public List<int> QuestStepsId { get; internal set; }

        public Quest()
        {
            Name = string.Empty;
            QuestStepsId = new();
        }

        public List<QuestStep> GetQuestSteps()
        {
            List<QuestStep> questSteps = new();

            foreach (int questStepId in QuestStepsId)
            {
                QuestStep? questStep = DofusApi.Instance.Datacenter.QuestsData.GetQuestStepById(questStepId);
                if (questStep is not null)
                    questSteps.Add(questStep);
            }

            return questSteps;
        }
    }

    public sealed class QuestStepRewards
    {
        public int Experience { get; init; }

        public int Kamas { get; init; }

        public List<KeyValuePair<int, int>> ItemsIdQuantities { get; init; }

        public List<int> EmotesId { get; init; }

        public List<int> JobsId { get; init; }

        public List<int> SpellsId { get; init; }

        public QuestStepRewards()
        {
            ItemsIdQuantities = new();
            EmotesId = new();
            JobsId = new();
            SpellsId = new();
        }

        public List<KeyValuePair<Item, int>> GetItemsQuantities()
        {
            List<KeyValuePair<Item, int>> itemsQuantities = new();

            foreach (KeyValuePair<int, int> pair in ItemsIdQuantities)
            {
                Item? item = DofusApi.Instance.Datacenter.ItemsData.GetItemById(pair.Key);
                if (item is not null)
                    itemsQuantities.Add(new(item, pair.Value));
            }

            return itemsQuantities;
        }

        public List<Emote> GetEmotes()
        {
            List<Emote> emotes = new();

            foreach (int emoteId in EmotesId)
            {
                Emote? emote = DofusApi.Instance.Datacenter.EmotesData.GetEmoteById(emoteId);
                if (emote is not null)
                    emotes.Add(emote);
            }

            return emotes;
        }

        public List<Job> GetJobs()
        {
            List<Job> jobs = new();

            foreach (int jobId in JobsId)
            {
                Job? job = DofusApi.Instance.Datacenter.JobsData.GetJobById(jobId);
                if (job is not null)
                    jobs.Add(job);
            }

            return jobs;
        }

        public List<Spell> GetSpells()
        {
            List<Spell> spells = new();

            foreach (int spellId in SpellsId)
            {
                Spell? spell = DofusApi.Instance.Datacenter.SpellsData.GetSpellById(spellId);
                if (spell is not null)
                    spells.Add(spell);
            }

            return spells;
        }
    }

    public sealed class QuestStep
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("d")]
        public string Description { get; init; }

        [JsonPropertyName("r")]
        [JsonConverter(typeof(QuestStepRewardsJsonConverter))]
        public QuestStepRewards Rewards { get; init; }

        public int DialogQuestionId { get; internal set; }

        public int OptimalLevel { get; internal set; }

        public List<int> QuestObjectivesId { get; internal set; }

        public List<IQuestObjective> QuestObjectives { get; internal set; }

        public QuestStep()
        {
            Name = string.Empty;
            Description = string.Empty;
            Rewards = new();
            QuestObjectivesId = new();
            QuestObjectives = new();
        }

        public bool HasReward()
        {
            return Rewards.Experience > 0 ||
                Rewards.Kamas > 0 ||
                Rewards.ItemsIdQuantities.Count > 0 ||
                Rewards.EmotesId.Count > 0 ||
                Rewards.JobsId.Count > 0 ||
                Rewards.SpellsId.Count > 0;
        }

        public DialogQuestion? GetDialogQuestion()
        {
            return DofusApi.Instance.Datacenter.DialogsData.GetDialogQuestionById(DialogQuestionId);
        }
    }

    public sealed class QuestObjective
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

        public QuestObjective()
        {
            Parameters = new();
        }

        public QuestObjectiveType? GetQuestObjectiveType()
        {
            return DofusApi.Instance.Datacenter.QuestsData.GetQuestObjectiveTypeById(QuestObjectiveTypeId);
        }

        public string GetCoordinate()
        {
            return XCoord.HasValue && YCoord.HasValue ? $"[{XCoord}, {YCoord}]" : "";
        }
    }

    public sealed class QuestObjectiveType
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("v")]
        public string Description { get; init; }

        public QuestObjectiveType()
        {
            Description = string.Empty;
        }
    }

    public sealed class QuestsData
    {
        private const string FILE_NAME = "quests.json";

        [JsonPropertyName("Q.q")]
        public List<Quest> Quests { get; init; }

        [JsonPropertyName("Q.s")]
        public List<QuestStep> QuestSteps { get; init; }

        [JsonPropertyName("Q.o")]
        public List<QuestObjective> QuestObjectives { get; init; }

        [JsonPropertyName("Q.t")]
        public List<QuestObjectiveType> QuestObjectiveTypes { get; init; }

        public QuestsData()
        {
            Quests = new();
            QuestSteps = new();
            QuestObjectives = new();
            QuestObjectiveTypes = new();
        }

        internal static QuestsData Build()
        {
            QuestsData data = Json.LoadFromFile<QuestsData>($"{DofusApi.OUTPUT_PATH}/{FILE_NAME}");
            QuestsCustomData dataCustom = Json.LoadFromFile<QuestsCustomData>($"{DofusApi.CUSTOM_PATH}/{FILE_NAME}");

            foreach (QuestCustom questCustom in dataCustom.QuestsCustom)
            {
                Quest? quest = data.GetQuestById(questCustom.Id);
                if (quest is not null)
                {
                    quest.Repeatable = questCustom.Repeatable;
                    quest.Account = questCustom.Account;
                    quest.HasDungeon = questCustom.HasDungeon;
                    quest.QuestStepsId = questCustom.QuestStepsId;
                }
            }

            foreach (QuestStepCustom questStepCustom in dataCustom.QuestStepsCustom)
            {
                QuestStep? questStep = data.GetQuestStepById(questStepCustom.Id);
                if (questStep is not null)
                {
                    questStep.DialogQuestionId = questStepCustom.DialogQuestionId;
                    questStep.OptimalLevel = questStepCustom.OptimalLevel;
                    questStep.QuestObjectivesId = questStepCustom.QuestObjectivesId;
                }
            }

            foreach (QuestStep questStep in data.QuestSteps)
            {
                List<QuestObjective> questObjectives = new();
                foreach (int questObjectiveId in questStep.QuestObjectivesId)
                {
                    QuestObjective? questObjective = data.GetQuestObjectiveById(questObjectiveId);
                    if (questObjective is not null)
                        questObjectives.Add(questObjective);
                }

                questStep.QuestObjectives = QuestObjectiveFactory.GetQuestObjectives(questObjectives).ToList();
            }

            return data;
        }

        public Quest? GetQuestById(int id)
        {
            return Quests.Find(x => x.Id == id);
        }

        public Quest? GetQuestByName(string name)
        {
            return Quests.Find(x => x.Name.RemoveDiacritics().Equals(name.RemoveDiacritics()));
        }

        public List<Quest> GetQuestsByName(string name)
        {
            string[] names = name.RemoveDiacritics().Split(' ');
            return Quests.FindAll(x => names.All(x.Name.RemoveDiacritics().Contains));
        }

        public string GetQuestNameById(int id)
        {
            Quest? quest = GetQuestById(id);

            return quest is null ? $"Inconnu ({id})" : quest.Name;
        }

        public QuestStep? GetQuestStepById(int id)
        {
            return QuestSteps.Find(x => x.Id == id);
        }

        public string GetQuestStepNameById(int id)
        {
            QuestStep? questStep = GetQuestStepById(id);

            return questStep is null ? $"Inconnu ({id})" : questStep.Name;
        }

        public QuestObjective? GetQuestObjectiveById(int id)
        {
            return QuestObjectives.Find(x => x.Id == id);
        }

        public string GetQuestObjectiveDescriptionById(int id)
        {
            QuestObjective? questObjective = GetQuestObjectiveById(id);

            return questObjective is null ? $"Inconnu ({id})" : QuestObjectiveFactory.GetQuestObjective(questObjective).GetDescription();
        }

        public QuestObjectiveType? GetQuestObjectiveTypeById(int id)
        {
            return QuestObjectiveTypes.Find(x => x.Id == id);
        }
    }
}
