using Cyberia.Api;
using Cyberia.Salamandra;

namespace Cyberia
{
    public sealed class CyberiaConfig
    {
        public bool EnableCheckCytrus { get; init; }
        public TimeSpan CheckCytrusInterval { get; init; }

        public bool EnableCheckLang { get; init; }
        public TimeSpan CheckLangInterval { get; init; }

        public bool EnableCheckBetaLang { get; init; }
        public TimeSpan CheckBetaLangInterval { get; init; }

        public bool EnableCheckTemporisLang { get; init; }
        public TimeSpan CheckTemporisLangInterval { get; init; }

        public ApiConfig ApiConfig { get; init; }
        public BotConfig BotConfig { get; init; }

        public CyberiaConfig()
        {
            ApiConfig = new();
            BotConfig = new();
        }
    }
}
