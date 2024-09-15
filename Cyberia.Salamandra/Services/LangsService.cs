using Cyberia.Langzilla;
using Cyberia.Langzilla.Enums;
using Cyberia.Langzilla.EventArgs;
using Cyberia.Langzilla.Models;
using Cyberia.Salamandra.Extensions.DSharpPlus;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;

using System.Text;

namespace Cyberia.Salamandra.Services;

public sealed class LangsService
{
    private readonly LangsWatcher _langsWatcher;

    public LangsService(LangsWatcher langsWatcher)
    {
        _langsWatcher = langsWatcher;
    }

    public async void OnCheckLangFinished(object? _, CheckLangFinishedEventArgs args)
    {
        if (!args.UpdatedLangs.Any())
        {
            return;
        }

        var forum = ChannelManager.LangForumChannel;
        if (forum is null)
        {
            return;
        }

        var thread = await CreateAutoDiffPostAsync(forum, args.Repository);

        foreach (var updatedLang in args.UpdatedLangs)
        {
            var delay = Task.Delay(1000);

            await SendAutoDiffLangMessageAsync(thread, updatedLang);

            await delay;
        }
    }

    public async Task LaunchManualDiff(LangType currentType, LangType modelType, LangLanguage language)
    {
        var forum = ChannelManager.LangForumChannel;
        if (forum is null)
        {
            return;
        }

        var isSameType = currentType == modelType;

        var currentRepository = _langsWatcher.GetRepository(currentType, language);
        var modelRepository = _langsWatcher.GetRepository(modelType, language);

        var thread = isSameType
            ? await CreateAutoDiffPostAsync(forum, currentRepository)
            : await CreateManualDiffPostAsync(forum, currentRepository, modelRepository);

        foreach (var lang in currentRepository.Langs)
        {
            var rateLimit = Task.Delay(1000);

            if (isSameType)
            {
                await SendAutoDiffLangMessageAsync(thread, lang);
            }
            else
            {
                var langModel = modelRepository.GetByName(lang.Name);
                await SendManualDiffLangMessageAsync(thread, lang, langModel);
            }

            await rateLimit;
        }
    }

    private static async Task<DiscordThreadChannel> CreateAutoDiffPostAsync(DiscordForumChannel forum, LangsRepository repository)
    {
        var lastChange = repository.LastChange.ToLocalTime();
        var type = repository.Type.ToStringFast();
        var language = repository.Language.ToStringFast();

        var postBuilder = new ForumPostBuilder()
            .WithName($"{type} {language} {DateTime.Now:yyyy-MM-dd HH:mm}")
            .WithMessage(new DiscordMessageBuilder().WithContent(
                $"Diff of langs {Formatter.Bold(type)} from {lastChange:yyyy-MM-dd HH:mmzzz} in {Formatter.Bold(language)}"));

        var typeTag = forum.GetDiscordForumTagByName(type);
        if (typeTag is not null)
        {
            postBuilder.AddTag(typeTag);
        }

        var languageTag = forum.GetDiscordForumTagByName(language);
        if (languageTag is not null)
        {
            postBuilder.AddTag(languageTag);
        }

        while (true)
        {
            try
            {
                var post = await forum.CreateForumPostAsync(postBuilder);
                return post.Channel;
            }
            catch (RateLimitException)
            {
                await Task.Delay(1000);
            }
        }
    }

    private static async Task<DiscordThreadChannel> CreateManualDiffPostAsync(DiscordForumChannel forum, LangsRepository currentRepository, LangsRepository modelRepository)
    {
        var currentLastChange = currentRepository.LastChange.ToLocalTime();
        var currentType = currentRepository.Type.ToStringFast();
        var currentLanguage = currentRepository.Language.ToStringFast();

        var modelLastChange = modelRepository.LastChange.ToLocalTime();
        var modelType = modelRepository.Type.ToStringFast();

        var postBuilder = new ForumPostBuilder()
            .WithName($"Diff {currentType} ➜ {modelType} in {currentLanguage} {DateTime.Now:yyyy-MM-dd HH:mm}")
            .WithMessage(new DiscordMessageBuilder().WithContent(
                $"Diff of langs {Formatter.Bold(currentType)} from {currentLastChange:yyyy-MM-dd HH:mmzzz} " +
                $"and {Formatter.Bold(modelType)} from {modelLastChange:yyyy-MM-dd HH:mmzzz} in {Formatter.Bold(currentLanguage)}"));

        var tag = forum.GetDiscordForumTagByName(DiscordForumChannelExtensions.ManualDiffTagName);
        if (tag is not null)
        {
            postBuilder.AddTag(tag);
        }

        var languageTag = forum.GetDiscordForumTagByName(currentLanguage);
        if (languageTag is not null)
        {
            postBuilder.AddTag(languageTag);
        }

        var post = await forum.CreateForumPostAsync(postBuilder);
        return post.Channel;
    }

    private static async Task SendAutoDiffLangMessageAsync(DiscordThreadChannel thread, Lang lang)
    {
        var message = new DiscordMessageBuilder()
            .WithContent(
                $"{(lang.New ? $"{Formatter.Bold("New")} lang" : "Lang")} " +
                $"{Formatter.Bold(lang.Name)} version {Formatter.Bold(lang.Version.ToString())}");

        var diffFilePath = lang.DiffFilePath;
        if (!File.Exists(diffFilePath))
        {
            await thread.SendMessageSafeAsync(message);
            return;
        }

        using var fileStream = File.OpenRead(diffFilePath);
        await thread.SendMessageSafeAsync(message.AddFile($"{lang.Name}.as", fileStream));
    }

    private static async Task SendManualDiffLangMessageAsync(DiscordThreadChannel thread, Lang currentLang, Lang? modelLang)
    {
        var message = new DiscordMessageBuilder()
            .WithContent(
                $"{(currentLang.New ? $"{Formatter.Bold("New")} lang" : "Lang")} " +
                $"{Formatter.Bold(currentLang.Name)} version {Formatter.Bold(currentLang.Version.ToString())}" +
                (modelLang is null ? $", not present in langs {modelLang}" : string.Empty));

        var diff = currentLang.Diff(modelLang);
        if (string.IsNullOrEmpty(diff))
        {
            await thread.SendMessageSafeAsync(message);
            return;
        }

        using MemoryStream memoryStream = new(Encoding.UTF8.GetBytes(diff));
        await thread.SendMessageSafeAsync(message.AddFile($"{currentLang.Name}.as", memoryStream));
    }
}
