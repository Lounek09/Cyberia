using DSharpPlus;
using DSharpPlus.Entities;

using Salamandra.Bot.Manager;
using Salamandra.Utils;

namespace Salamandra.Script
{
    public static class CytrusManager
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Needed for the timer to work properly")]
        private static Timer? _timer = null;
        private static readonly HttpClient _httpClient = new();

        public static void Listen()
        {
            if (Salamandra.Config.EnableCheckCytrus)
                _timer = new Timer(async _ => await CheckCytrus(), null, 10000, 60000);
        }

        public static async Task CheckCytrus()
        {
            if (Directory.Exists(Constant.CYTRUS_PATH))
                Directory.CreateDirectory(Constant.CYTRUS_PATH);

            string cytrus = "";
            try
            {
                using (HttpResponseMessage response = await _httpClient.GetAsync(Constant.CYTRUS_URL))
                {
                    response.EnsureSuccessStatusCode();
                    cytrus = await response.Content.ReadAsStringAsync();
                }
            }
            catch (HttpRequestException e)
            {
                Salamandra.Logger.Error(e);
                return;
            }

            if (string.IsNullOrEmpty(cytrus))
                return;

            string lastCytrus = "{}";
            if (File.Exists(Constant.CYTRUS_PATH + "/cytrus.json"))
                lastCytrus = File.ReadAllText(Constant.CYTRUS_PATH + "/cytrus.json");

            string diff = Json.Diff(cytrus, lastCytrus);

            if (string.IsNullOrEmpty(diff))
                return;

            Salamandra.Logger.Info("Cytrus update detected :\n" + diff.ToString());

            if (File.Exists(Constant.CYTRUS_PATH + "/cytrus.json"))
                File.Move(Constant.CYTRUS_PATH + "/cytrus.json", Constant.CYTRUS_PATH + "/cytrus_" + DateTime.Now.ToString("MMddyyyyHHmm") + ".json");
            File.WriteAllText(Constant.CYTRUS_PATH + "/cytrus.json", cytrus);

            DiscordChannel? channel = await Salamandra.Bot.Config.GetCytrusChannel();
            if (channel is not null)
                await MessageManager.SendMessage(channel, new DiscordMessageBuilder().WithContent(Formatter.BlockCode(diff, "json")));
        }
    }
}
