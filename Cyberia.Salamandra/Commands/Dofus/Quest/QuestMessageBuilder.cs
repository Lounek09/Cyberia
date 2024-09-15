using Cyberia.Api;
using Cyberia.Api.Data.Dialogs;
using Cyberia.Api.Data.Quests;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Extensions.DSharpPlus;
using Cyberia.Salamandra.Managers;
using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.Entities;

using Microsoft.Extensions.DependencyInjection;

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

    public QuestMessageBuilder(EmbedBuilderService embedBuilderService, QuestData questData, int selectedQuestStepIndex = 0)
    {
        _embedBuilderService = embedBuilderService;
        _questData = questData;
        _questStepsData = questData.GetQuestStepsData().ToList();
        _selectedQuestStepIndex = selectedQuestStepIndex;
        _questStepData = _questStepsData.Count > selectedQuestStepIndex ? _questStepsData[selectedQuestStepIndex] : null;
        _dialogQuestionData = _questStepData?.GetDialogQuestionData();
    }

    public static QuestMessageBuilder? Create(IServiceProvider provider, int version, string[] parameters)
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

                return new QuestMessageBuilder(embedBuilderService, questData, selectedQuestStepIndex);
            }
        }

        return null;
    }

    public static string GetPacket(int questId, int selectedQuestStepIndex = 0)
    {
        return PacketManager.ComponentBuilder(PacketHeader, PacketVersion, questId, selectedQuestStepIndex);
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
        var embed = _embedBuilderService.CreateEmbedBuilder(EmbedCategory.Quests, BotTranslations.Embed_Quest_Author)
            .WithTitle($"{_questData.Name} ({_questData.Id}) {Emojis.Quest(_questData.Repeatable, _questData.Account)}{(_questData.HasDungeon ? Emojis.Dungeon : string.Empty)}");

        if (_questStepData is not null)
        {
            embed.WithDescription($"{Formatter.Bold(_questStepData.Name)}\n{(string.IsNullOrEmpty(_questStepData.Description) ? string.Empty : Formatter.Italic(_questStepData.Description))}");

            var optimalLevel = _questStepData.OptimalLevel;
            if (optimalLevel > 0)
            {
                embed.AddField(BotTranslations.Embed_Field_OptimalLevel_Title, optimalLevel.ToString());
            }

            if (_dialogQuestionData is not null)
            {
                embed.AddField(BotTranslations.Embed_Field_Dialog_Title, _dialogQuestionData.Message);
            }

            if (_questStepData.QuestObjectives.Count > 0)
            {
                embed.AddQuestObjectiveFields(_questStepData.QuestObjectives);
            }

            if (_questStepData.HasReward())
            {
                StringBuilder rewardsBuilder = new();

                if (_questStepData.RewardsData.Experience > 0)
                {
                    rewardsBuilder.Append(_questStepData.RewardsData.Experience.ToFormattedString());
                    rewardsBuilder.Append(' ');
                    rewardsBuilder.Append(Emojis.Xp);
                    rewardsBuilder.Append('\n');
                }

                if (_questStepData.RewardsData.Kamas > 0)
                {
                    rewardsBuilder.Append(_questStepData.RewardsData.Kamas.ToFormattedString());
                    rewardsBuilder.Append(' ');
                    rewardsBuilder.Append(Emojis.Kamas);
                    rewardsBuilder.Append('\n');
                }

                var itemsReward = _questStepData.RewardsData.GetItemsDataQuantities();
                if (itemsReward.Count > 0)
                {
                    rewardsBuilder.Append(string.Join(", ", itemsReward.Select(x => $"{Formatter.Bold(x.Value.ToString())}x {x.Key.Name}")));
                    rewardsBuilder.Append('\n');
                }

                var emotesReward = _questStepData.RewardsData.GetEmotesData().ToList();
                if (emotesReward.Count > 0)
                {
                    rewardsBuilder.Append(BotTranslations.Embed_Field_Rewards_Content_Emotes);
                    rewardsBuilder.Append(' ');
                    rewardsBuilder.Append(string.Join(", ", emotesReward.Select(x => x.Name)));
                    rewardsBuilder.Append('\n');
                }

                var jobsReward = _questStepData.RewardsData.GetJobsData().ToList();
                if (jobsReward.Count > 0)
                {
                    rewardsBuilder.Append(BotTranslations.Embed_Field_Rewards_Content_Jobs);
                    rewardsBuilder.Append(' ');
                    rewardsBuilder.Append(string.Join(", ", jobsReward.Select(x => x.Name)));
                    rewardsBuilder.Append('\n');
                }

                var spellsReward = _questStepData.RewardsData.GetSpellsData().ToList();
                if (emotesReward.Count > 0)
                {
                    rewardsBuilder.Append(BotTranslations.Embed_Field_Rewards_Content_Spells);
                    rewardsBuilder.Append(' ');
                    rewardsBuilder.Append(string.Join(", ", spellsReward.Select(x => x.Name)));
                    rewardsBuilder.Append('\n');
                }

                if (rewardsBuilder.Length > 0)
                {
                    embed.AddField(BotTranslations.Embed_Field_Rewards_Title, rewardsBuilder.ToString());
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
                    $"{BotTranslations.Select_QuestStep_Content_Step} {i + 1}",
                    GetPacket(_questData.Id, i),
                    StringExtensions.WithMaxLength(_questStepsData[i].Name, 100),
                    isDefault: i == _selectedQuestStepIndex);
            }
        }

        return new(PacketManager.SelectComponentBuilder(selectIndex), BotTranslations.Select_QuestStep_Placeholder, OptionsGenerator(startIndex));
    }
}
