using Cyberia.Api.DatacenterNS;
using Cyberia.Salamandra.DsharpPlus;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

using System.Text;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class QuestMessageBuilder : ICustomMessageBuilder
    {
        public const string PACKET_HEADER = "Q";
        public const int PACKET_VERSION = 1;

        private readonly QuestData _questData;
        private readonly List<QuestStepData> _questStepsData;
        private readonly int _selectedQuestStepIndex;
        private readonly QuestStepData? _questStepData;
        private readonly DialogQuestionData? _dialogQuestionData;

        public QuestMessageBuilder(QuestData questData, int selectedQuestStepIndex = 0)
        {
            _questData = questData;
            _questStepsData = questData.GetQuestStepsData();
            _selectedQuestStepIndex = selectedQuestStepIndex;
            _questStepData = _questStepsData.Count > selectedQuestStepIndex ? _questStepsData[selectedQuestStepIndex] : null;
            _dialogQuestionData = _questStepData?.GetDialogQuestionData();
        }

        public static QuestMessageBuilder? Create(int version, string[] parameters)
        {
            if (version == PACKET_VERSION &&
                parameters.Length > 1 &&
                int.TryParse(parameters[0], out int questId) &&
                int.TryParse(parameters[1], out int selectedQuestStepIndex))
            {
                QuestData? questData = Bot.Instance.Api.Datacenter.QuestsData.GetQuestDataById(questId);
                if (questData is not null)
                    return new QuestMessageBuilder(questData, selectedQuestStepIndex);
            }

            return null;
        }

        public static string GetPacket(int questId, int selectedQuestStepIndex = 0)
        {
            return InteractionManager.ComponentPacketBuilder(PACKET_HEADER, PACKET_VERSION, questId, selectedQuestStepIndex);
        }

        public async Task<T> GetMessageAsync<T>() where T : IDiscordMessageBuilder, new()
        {
            IDiscordMessageBuilder message = new T()
                .AddEmbed(await EmbedBuilder());

            DiscordSelectComponent select = Select1Builder();
            if (select.Options.Count > 1)
                message.AddComponents(select);

            DiscordSelectComponent select2 = Select2Builder();
            if (select2.Options.Count > 0)
                message.AddComponents(select2);

            return (T)message;
        }

        private Task<DiscordEmbedBuilder> EmbedBuilder()
        {
            DiscordEmbedBuilder embed = EmbedManager.BuildDofusEmbed(DofusEmbedCategory.Quests, "Livre de quêtes")
                .WithTitle($"{_questData.Name} ({_questData.Id}) {Emojis.Quest(_questData.Repeatable, _questData.Account)}{(_questData.HasDungeon ? Emojis.DUNGEON : "")}");

            if (_questStepData is not null)
            {
                embed.WithDescription($"{Formatter.Bold(_questStepData.Name)}\n{(string.IsNullOrEmpty(_questStepData.Description) ? "" : Formatter.Italic(_questStepData.Description))}");

                int optimalLevel = _questStepData.OptimalLevel;
                if (optimalLevel > 0)
                    embed.AddField("Niveau optimal :", optimalLevel.ToString());

                if (_dialogQuestionData is not null)
                    embed.AddField("Dialogue :", _dialogQuestionData.Question);

                if (_questStepData.QuestObjectives.Count > 0)
                    embed.AddQuestObjectiveFields(_questStepData.QuestObjectives);

                if (_questStepData.HasReward())
                {
                    StringBuilder rewards = new();

                    if (_questStepData.RewardsData.Experience > 0)
                        rewards.AppendFormat("{0} {1}\n", _questStepData.RewardsData.Experience.ToStringThousandSeparator(), Emojis.XP);

                    if (_questStepData.RewardsData.Kamas > 0)
                        rewards.AppendFormat("{0} {1}\n", _questStepData.RewardsData.Kamas.ToStringThousandSeparator(), Emojis.KAMAS);

                    List<KeyValuePair<ItemData, int>> itemsReward = _questStepData.RewardsData.GetItemsDataQuantities();
                    if (itemsReward.Count > 0)
                        rewards.AppendLine(string.Join(", ", itemsReward.Select(x => $"{Formatter.Bold(x.Value.ToString())}x {x.Key.Name}")));

                    List<EmoteData> emotesReward = _questStepData.RewardsData.GetEmotesData();
                    if (emotesReward.Count > 0)
                        rewards.AppendFormat("Emotes : {0}\n", string.Join(", ", emotesReward.Select(x => x.Name)));

                    List<JobData> jobsReward = _questStepData.RewardsData.GetJobsData();
                    if (jobsReward.Count > 0)
                        rewards.AppendFormat("Métiers : {0}\n", string.Join(", ", jobsReward.Select(x => x.Name)));

                    List<SpellData> spellsReward = _questStepData.RewardsData.GetSpellsData();
                    if (emotesReward.Count > 0)
                        rewards.AppendFormat("Sorts : {0}", string.Join(", ", spellsReward.Select(x => x.Name)));

                    if (rewards.Length > 0)
                        embed.AddField("Récompenses :", rewards.ToString());
                }
            }

            return Task.FromResult(embed);
        }

        private DiscordSelectComponent Select1Builder()
        {
            List<DiscordSelectComponentOption> options = new();

            for (int i = 0; i < _questStepsData.Count && i < 25; i++)
                options.Add(new($"Etape {i + 1}", GetPacket(_questData.Id, i), _questStepsData[i].Name.WithMaxLength(100), isDefault: i == _selectedQuestStepIndex));

            return new(InteractionManager.SelectComponentPacketBuilder(0), "Sélectionne une étape pour l'afficher", options);
        }

        private DiscordSelectComponent Select2Builder()
        {
            List<DiscordSelectComponentOption> options = new();

            for (int i = 25; i < _questStepsData.Count; i++)
                options.Add(new($"Etape {i + 1}", GetPacket(_questData.Id, i), _questStepsData[i].Name.WithMaxLength(100), isDefault: i == _selectedQuestStepIndex));

            return new(InteractionManager.SelectComponentPacketBuilder(1), "Sélectionne une étape pour l'afficher", options);
        }
    }
}
