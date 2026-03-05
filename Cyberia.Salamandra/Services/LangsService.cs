using Cyberia.Database.Models;
using Cyberia.Database.Repositories;
using Cyberia.Langzilla;
using Cyberia.Langzilla.EventArgs;
using Cyberia.Langzilla.Extensions;
using Cyberia.Langzilla.Primitives;
using Cyberia.Salamandra.Extensions.DSharpPlus;

using DSharpPlus;
using DSharpPlus.Entities;

using System.Text;

namespace Cyberia.Salamandra.Services;

/// <summary>
/// Represents a service to handle langs events and logic.
/// </summary>
public interface ILangsService
{
    /// <summary>
    /// Launches a manual diff between two types of langs for the specified language, if the types are equal, it will send the last auto diff.
    /// </summary>
    /// <param name="currentType">The type of the current langs diff.</param>
    /// <param name="modelType">The type of the model langs.</param>
    /// <param name="language">The language of the langs.</param>
    Task LaunchManualDiff(LangType currentType, LangType modelType, Language language);

    /// <summary>
    /// Handles the event when the check of langs is finished.
    /// </summary>
    /// <param name="_">Ignored.</param>
    /// <param name="eventArgs">The event arguments.</param>
    ValueTask OnNewLangsDetected(ILangsWatcher _, NewLangsDetectedEventArgs eventArgs);

    /// <summary>
    /// Handles the event when an error occurs in the Langs watcher.
    /// </summary>
    /// <param name="_">Ignored.</param>
    /// <param name="eventArgs">The event arguments.</param>
    ValueTask OnLangsErrored(ILangsWatcher _, LangsErroredEventArgs eventArgs);
}

public sealed class LangsService : ILangsService
{
    private readonly ICachedChannelsManager _cachedChannelsManager;
    private readonly IEmbedBuilderService _embedBuilderService;
    private readonly LangRepository _langRepository;
    private readonly ILangsWatcher _langsWatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="LangsService"/> class.
    /// </summary>
    public LangsService(ICachedChannelsManager cachedChannelsManager, IEmbedBuilderService embedBuilderService, LangRepository langRepository, ILangsWatcher langsWatcher)
    {
        _cachedChannelsManager = cachedChannelsManager;
        _embedBuilderService = embedBuilderService;
        _langRepository = langRepository;
        _langsWatcher = langsWatcher;
    }

    public async Task LaunchManualDiff(LangType currentType, LangType modelType, Language language)
    {
        var forum = _cachedChannelsManager.LangsForumChannel;
        if (forum is null)
        {
            return;
        }

        var isSameType = currentType == modelType;
        LangsIdentifier currentIdentifier = new(currentType, language);
        LangsIdentifier modelIdentifier = new(modelType, language);

        var currentLangs = await _langRepository.GetManyByIdentifierAsync(currentIdentifier);
        var modelLangs = await _langRepository.GetManyByIdentifierAsync(modelIdentifier);

        var thread = isSameType
            ? await CreateAutoDiffPostAsync(forum, currentIdentifier)
            : await CreateManualDiffPostAsync(forum, currentIdentifier, modelIdentifier);

        foreach (var currentLang in currentLangs)
        {
            var rateLimit = Task.Delay(1000);

            if (isSameType)
            {
                await SendAutoDiffLangMessageAsync(thread, currentLang);
            }
            else
            {
                var modelLang = modelLangs.FirstOrDefault(x => x.Name.Equals(currentLang.Name, StringComparison.OrdinalIgnoreCase));
                await SendManualDiffLangMessageAsync(thread, currentLang, modelLang);
            }

            await rateLimit;
        }
    }

    public async ValueTask OnNewLangsDetected(ILangsWatcher _, NewLangsDetectedEventArgs eventArgs)
    {
        var forum = _cachedChannelsManager.LangsForumChannel;
        if (forum is null)
        {
            return;
        }

        var thread = await CreateAutoDiffPostAsync(forum, eventArgs.Identifier);

        foreach (var updatedLang in eventArgs.UpdatedLangs)
        {
            var delay = Task.Delay(1000);

            await SendAutoDiffLangMessageAsync(thread, updatedLang);

            await delay;
        }
    }

    public async ValueTask OnLangsErrored(ILangsWatcher _, LangsErroredEventArgs eventArgs)
    {
        var embed = _embedBuilderService.CreateErrorEmbedBuilder(
            "An error occurred while checking for new Langs version",
            eventArgs.ErrorMessage,
            eventArgs.Exception);

        await _cachedChannelsManager.SendErrorMessage(embed);
    }

