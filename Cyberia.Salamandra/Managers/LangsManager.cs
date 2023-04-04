using Cyberia.Langzilla;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Managers
{
    public static class LangsManager
    {
        public static async void OnCheckLangFinished(object? sender, CheckLangFinishedEventArgs e)
        {
            if (e.UpdatedLangs.Count == 0)
                return;

            DiscordChannel? channel = await Bot.Instance.Config.GetLangChannel();
            if (channel is null)
            {
                Bot.Instance.Logger.Error($"Unknown lang channel (id:{Bot.Instance.Config.LangChannelId})");
                return;
            }

            DiscordThreadChannel thread = await channel.CreateThreadAsync($"{e.Type} {e.Language} {DateTime.Now:dd-MM-yyyy HH\\hmm}", AutoArchiveDuration.Hour, ChannelType.PublicThread);
            foreach (Lang lang in e.UpdatedLangs)
            {
                Task rateLimite = Task.Delay(1000);

                DiscordMessageBuilder message = new DiscordMessageBuilder()
                    .WithContent($"{(lang.IsNew ? $"{Formatter.Bold("New")} lang" : "Lang")} {Formatter.Bold(lang.Name)} version {Formatter.Bold(lang.Version.ToString())}");

                if (File.Exists(lang.GetDiffFilePath()))
                {
                    using FileStream fileStream = File.OpenRead(lang.GetDiffFilePath());
                    await thread.SendMessageAsync(message.AddFile($"{lang.Name}.as", fileStream));
                }
                else
                    await thread.SendMessageAsync(message);

                await rateLimite;
            }
        }
    }
}
