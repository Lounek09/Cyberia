using Cyberia.Langzilla;
using Cyberia.Langzilla.Enums;
using Cyberia.Langzilla.EventArgs;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Managers;

public static class LangsManager
{
    public static async void OnCheckLangFinished(object? _, CheckLangFinishedEventArgs e)
    {
        if (e.UpdatedLangsData.Count == 0)
        {
            return;
        }

        var forum = await GetLangForumChannel();
        if (forum is null)
        {
            return;
        }

        var thread = await CreateThreadAsync(forum, e.Type, e.Language);

        foreach (var updatedLangData in e.UpdatedLangsData)
        {
            var delay = Task.Delay(1000);

            await SendLangMessageAsync(thread, updatedLangData);

            await delay;
        }
    }

    private static async Task<DiscordForumChannel?> GetLangForumChannel()
    {
        var id = Bot.Config.LangForumChannelId;
        if (id == 0)
        {
            return null;
        }

        try
        {
            var channel = await Bot.Client.GetChannelAsync(id);
            if (channel is DiscordForumChannel forum)
            {
                return forum;
            }

            Log.Error("The given lang channel is not a forum {ChannelId}", id);
        }
        catch
        {
            Log.Error("Unknown lang forum channel {ChannelId}", id);
        }

        return null;
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

    private static async Task<DiscordMessage> SendLangMessageAsync(DiscordThreadChannel thread, LangData langData)
    {
        var message = new DiscordMessageBuilder()
                .WithContent($"{(langData.New ? $"{Formatter.Bold("New")} lang" : "Lang")} {Formatter.Bold(langData.Name)} version {Formatter.Bold(langData.Version.ToString())}");

        var diffFilePath = langData.GetDiffFilePath();
        if (!File.Exists(diffFilePath))
        {
            return await thread.SendMessageAsync(message);
        }

        using var fileStream = File.OpenRead(diffFilePath);
        return await thread.SendMessageAsync(message.AddFile($"{langData.Name}.as", fileStream));
    }
}
