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
    public const int PacketVersion = 1;

    private readonly EmbedBuilderService _embedBuilderService;
    private readonly QuestData _questData;
    private readonly List<QuestStepData> _questStepsData;
    private readonly int _selectedQuestStepIndex;
    private readonly QuestStepData? _questStepData;
    private readonly DialogQuestionData? _dialogQuestionData;
    private readonly CultureInfo? _culture;

    public QuestMessageBuilder(EmbedBuilderService embedBuilderService, QuestData questData, CultureInfo? culture, int selectedQuestStepIndex = 0)
    {
        _embedBuilderService = embedBuilderService;
        _questData = questData;
        _questStepsData = questData.GetQuestStepsData().ToList();
        _selectedQuestStepIndex = selectedQuestStepIndex;
        _questStepData = _questStepsData.Count > selectedQuestStepIndex ? _questStepsData[selectedQuestStepIndex] : null;
        _dialogQuestionData = _questStepData?.GetDialogQuestionData();
        _culture = culture;
    }

    public static QuestMessageBuilder? Create(IServiceProvider provider, int version, CultureInfo? culture, string[] parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 1 &&
            int.TryParse(parameters[0], out var questId) &&
            int.TryParse(parameters[1], out var selectedQuestStepIndex))
        {
            var questData = DofusApi.Datacenter.QuestsRepository.GetQuestDataById(questId);
            if (questData is not null)
            {
                var embedBuilderService = provider.GetRequiredService<EmbedBuilderService>();

                return new QuestMessageBuilder(embedBuilderService, questData, culture, selectedQuestStepIndex);
            }
        }

        return null;
    }

    public static string GetPacket(int questId, int selectedQuestStepIndex = 0)
    {
        return PacketFormatter.Action(PacketHeader, PacketVersion, questId, selectedQuestStepIndex);
    }

    public async Task<T> BuildAsync<T>() where T : IDiscordMessageBuilder, new()
    {
        var message = new T()
            .AddEmbed(await EmbedBuilder());

        var select = SelectBuilder(0, 0);
        if (select.Options.Count > 1)
        {
            message.AddComponents(select);
        }

        var select2 = SelectBuilder(1, Constant.MaxSelectOption);
        if (select2.Options.Count > 0)
        {
            message.AddComponents(select2);
        }

        return (T)message;
    }

    private Task<DiscordEmbedBuilder> EmbedBuilder()
    {
        var embed = _embedBuilderService.CreateEmbedBuilder(EmbedCategory.Quests, Translation.Get<BotTranslations>("Embed.Quest.Author", _culture))
            .WithTitle($"{_questData.Name.ToString(_culture)} ({_questData.Id}) {Emojis.Quest(_questData.Repeatable, _questData.Account)}{(_questData.HasDungeon ? Emojis.Dungeon : string.Empty)}");

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

            if (_questStepData.HasReward())
            {
                StringBuilder rewardsBuilder = new();

                if (_questStepData.RewardsData.Experience > 0)
                {
                    rewardsBuilder.Append(_questStepData.RewardsData.Experience.ToFormattedString(_culture));
                    rewardsBuilder.Append(' ');
                    rewardsBuilder.Append(Emojis.Xp);
                    rewardsBuilder.Append('\n');
                }

                if (_questStepData.RewardsData.Kamas > 0)
                {
                    rewardsBuilder.Append(_questStepData.RewardsData.Kamas.ToFormattedString(_culture));
                    rewardsBuilder.Append(' ');
                    rewardsBuilder.Append(Emojis.Kamas);
                    rewardsBuilder.Append('\n');
                }

                var itemsReward = _questStepData.RewardsData.GetItemsDataQuantities();
                if (itemsReward.Count > 0)
                {
                    rewardsBuilder.Append(string.Join(", ", itemsReward.Select(x => $"{Formatter.Bold(x.Value.ToString())}x {x.Key.Name.ToString(_culture)}")));
                    rewardsBuilder.Append('\n');
                }

                var emotesReward = _questStepData.RewardsData.GetEmotesData().ToList();
                if (emotesReward.Count > 0)
                {
                    rewardsBuilder.Append(Translation.Get<BotTranslations>("Embed.Field.Rewards.Content.Emotes", _culture));
                    rewardsBuilder.Append(' ');
                    rewardsBuilder.Append(string.Join(", ", emotesReward.Select(x => x.Name.ToString(_culture))));
                    rewardsBuilder.Append('\n');
                }

                var jobsReward = _questStepData.RewardsData.GetJobsData().ToList();
                if (jobsReward.Count > 0)
                {
                    rewardsBuilder.Append(Translation.Get<BotTranslations>("Embed.Field.Rewards.Content.Jobs", _culture));
                    rewardsBuilder.Append(' ');
                    rewardsBuilder.Append(string.Join(", ", jobsReward.Select(x => x.Name.ToString(_culture))));
                    rewardsBuilder.Append('\n');
                }

                var spellsReward = _questStepData.RewardsData.GetSpellsData().ToList();
                if (spellsReward.Count > 0)
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
        }

        return Task.FromResult(embed);
    }

    private DiscordSelectComponent SelectBuilder(int selectIndex, int startIndex)
    {
        IEnumerable<DiscordSelectComponentOption> OptionsGenerator(int startIndex)
        {
            var endIndex = Math.Min(startIndex + Constant.MaxSelectOption, _questStepsData.Count);
            for (var i = startIndex; i < endIndex; i++)
            {
                yield return new DiscordSelectComponentOption(
                    $"{Translation.Get<BotTranslations>("Select.QuestStep.Content.Step", _culture)} {i + 1}",
                    GetPacket(_questData.Id, i),
                    StringExtensions.WithMaxLength(_questStepsData[i].Name.ToString(_culture), 100),
                    isDefault: i == _selectedQuestStepIndex);
            }
        }

        return new DiscordSelectComponent(
            PacketFormatter.Select(selectIndex),
            Translation.Get<BotTranslations>("Select.QuestStep.Placeholder", _culture),
            OptionsGenerator(startIndex));
    }
}
