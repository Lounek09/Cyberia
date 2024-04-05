using Cyberia.Langzilla;
using Cyberia.Langzilla.Enums;
using Cyberia.Langzilla.EventArgs;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Managers;

public static class LangsManager
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

        var thread = await CreateThreadAsync(forum, args.Type, args.Language);

        foreach (var updatedLang in args.UpdatedLangs)
        {
            var delay = Task.Delay(1000);

            await SendLangMessageAsync(thread, updatedLang);

            await delay;
        }
    }

    private static async Task<DiscordThreadChannel> CreateThreadAsync(DiscordForumChannel forum, LangType type, LangLanguage language)
    {
        var now = DateTime.Now;

        var postBuilder = new ForumPostBuilder()
            .WithName($"{type} {language} {now:dd-MM-yyyy HH\\hmm}")
            .WithMessage(new DiscordMessageBuilder().WithContent($"Diff des langs {Formatter.Bold(type.ToString())} de {now:HH\\hmm} le {now:dd/MM/yyyy} en {Formatter.Bold(language.ToString())}"));

        var typeTag = GetDiscordForumTagByName(forum, type.ToString());
        if (typeTag is not null)
        {
            postBuilder.AddTag(typeTag);
        }

        var languageTag = GetDiscordForumTagByName(forum, language.ToString());
        if (languageTag is not null)
        {
            postBuilder.AddTag(languageTag);
        }

        var post = await forum.CreateForumPostAsync(postBuilder);
        return post.Channel;
    }

    private static DiscordForumTag? GetDiscordForumTagByName(DiscordForumChannel forum, string name)
    {
        return forum.AvailableTags.FirstOrDefault(x => x.Name.Equals(name));
    }

    private static async Task<DiscordMessage> SendLangMessageAsync(DiscordThreadChannel thread, Lang lang)
    {
        var message = new DiscordMessageBuilder()
            .WithContent($"{(lang.New ? $"{Formatter.Bold("New")} lang" : "Lang")} {Formatter.Bold(lang.Name)} version {Formatter.Bold(lang.Version.ToString())}");

        var diffFilePath = lang.DiffFilePath;
        if (!File.Exists(diffFilePath))
        {
            return await thread.SendMessageAsync(message);
        }

        using var fileStream = File.OpenRead(diffFilePath);
        return await thread.SendMessageAsync(message.AddFile($"{lang.Name}.as", fileStream));
    }
}
