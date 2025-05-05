using Cyberia.Api;
using Cyberia.Api.Data.Dialogs;
using Cyberia.Api.Data.Quests;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Extensions.DSharpPlus;
using Cyberia.Salamandra.Formatters;
using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.Entities;

using Microsoft.Extensions.DependencyInjection;

using System.Globalization;
using System.Text;

namespace Cyberia.Salamandra.Commands.Dofus.Quest;

public sealed class QuestMessageBuilder : ICustomMessageBuilder
{
    public const string PacketHeader = "Q";
    public const int PacketVersion = 2;

    private readonly EmbedBuilderService _embedBuilderService;
    private readonly QuestData _questData;
    private readonly List<QuestStepData> _questStepsData;
    private readonly int _selectedQuestStepIndex;
    private readonly bool _hasQuestStepRewardsBaseLevel;
    private readonly int? _selectedQuestStepRewardBaseLevelIndex;
    private readonly QuestStepData? _questStepData;
    private readonly DialogQuestionData? _dialogQuestionData;
    private readonly CultureInfo? _culture;

    public QuestMessageBuilder(EmbedBuilderService embedBuilderService, QuestData questData, int selectedQuestStepIndex, int? selectedQuestStepRewardBaseLevelIndex, CultureInfo? culture)
    {
        _embedBuilderService = embedBuilderService;
        _questData = questData;
        _questStepsData = questData.GetQuestStepsData().ToList();
        _selectedQuestStepIndex = selectedQuestStepIndex;
        _questStepData = _questStepsData.Count > selectedQuestStepIndex
            ? _questStepsData[selectedQuestStepIndex]
            : null;
        _hasQuestStepRewardsBaseLevel = _questStepData?.HasRewardsBaseLevel() ?? false;
        if (_hasQuestStepRewardsBaseLevel &&
            (selectedQuestStepRewardBaseLevelIndex is null || selectedQuestStepRewardBaseLevelIndex > _questStepData!.RewardsBaseLevelsData.Count - 1))
        {
            _selectedQuestStepRewardBaseLevelIndex = _questStepData!.RewardsBaseLevelsData.Count - 1;
        }
        else
        {
            _selectedQuestStepRewardBaseLevelIndex = selectedQuestStepRewardBaseLevelIndex;
        }

        _dialogQuestionData = _questStepData?.GetDialogQuestionData();
        _culture = culture;
    }

    public static QuestMessageBuilder? Create(IServiceProvider provider, int version, CultureInfo? culture, params ReadOnlySpan<string> parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 2 &&
            int.TryParse(parameters[0], out var questId) &&
            int.TryParse(parameters[1], out var selectedQuestStepIndex))
        {
            var questData = DofusApi.Datacenter.QuestsRepository.GetQuestDataById(questId);
            if (questData is not null)
            {
                var embedBuilderService = provider.GetRequiredService<EmbedBuilderService>();

                if (int.TryParse(parameters[2], out var selectedQuestStepRewardBaseLevelIndex))
                {
                    return new QuestMessageBuilder(embedBuilderService, questData, selectedQuestStepIndex, selectedQuestStepRewardBaseLevelIndex, culture);
                }

                return new QuestMessageBuilder(embedBuilderService, questData, selectedQuestStepIndex, null, culture);
            }
        }

