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

        private readonly Quest _quest;
        private readonly List<QuestStep> _questSteps;
        private readonly int _selectedQuestStepIndex;
        private readonly QuestStep? _questStep;

        public QuestMessageBuilder(Quest quest, int selectedQuestStepIndex = 0)
        {
            _quest = quest;
            _questSteps = quest.GetQuestSteps();
            _selectedQuestStepIndex = selectedQuestStepIndex;
            _questStep = _questSteps.Count > selectedQuestStepIndex ? _questSteps[selectedQuestStepIndex] : null;
        }

        public static QuestMessageBuilder? Create(int version, string[] parameters)
        {
            if (version == PACKET_VERSION &&
                parameters.Length > 1 &&
                int.TryParse(parameters[0], out int questId) &&
                int.TryParse(parameters[1], out int selectedQuestStepIndex))
            {
                Quest? quest = Bot.Instance.Api.Datacenter.QuestsData.GetQuestById(questId);
                if (quest is not null)
                    return new QuestMessageBuilder(quest, selectedQuestStepIndex);
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
                .WithTitle($"{_quest.Name} ({_quest.Id}) {Emojis.Quest(_quest.Repeatable, _quest.Account)}{(_quest.HasDungeon ? Emojis.DUNGEON : "")}");

            if (_questStep is not null)
            {
                embed.WithDescription($"{Formatter.Bold(_questStep.Name)}\n{(string.IsNullOrEmpty(_questStep.Description) ? "" : Formatter.Italic(_questStep.Description))}");

                int optimalLevel = _questStep.OptimalLevel;
                if (optimalLevel > 0)
                    embed.AddField("Niveau optimal :", optimalLevel.ToString());

                DialogQuestion? dialogQuestion = _questStep.GetDialogQuestion();
                if (dialogQuestion is not null)
                    embed.AddField("Dialogue :", dialogQuestion.Question);

                if (_questStep.QuestObjectives.Count > 0)
                    embed.AddQuestObjectiveFields("Objectifs :", _questStep.QuestObjectives);

                if (_questStep.HasReward())
                {
                    StringBuilder rewards = new();

                    if (_questStep.Rewards.Experience > 0)
                        rewards.AppendFormat("{0} {1}\n", _questStep.Rewards.Experience.ToStringThousandSeparator(), Emojis.XP);

                    if (_questStep.Rewards.Kamas > 0)
                        rewards.AppendFormat("{0} {1}\n", _questStep.Rewards.Kamas.ToStringThousandSeparator(), Emojis.KAMAS);

                    List<KeyValuePair<Item, int>> itemsReward = _questStep.Rewards.GetItemsQuantities();
                    if (itemsReward.Count > 0)
                        rewards.AppendLine(string.Join(", ", itemsReward.Select(x => $"{Formatter.Bold(x.Value.ToString())}x {x.Key.Name}")));

                    List<Emote> emotesReward = _questStep.Rewards.GetEmotes();
                    if (emotesReward.Count > 0)
                        rewards.AppendFormat("Emotes : {0}\n", string.Join(", ", emotesReward.Select(x => x.Name)));

                    List<Job> jobsReward = _questStep.Rewards.GetJobs();
                    if (jobsReward.Count > 0)
                        rewards.AppendFormat("Métiers : {0}\n", string.Join(", ", jobsReward.Select(x => x.Name)));

                    List<Spell> spellsReward = _questStep.Rewards.GetSpells();
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

            for (int i = 0; i < _questSteps.Count && i < 25; i++)
                options.Add(new($"Etape {i + 1}", GetPacket(_quest.Id, i), _questSteps[i].Name.WithMaxLength(100), isDefault: i == _selectedQuestStepIndex));

            return new(InteractionManager.SelectComponentPacketBuilder(0), "Sélectionne une étape pour l'afficher", options);
        }

        private DiscordSelectComponent Select2Builder()
        {
            List<DiscordSelectComponentOption> options = new();

            for (int i = 25; i < _questSteps.Count; i++)
                options.Add(new($"Etape {i + 1}", GetPacket(_quest.Id, i), _questSteps[i].Name.WithMaxLength(100), isDefault: i == _selectedQuestStepIndex));

            return new(InteractionManager.SelectComponentPacketBuilder(1), "Sélectionne une étape pour l'afficher", options);
        }
    }
}