    /// <summary>
    /// Creates a forum post for the auto diff of langs.
    /// </summary>
    /// <param name="forum">The forum channel where to create the post.</param>
    /// <param name="identifier">The identifier of the langs diff.</param>
    /// <returns>The created thread channel.</returns>
    private async Task<DiscordThreadChannel> CreateAutoDiffPostAsync(DiscordForumChannel forum, LangsIdentifier identifier)
    {
        var lastModified = _langsWatcher.LastModifieds[identifier].ToLocalTime();
        var type = identifier.Type.ToStringFast();
        var language = identifier.Language.ToStringFast();

        var postBuilder = new ForumPostBuilder()
            .WithName($"Diff of {type} in {language} - {DateTime.Now:yyyy-MM-dd HH:mm}")
            .WithMessage(new DiscordMessageBuilder().WithContent(
                $"Diff of langs {Formatter.Bold(type)} from {lastModified:yyyy-MM-dd HH:mmzzz} in {Formatter.Bold(language)}"));

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
    /// <param name="currentIdentifier">The identifier of the current langs diff.</param>
    /// <param name="modelIdentifier">The identifier of the model langs.</param>
    /// <returns>The created thread channel.</returns>
    private async Task<DiscordThreadChannel> CreateManualDiffPostAsync(DiscordForumChannel forum, LangsIdentifier currentIdentifier, LangsIdentifier modelIdentifier)
    {
        var currentLastModified = _langsWatcher.LastModifieds[currentIdentifier].ToLocalTime();
        var currentType = currentIdentifier.Type.ToStringFast();

        var modelLastModified = _langsWatcher.LastModifieds[modelIdentifier].ToLocalTime();
        var modelType = modelIdentifier.Type.ToStringFast();

        var language = currentIdentifier.Language.ToStringFast();

        var postBuilder = new ForumPostBuilder()
            .WithName($"Diff of {currentType} compared to {modelType} in {language} - {DateTime.Now:yyyy-MM-dd HH:mm}")
            .WithMessage(new DiscordMessageBuilder().WithContent(
                $"Diff of langs {Formatter.Bold(currentType)} from {currentLastModified:yyyy-MM-dd HH:mmzzz} " +
                $"compared to {Formatter.Bold(modelType)} from {modelLastModified:yyyy-MM-dd HH:mmzzz} in {Formatter.Bold(language)}"));

        var tag = forum.GetDiscordForumTagByName(DiscordForumChannelExtensions.ManualDiffTagName);
        if (tag is not null)
        {
            postBuilder.AddTag(tag);
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
    /// Sends an auto diff message to the specified thread channel.
    /// </summary>
    /// <param name="thread">The thread channel where to send the message.</param>
    /// <param name="lang">The lang diff.</param>
    private static async Task SendAutoDiffLangMessageAsync(DiscordThreadChannel thread, Lang lang)
    {
        var message = new DiscordMessageBuilder()
            .WithContent(
                $"{(lang.IsNew ? $"{Formatter.Bold("New")} lang" : "Lang")} " +
                $"{Formatter.Bold(lang.Name)} version {Formatter.Bold(lang.Version.ToString())}");

        var diffFilePath = lang.GetDiffFilePath();
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
    /// <param name="lang">The current lang diff.</param>
    /// <param name="modelLang">The model lang.</param>
    private static async Task SendManualDiffLangMessageAsync(DiscordThreadChannel thread, Lang lang, Lang? modelLang)
    {
        var message = new DiscordMessageBuilder()
            .WithContent(
                $"{(lang.IsNew ? $"{Formatter.Bold("New")} lang" : "Lang")} " +
                $"{Formatter.Bold(lang.Name)} version {Formatter.Bold(lang.Version.ToString())}" +
                (modelLang is null ? $", not present in langs {modelLang}" : string.Empty));

        if (modelLang is null)
        {
            await thread.SendMessageSafeAsync(message);
            return;
        }

        var diff = Lang.Diff(lang, modelLang);
        if (string.IsNullOrEmpty(diff))
        {
            await thread.SendMessageSafeAsync(message);
            return;
        }

        using MemoryStream memoryStream = new(Encoding.UTF8.GetBytes(diff));
        await thread.SendMessageSafeAsync(message.AddFile($"{lang.Name}.as", memoryStream));
    }
}
