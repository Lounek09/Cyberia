using Cyberia.Langzilla;
using Cyberia.Langzilla.Enums;
using Cyberia.Langzilla.EventArgs;
using Cyberia.Salamandra.DsharpPlus;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;

using System.Text;

namespace Cyberia.Salamandra.Managers;

public static class LangManager
{
	public static async void OnCheckLangFinished(object? _, CheckLangFinishedEventArgs args)
    {
        if (args.UpdatedLangs.Count == 0)
        {
            return;
        }

        var forum = ChannelManager.LangForumChannel;
        if (forum is null)
        {
            return;
        }

        var thread = await forum.CreateAutoDiffPostAsync(args.Repository);

        foreach (var updatedLang in args.UpdatedLangs)
        {
            var delay = Task.Delay(1000);

            await thread.SendAutoDiffLangMessageAsync(updatedLang);

            await delay;
        }
    }

	public static async Task LaunchManualDiff(LangType currentType, LangType modelType, LangLanguage language)
	{
		var forum = ChannelManager.LangForumChannel;
		if (forum is null)
		{
			return;
		}

        var isSameType = currentType == modelType;

        var currentRepository = LangsWatcher.LangRepositories[(currentType, language)];
        var modelRepository = LangsWatcher.LangRepositories[(modelType, language)];

		var thread = isSameType
            ? await forum.CreateAutoDiffPostAsync(currentRepository)
            : await forum.CreateManualDiffPostAsync(currentRepository, modelRepository);

		foreach (var lang in currentRepository.Langs)
		{
			var rateLimit = Task.Delay(1000);

            if (isSameType)
            {
                await thread.SendAutoDiffLangMessageAsync(lang);
            }
            else
            {
                var langModel = modelRepository.GetByName(lang.Name);
                await thread.SendManualDiffLangMessageAsync(lang, langModel);
            }

			await rateLimit;
		}
	}

    private static async Task<DiscordThreadChannel> CreateAutoDiffPostAsync(this DiscordForumChannel forum, LangRepository repository)
    {
        var now = DateTime.Now;

        var postBuilder = new ForumPostBuilder()
            .WithName($"{repository.Type} {repository.Language} {now:dd-MM-yyyy HH\\hmm}")
            .WithMessage(new DiscordMessageBuilder().WithContent(
                $"Diff des langs {Formatter.Bold(repository.Type.ToString())} " +
                $"de {repository.LastChange.ToLocalTime():dd/MM/yyyy HH:mmzzz} " +
                $"en {Formatter.Bold(repository.Language.ToString())}"));

        var typeTag = forum.GetDiscordForumTagByName(repository.Type.ToString());
        if (typeTag is not null)
        {
            postBuilder.AddTag(typeTag);
        }

        var languageTag = forum.GetDiscordForumTagByName(repository.Language.ToString());
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

    private static async Task<DiscordThreadChannel> CreateManualDiffPostAsync(this DiscordForumChannel forum, LangRepository currentRepository, LangRepository modelRepository)
    {
		var postBuilder = new ForumPostBuilder()
			.WithName($"Diff {currentRepository.Type} -> {modelRepository.Type} en {currentRepository.Language} {DateTime.Now:dd-MM-yyyy HH\\hmm}")
			.WithMessage(new DiscordMessageBuilder().WithContent(
				$"Diff des langs {Formatter.Bold(currentRepository.Type.ToString())} de {currentRepository.LastChange.ToLocalTime():dd/MM/yyyy HH:mmzzz} " +
				$"et {Formatter.Bold(modelRepository.Type.ToString())} de {modelRepository.LastChange.ToLocalTime():dd/MM/yyyy HH:mmzzz} " +
				$"en {Formatter.Bold(currentRepository.Language.ToString())}"));

		var tag = ChannelManager.LangForumChannel!.GetDiscordForumTagByName("Diff manuel");
		if (tag is not null)
		{
			postBuilder.AddTag(tag);
		}

		var post = await forum.CreateForumPostAsync(postBuilder);
		return post.Channel;
	}

    private static async Task SendManualDiffLangMessageAsync(this DiscordThreadChannel thread, Lang currentLang, Lang? modelLang)
    {
		var message = new DiscordMessageBuilder()
			.WithContent(
				$"{(currentLang.New ? $"{Formatter.Bold("New")} lang" : "Lang")} " +
				$"{Formatter.Bold(currentLang.Name)} version {Formatter.Bold(currentLang.Version.ToString())}" +
				(modelLang is null ? $", non présent dans les langs {modelLang}" : string.Empty));

		var diff = currentLang.Diff(modelLang);
		if (string.IsNullOrEmpty(diff))
		{
			await thread.SendMessageAsync(message);
            return;
		}

		using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(diff));
		await thread.SendMessageAsync(message.AddFile($"{currentLang.Name}.as", memoryStream));
	}

    private static async Task SendAutoDiffLangMessageAsync(this DiscordThreadChannel thread, Lang lang)
    {
        var message = new DiscordMessageBuilder()
            .WithContent(
                $"{(lang.New ? $"{Formatter.Bold("New")} lang" : "Lang")} " +
                $"{Formatter.Bold(lang.Name)} version {Formatter.Bold(lang.Version.ToString())}");

        var diffFilePath = lang.DiffFilePath;
        if (!File.Exists(diffFilePath))
        {
            await thread.SendMessageAsync(message);
            return;
        }

        using var fileStream = File.OpenRead(diffFilePath);
        await thread.SendMessageAsync(message.AddFile($"{lang.Name}.as", fileStream));
    }
}
