using Cyberia.Langs;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Managers
{
    public static class LangsManager
    {
        public static async void OnCheckLangFinished(object? sender, CheckLangFinishedEventArgs e)
        {
            if (e.Langs.Count == 0)
                return;

            //Create a thread in discord
            DiscordChannel? channel = await Cyberia.Salamandra.Config.GetLangChannel();
            DiscordThreadChannel? thread = null;
            if (channel is null)
                Cyberia.Salamandra.Logger.Error($"Unknown lang channel (id:{Cyberia.Salamandra.Config.LangChannelId})");
            else
                thread = await channel.CreateThreadAsync($"{e.Type} {e.Language} {DateTime.Now:dd-MM-yyyy HH\\hmm}", AutoArchiveDuration.Hour, ChannelType.PublicThread);

            //Download, extract and diff each new lang and send to discord in the previously created thread
            foreach (Lang lang in e.Langs)
            {
                Task rateLimite = Task.Delay(1500);

                if (!await lang.DownloadAsync())
                {
                    Cyberia.Langs.Logger.Error($"Download of lang {lang.Name} version {lang.Version} in {lang.Language} failed");
                    return;
                }

                Cyberia.Langs.Logger.Info($"{(lang.IsNew ? "[NEW] " : "")}Lang {lang.Name} version {lang.Version} in {lang.Language} downloaded");

                if (!Flare.ExtractLang(lang))
                {
                    Cyberia.Langs.Logger.Error($"Extract of lang {lang.Name} version {lang.Version} in {lang.Language} failed");
                    return;
                }

                if (!Cyberia.Langs.DiffLastExtractedLang(lang, out string diffPath))
                {
                    Cyberia.Langs.Logger.Error($"Diff of lang {lang.Name} version {lang.Version} in {lang.Language} failed");
                    return;
                }

                if (thread is not null)
                {
                    DiscordMessageBuilder message = new DiscordMessageBuilder()
                        .WithContent($"{(lang.IsNew ? $"{Formatter.Bold("New")} lang" : "Lang")} {Formatter.Bold(lang.Name)} version {Formatter.Bold(lang.Version.ToString())}");

                    try
                    {
                        //if the file doesn't exist it means that there is no difference
                        using (FileStream fileStream = File.OpenRead(diffPath))
                            await thread.SendMessageAsync(message.AddFile($"{lang.Name}.as", fileStream));
                    }
                    catch
                    {
                        await thread.SendMessageAsync(message);
                    }

                    await rateLimite;
                }
            }
        }
    }
}
