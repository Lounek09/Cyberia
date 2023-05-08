using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Factories;
using Cyberia.Api.Factories.QuestObjectives;
using Cyberia.Salamandra.DsharpPlus;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class QuestMessageBuilder : CustomMessageBuilder
    {
        private readonly Quest _quest;
        private readonly List<QuestStep> _questSteps;
        private QuestStep? _currentQuestStep;
        private int _selectIndex;

        public QuestMessageBuilder(Quest quest) :
            base()
        {
            _quest = quest;
            _questSteps = quest.GetQuestSteps();
            _currentQuestStep = _questSteps.Count > 0 ? _questSteps[0] : null;
            _selectIndex = 0;
        }

        protected override Task<DiscordEmbedBuilder> EmbedBuilder()
        {
            DiscordEmbedBuilder embed = EmbedManager.BuildDofusEmbed(DofusEmbedCategory.Quests, "Livre de quêtes")
                .WithTitle($"{_quest.Name} ({_quest.Id}) {Emojis.Quest(_quest.Repeatable, _quest.Account)}{(_quest.HasDungeon ? Emojis.DUNGEON : "")}");

            if (_currentQuestStep is not null)
            {
                embed.WithDescription($"{Formatter.Bold(_currentQuestStep.Name)}\n" +
                    (string.IsNullOrEmpty(_currentQuestStep.Description) ? "" : Formatter.Italic(_currentQuestStep.Description)));

                int optimalLevel = _currentQuestStep.OptimalLevel;
                if (optimalLevel > 0)
                    embed.AddField("Niveau optimal :", optimalLevel.ToString());

                DialogQuestion? dialogQuestion = _currentQuestStep.GetDialogQuestion();
                if (dialogQuestion is not null)
                    embed.AddField("Dialogue :", dialogQuestion.Question);

                if (_currentQuestStep.QuestObjectives.Count > 0)
                    embed.AddQuestObjectiveFields("Objectifs :", _currentQuestStep.QuestObjectives);

                if (_currentQuestStep.HasReward())
                {
                    HashSet<string> rewardValue = new();

                    if (_currentQuestStep.Rewards.Experience > 0)
                        rewardValue.Add($"{_currentQuestStep.Rewards.Experience.ToStringThousandSeparator()} {Emojis.XP}");

                    if (_currentQuestStep.Rewards.Kamas > 0)
                        rewardValue.Add($"{_currentQuestStep.Rewards.Kamas.ToStringThousandSeparator()} {Emojis.KAMAS}");

                    List<KeyValuePair<Item, int>> itemsReward = _currentQuestStep.Rewards.GetItemsQuantities();
                    if (itemsReward.Count > 0)
                    {
                        HashSet<string> itemsRewardNames = new();
                        foreach (KeyValuePair<Item, int> itemReward in itemsReward)
                            itemsRewardNames.Add($"{Formatter.Bold(itemReward.Value.ToString())}x {itemReward.Key.Name}");

                        if (itemsRewardNames.Count > 0)
                            rewardValue.Add(string.Join(", ", itemsRewardNames));
                    }

                    List<Emote> emotesReward = _currentQuestStep.Rewards.GetEmotes();
                    if (emotesReward.Count > 0)
                    {
                        HashSet<string> emotesRewardNames = new();
                        foreach (Emote emoteReward in emotesReward)
                            emotesRewardNames.Add(emoteReward.Name);

                        if (emotesRewardNames.Count > 0)
                            rewardValue.Add($"Emotes : {string.Join(", ", emotesRewardNames)}");
                    }

                    List<Job> jobsReward = _currentQuestStep.Rewards.GetJobs();
                    if (jobsReward.Count > 0)
                    {
                        HashSet<string> jobsRewardNames = new();
                        foreach (Job jobReward in jobsReward)
                            jobsRewardNames.Add(jobReward.Name);

                        if (jobsRewardNames.Count > 0)
                            rewardValue.Add($"Métiers : {string.Join(", ", jobsRewardNames)}");
                    }

                    List<Spell> spellsReward = _currentQuestStep.Rewards.GetSpells();
                    if (emotesReward.Count > 0)
                    {
                        HashSet<string> spellsRewardNames = new();
                        foreach (Spell spellReward in spellsReward)
                            spellsRewardNames.Add(spellReward.Name);

                        if (spellsRewardNames.Count > 0)
                            rewardValue.Add($"Sorts : {string.Join(", ", spellsRewardNames)}");
                    }

                    if (rewardValue.Count > 0)
                        embed.AddField("Récompenses :", string.Join('\n', rewardValue));
                }
            }

            return Task.FromResult(embed);
        }

        private DiscordSelectComponent SelectBuilder()
        {
            HashSet<DiscordSelectComponentOption> options = new();

            for (int i = 0; i < _questSteps.Count && i < 25; i++)
                options.Add(new("Etape " + (i + 1), $"{_questSteps[i].Id}|{i}", _questSteps[i].Name.WithMaxLength(100), isDefault: i == _selectIndex));

            return new("select", "Sélectionne une étape pour l'afficher", options);
        }

        private DiscordSelectComponent Select2Builder()
        {
            HashSet<DiscordSelectComponentOption> options = new();

            for (int i = 25; i < _questSteps.Count; i++)
                options.Add(new("Etape " + (i + 1), $"{_questSteps[i].Id}|{i}", _questSteps[i].Name.WithMaxLength(100), isDefault: i == _selectIndex));

            return new("select2", "Sélectionne une étape pour l'afficher", options);
        }

        protected override async Task<DiscordInteractionResponseBuilder> InteractionResponseBuilder()
        {
            DiscordInteractionResponseBuilder response = await base.InteractionResponseBuilder();

            DiscordSelectComponent select = SelectBuilder();
            if (select.Options.Count > 1)
                response.AddComponents(select);

            DiscordSelectComponent select2 = Select2Builder();
            if (select2.Options.Count > 1)
                response.AddComponents(select2);

            return response;
        }

        protected override async Task<DiscordFollowupMessageBuilder> FollowupMessageBuilder()
        {
            DiscordFollowupMessageBuilder followupMessage = await base.FollowupMessageBuilder();

            DiscordSelectComponent select = SelectBuilder();
            if (select.Options.Count > 1)
                followupMessage.AddComponents(select);

            DiscordSelectComponent select2 = Select2Builder();
            if (select2.Options.Count > 1)
                followupMessage.AddComponents(select2);

            return followupMessage;
        }

        protected override async Task<bool> InteractionTreatment(ComponentInteractionCreateEventArgs e)
        {
            switch (e.Id)
            {
                case "select":
                case "select2":
                    string[] args = e.Interaction.Data.Values.First().Split("|");

                    int id = Convert.ToInt32(args[0]);
                    _currentQuestStep = Bot.Instance.Api.Datacenter.QuestsData.GetQuestStepById(id);
                    _selectIndex = Convert.ToInt32(args[1]);

                    await UpdateInteractionResponse(e.Interaction);
                    return true;
                default:
                    await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
                    return false;
            }
        }
    }
}
