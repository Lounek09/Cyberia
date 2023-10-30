using Cyberia.Langzilla;
using Cyberia.Langzilla.Enums;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Managers
{
    public static class LangsManager
    {
        public static async void OnCheckLangFinished(object? _, CheckLangFinishedEventArgs e)
        {
            if (e.UpdatedLangs.Count == 0)
            {
                return;
            }

            DiscordForumChannel? forum = await GetLangForumChannel();
            if (forum is null)
            {
                return;
            }

            DiscordThreadChannel thread = await CreateThreadAsync(forum, e.Type, e.Language);

            foreach (Lang updatedLang in e.UpdatedLangs)
            {
                Task delay = Task.Delay(1000);

                await SendLangMessageAsync(thread, updatedLang);

                await delay;
            }
        }

        private static async Task<DiscordForumChannel?> GetLangForumChannel()
        {
            ulong id = Bot.Config.LangForumChannelId;
            if (id == 0)
            {
                return null;
            }

            try
            {
                DiscordChannel channel = await Bot.Client.GetChannelAsync(id);
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

        private static async Task<DiscordThreadChannel> CreateThreadAsync(DiscordForumChannel forum, LangType type, Language language)
        {
            DateTime now = DateTime.Now;

            ForumPostBuilder postBuilder = new ForumPostBuilder()
                .WithName($"{type} {language} {now:dd-MM-yyyy HH\\hmm}")
                .WithMessage(new DiscordMessageBuilder().WithContent($"Diff des langs {Formatter.Bold(type.ToString())} de {now:HH\\hmm} le {now:dd/MM/yyyy} en {Formatter.Bold(language.ToString())}"));

            DiscordForumTag? typeTag = GetDiscordForumTagByName(forum, type.ToString());
            if (typeTag is not null)
            {
                postBuilder.AddTag(typeTag);
            }

            DiscordForumTag? languageTag = GetDiscordForumTagByName(forum, language.ToString());
            if (languageTag is not null)
            {
                postBuilder.AddTag(languageTag);
            }

            DiscordForumPostStarter post = await forum.CreateForumPostAsync(postBuilder);

            return post.Channel;
        }

        private static DiscordForumTag? GetDiscordForumTagByName(DiscordForumChannel forum, string name)
        {
            return forum.AvailableTags.FirstOrDefault(x => x.Name.Equals(name));
        }

        private static async Task<DiscordMessage> SendLangMessageAsync(DiscordThreadChannel thread, Lang lang)
        {
            DiscordMessageBuilder message = new DiscordMessageBuilder()
                    .WithContent($"{(lang.IsNew ? $"{Formatter.Bold("New")} lang" : "Lang")} {Formatter.Bold(lang.Name)} version {Formatter.Bold(lang.Version.ToString())}");

            string diffFilePath = lang.GetDiffFilePath();
            if (!File.Exists(diffFilePath))
            {
                return await thread.SendMessageAsync(message);
            }

            using FileStream fileStream = File.OpenRead(diffFilePath);
            return await thread.SendMessageAsync(message.AddFile($"{lang.Name}.as", fileStream));
        }
    }
}
