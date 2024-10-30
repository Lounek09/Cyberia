using Cyberia.Langzilla;
using Cyberia.Langzilla.Enums;
using Cyberia.Langzilla.EventArgs;
using Cyberia.Langzilla.Models;
using Cyberia.Salamandra.Extensions.DSharpPlus;

using DSharpPlus;
using DSharpPlus.Entities;

using System.Text;

namespace Cyberia.Salamandra.Services;

/// <summary>
/// Represents a service to handle langs events and logic.
/// </summary>
public sealed class LangsService
{
    private readonly CachedChannelsService _cachedChannelsService;
    private readonly LangsWatcher _langsWatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="LangsService"/> class.
    /// </summary>
    /// <param name="cachedChannelsService">The service to get the cached channels from.</param>
    /// <param name="langsWatcher">The watcher to get the langs from.</param>
    public LangsService(CachedChannelsService cachedChannelsService, LangsWatcher langsWatcher)
    {
        _cachedChannelsService = cachedChannelsService;
        _langsWatcher = langsWatcher;
    }

    /// <summary>
    /// Launches a manual diff between two types of langs for the specified language, if the types are equal, it will send the last auto diff.
    /// </summary>
    /// <param name="currentType">The current type of langs.</param>
    /// <param name="modelType">The model type of langs.</param>
    /// <param name="language">The language of the langs.</param>
    public async Task LaunchManualDiff(LangType currentType, LangType modelType, Language language)
    {
        var forum = _cachedChannelsService.LangsForumChannel;
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

    /// <summary>
    /// Handles the event when the check of langs is finished.
    /// </summary>
    /// <param name="_">Ignored.</param>
    /// <param name="eventArgs">The event arguments.</param>
    internal async ValueTask OnCheckLangsFinished(LangsWatcher _, CheckLangFinishedEventArgs eventArgs)
    {
        if (eventArgs.UpdatedLangs.Count == 0)
        {
            return;
        }

        var forum = _cachedChannelsService.LangsForumChannel;
        if (forum is null)
        {
            return;
        }

        var thread = await CreateAutoDiffPostAsync(forum, eventArgs.Repository);

        foreach (var updatedLang in eventArgs.UpdatedLangs)
        {
            var delay = Task.Delay(1000);

            await SendAutoDiffLangMessageAsync(thread, updatedLang);

            await delay;
        }
    }

    /// <summary>
    /// Creates a forum post for the auto diff of langs.
    /// </summary>
    /// <param name="forum">The forum channel where to create the post.</param>
    /// <param name="repository">The repository of langs diff.</param>
    /// <returns>The created thread channel.</returns>
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

        var post = await forum.CreateForumPostAsync(postBuilder);
        return post.Channel;
    }

    /// <summary>
    /// Creates a forum post for the manual diff of langs.
    /// </summary>
    /// <param name="forum">The forum channel where to create the post.</param>
    /// <param name="currentRepository">The current repository of the langs diff.</param>
    /// <param name="modelRepository">The model repository of the langs diff.</param>
    /// <returns>The created thread channel.</returns>
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

    /// <summary>
    /// Sends an auto diff message to the specified thread channel.
    /// </summary>
    /// <param name="thread">The thread channel where to send the message.</param>
    /// <param name="lang">The updated lang.</param>
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

    /// <summary>
    /// Sends a manual diff message to the specified thread channel.
    /// </summary>
    /// <param name="thread">The thread channel where to send the message.</param>
    /// <param name="currentLang">The updated lang.</param>
    /// <param name="modelLang">The model lang.</param>
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
