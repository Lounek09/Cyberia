using DSharpPlus;
using DSharpPlus.Entities;

using Salamandra.Bot.Managers;
using Salamandra.Langs;
using Salamandra.Langs.Enums;

namespace Salamandra.Managers
{
    public static class LangsManager
    {
        private static Dictionary<string, Timer> _timer = new();

        public static void Listen()
        {
            _timer = new();

            foreach (Language language in Enum.GetValues<Language>())
            {
                if (Salamandra.Config.EnableCheckLang)
                    _timer.Add($"{LangType.Official}_{language}", new Timer(async _ => await Salamandra.Langs.Launch(LangType.Official, language), null, 10000, 360000));

                if (Salamandra.Config.EnableCheckBetaLang)
                    _timer.Add($"{LangType.Beta}_{language}", new Timer(async _ => await Salamandra.Langs.Launch(LangType.Beta, language), null, 130000, 360000));

                if (Salamandra.Config.EnableCheckTemporisLang)
                    _timer.Add($"{LangType.Temporis}_{language}", new Timer(async _ => await Salamandra.Langs.Launch(LangType.Temporis, language), null, 250000, 360000));
            }
        }

        public static async void OnCheckLangFinished(object? sender, CheckLangFinishedEventArgs e)
        {
            if (e.Langs.Count == 0)
                return;

            DiscordChannel? channel = await Salamandra.Bot.Config.GetLangChannel();
            DiscordThreadChannel? thread = null;
            if (channel is null)
                Salamandra.Logger.Error($"Unknown lang channel (id:{Salamandra.Bot.Config.LangChannelId})");
            else
                thread = await channel.CreateThreadAsync($"{e.Type} {e.Language} {DateTime.Now:dd-MM-yyyy HH\\hmm}", AutoArchiveDuration.Hour, ChannelType.PublicThread);

            foreach (Lang lang in e.Langs)
            {
                Task rateLimite = Task.Delay(1500);

                if (!await lang.Download())
                {
                    Salamandra.Logger.Error($"Download of lang {lang.Name} version {lang.Version} in {lang.Language} failed");
                    return;
                }

                Salamandra.Logger.Info($"{(lang.IsNew ? "[NEW] " : "")}Lang {lang.Name} version {lang.Version} in {lang.Language} downloaded");

                if (!Flare.ExtractLang(lang))
                {
                    Salamandra.Logger.Error($"Extract of lang {lang.Name} version {lang.Version} in {lang.Language} failed");
                    return;
                }

                if (!Salamandra.Langs.DiffLastExtractedLang(lang))
                {
                    Salamandra.Logger.Error($"Diff of lang {lang.Name} version {lang.Version} in {lang.Language} failed");
                    return;
                }

                if (thread is not null)
                {
                    DiscordMessageBuilder message = new DiscordMessageBuilder()
                        .WithContent($"{(lang.IsNew ? $"{Formatter.Bold("New")} lang" : "Lang")} {Formatter.Bold(lang.Name)} version {Formatter.Bold(lang.Version.ToString())}");

                    FileStream? fileStream = null;
                    if (File.Exists($"{lang.DirectoryPath}/diff.as"))
                    {
                        fileStream = File.OpenRead($"{lang.DirectoryPath}/diff.as");
                        message.WithFile("diff.as", fileStream);
                    }

                    await MessageManager.SendMessage(thread, message, fileStream);
                    await rateLimite;
                }
            }
        }
    }
}
