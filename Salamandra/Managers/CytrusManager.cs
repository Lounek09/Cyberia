using DSharpPlus;
using DSharpPlus.Entities;

using Salamandra.Bot.Managers;
using Salamandra.Cytrus;

namespace Salamandra.Managers
{
    public static class CytrusManager
    {
        private static Dictionary<string, Timer> _timer = new();

        public static void Listen()
        {
            _timer = new();

            if (Salamandra.Config.EnableCheckCytrus)
                _timer.Add("Cytrus", new Timer(async _ => await Salamandra.Cytrus.Launch(), null, 10000, 60000));
        }

        public static async void OnNewCytrusDetected(object? sender, NewCytrusDetectedEventArgs e)
        {
            DiscordChannel? channel = await Salamandra.Bot.Config.GetCytrusChannel();
            if (channel is not null)
                await MessageManager.SendMessage(channel, new DiscordMessageBuilder().WithContent(Formatter.BlockCode(e.Diff, "json")));
        }
    }
}