        return null;
    }

    public static string GetPacket(int questId, int selectedQuestStepIndex, int? selectedQuestStepRewardBaseLevelIndex)
    {
        return PacketFormatter.Action(PacketHeader, PacketVersion, questId, selectedQuestStepIndex, selectedQuestStepRewardBaseLevelIndex);
    }

    public async Task<T> BuildAsync<T>() where T : IDiscordMessageBuilder, new()
    {
        var message = new T()
            .AddEmbed(await EmbedBuilder());

        var select = QuestStepSelectBuilder(0, 0);
        if (select.Options.Count > 1)
        {
            message.AddActionRowComponent(select);
        }

        select = QuestStepSelectBuilder(1, Constant.MaxSelectOption);
        if (select.Options.Count > 0)
        {
            message.AddActionRowComponent(select);
        }

        if (_hasQuestStepRewardsBaseLevel)
        {
            select = QuestStepRewardsBaseLevelSelectBuilder(2, 0);
            if (select.Options.Count > 1)
            {
                message.AddActionRowComponent(select);
            }

            select = QuestStepRewardsBaseLevelSelectBuilder(3, Constant.MaxSelectOption);
            if (select.Options.Count > 0)
            {
                message.AddActionRowComponent(select);
            }
        }

        return (T)message;
    }

    private Task<DiscordEmbedBuilder> EmbedBuilder()
    {
        var embed = _embedBuilderService.CreateEmbedBuilder(EmbedCategory.Quests, Translation.Get<BotTranslations>("Embed.Quest.Author", _culture), _culture)
            .WithTitle($"{_questData.Name.ToString(_culture)} ({_questData.Id}) {Emojis.Quest(_questData, _culture)}{(_questData.HasDungeon ? Emojis.Dungeon(_culture) : string.Empty)}");

        if (_questStepData is not null)
        {
            embed.WithDescription($"{Formatter.Bold(_questStepData.Name.ToString(_culture))}");

            var description = _questStepData.Description.ToString(_culture);
            if (!string.IsNullOrEmpty(description))
            {
                embed.Description += $"\n{Formatter.Italic(description)}";
            }

            var optimalLevel = _questStepData.OptimalLevel;
            if (optimalLevel > 0)
            {
                embed.AddField(Translation.Get<BotTranslations>("Embed.Field.OptimalLevel.Title", _culture), optimalLevel.ToString());
            }

            if (_dialogQuestionData is not null)
            {
                embed.AddField(
                    Translation.Get<BotTranslations>("Embed.Field.Dialog.Title", _culture),
                    _dialogQuestionData.Message.ToString(_culture));
            }

            if (_questStepData.QuestObjectives.Count > 0)
            {
                embed.AddQuestObjectivesFields(_questStepData.QuestObjectives, _culture);
            }

            if (_questStepData.HasRewards())
            {
                StringBuilder rewardsBuilder = new();

                if (_questStepData.RewardsData.Experience > 0)
                {
                    rewardsBuilder.Append(_questStepData.RewardsData.Experience.ToFormattedString(_culture));
                    rewardsBuilder.Append(' ');
                    rewardsBuilder.Append(Emojis.Xp(_culture));
                    rewardsBuilder.Append('\n');
                }

                if (_questStepData.RewardsData.Kamas > 0)
                {
                    rewardsBuilder.Append(_questStepData.RewardsData.Kamas.ToFormattedString(_culture));
                    rewardsBuilder.Append(' ');
                    rewardsBuilder.Append(Emojis.Kamas(_culture));
                    rewardsBuilder.Append('\n');
                }

                var itemsReward = _questStepData.RewardsData.GetItemsDataQuantities();
                if (itemsReward.Count > 0)
                {
                    rewardsBuilder.Append(string.Join(", ", itemsReward.Select(x => $"{Formatter.Bold(x.Value.ToString())}x {x.Key.Name.ToString(_culture)}")));
                    rewardsBuilder.Append('\n');
                }

                var emotesReward = _questStepData.RewardsData.GetEmotesData();
                if (emotesReward.Any())
                {
                    rewardsBuilder.Append(Translation.Get<BotTranslations>("Embed.Field.Rewards.Content.Emotes", _culture));
                    rewardsBuilder.Append(' ');
                    rewardsBuilder.Append(string.Join(", ", emotesReward.Select(x => Emojis.Emote(x, _culture) + x.Name.ToString(_culture))));
                    rewardsBuilder.Append('\n');
                }

                var jobsReward = _questStepData.RewardsData.GetJobsData();
                if (jobsReward.Any())
                {
                    rewardsBuilder.Append(Translation.Get<BotTranslations>("Embed.Field.Rewards.Content.Jobs", _culture));
                    rewardsBuilder.Append(' ');
                    rewardsBuilder.Append(string.Join(", ", jobsReward.Select(x => Emojis.Job(x, _culture) + x.Name.ToString(_culture))));
                    rewardsBuilder.Append('\n');
                }

                var spellsReward = _questStepData.RewardsData.GetSpellsData();
                if (spellsReward.Any())
                {
                    rewardsBuilder.Append(Translation.Get<BotTranslations>("Embed.Field.Rewards.Content.Spells", _culture));
                    rewardsBuilder.Append(' ');
                    rewardsBuilder.Append(string.Join(", ", spellsReward.Select(x => x.Name.ToString(_culture))));
                    rewardsBuilder.Append('\n');
                }

                if (rewardsBuilder.Length > 0)
                {
                    embed.AddField(Translation.Get<BotTranslations>("Embed.Field.Rewards.Title", _culture), rewardsBuilder.ToString());
                }
            }

            if (_hasQuestStepRewardsBaseLevel)
            {
                var questStepRewardBaseLevel = _questStepData.RewardsBaseLevelsData[_selectedQuestStepRewardBaseLevelIndex!.Value];
                StringBuilder rewardsBaseLevelsBuilder = new();

                if (questStepRewardBaseLevel.Experience > 0)
                {
                    rewardsBaseLevelsBuilder.Append(questStepRewardBaseLevel.Experience.ToFormattedString(_culture));
                    rewardsBaseLevelsBuilder.Append(' ');
                    rewardsBaseLevelsBuilder.Append(Emojis.Xp(_culture));
                    rewardsBaseLevelsBuilder.Append('\n');
                }

                if (questStepRewardBaseLevel.Kamas > 0)
                {
                    rewardsBaseLevelsBuilder.Append(questStepRewardBaseLevel.Kamas.ToFormattedString(_culture));
                    rewardsBaseLevelsBuilder.Append(' ');
                    rewardsBaseLevelsBuilder.Append(Emojis.Kamas(_culture));
                    rewardsBaseLevelsBuilder.Append('\n');
                }

                if (rewardsBaseLevelsBuilder.Length > 0)
                {
                    embed.AddField(Translation.Get<BotTranslations>("Embed.Field.RewardsBaseLevel.Title", _culture), rewardsBaseLevelsBuilder.ToString());
                }
            }
        }

        return Task.FromResult(embed);
    }

    private DiscordSelectComponent QuestStepSelectBuilder(int selectIndex, int startIndex)
    {
        IEnumerable<DiscordSelectComponentOption> OptionsGenerator(int startIndex)
        {
            var endIndex = Math.Min(startIndex + Constant.MaxSelectOption, _questStepsData.Count);
            for (var i = startIndex; i < endIndex; i++)
            {
                yield return new DiscordSelectComponentOption(
                    $"{Translation.Get<BotTranslations>("Select.QuestStep.Content.Step", _culture)} {i + 1}",
                    GetPacket(_questData.Id, i, null),
                    StringExtensions.WithMaxLength(_questStepsData[i].Name.ToString(_culture), 100),
                    isDefault: i == _selectedQuestStepIndex);
            }
        }

        return new DiscordSelectComponent(
            PacketFormatter.Select(selectIndex),
            Translation.Get<BotTranslations>("Select.QuestStep.Placeholder", _culture),
            OptionsGenerator(startIndex));
    }

    private DiscordSelectComponent QuestStepRewardsBaseLevelSelectBuilder(int selectIndex, int startIndex)
    {
        IEnumerable<DiscordSelectComponentOption> OptionsGenerator(int startIndex)
        {
            if (_questStepData is null)
            {
                yield break;
            }

            var endIndex = Math.Min(startIndex + Constant.MaxSelectOption, _questStepData.RewardsBaseLevelsData.Count);
            for (var i = startIndex; i < endIndex; i++)
            {
                var questStepRewardBaseLevel = _questStepData.RewardsBaseLevelsData[i];
                var minLevel = questStepRewardBaseLevel.MinLevel;
                var maxLevel = questStepRewardBaseLevel.MaxLevel;

                yield return new DiscordSelectComponentOption(
                    $"{Translation.Get<BotTranslations>("Select.QuestStepRewardsBaseLevel.Content.Rewards", _culture)} {Translation.Get<BotTranslations>("ShortLevel", _culture)}{minLevel}{(minLevel == maxLevel ? string.Empty : $"-{maxLevel}")}",
                    GetPacket(_questData.Id, _selectedQuestStepIndex, i),
                    isDefault: i == _selectedQuestStepRewardBaseLevelIndex);
            }
        }

        return new DiscordSelectComponent(
            PacketFormatter.Select(selectIndex),
            Translation.Get<BotTranslations>("Select.QuestStepRewardsBaseLevel.Placeholder", _culture),
            OptionsGenerator(startIndex));
    }
}
