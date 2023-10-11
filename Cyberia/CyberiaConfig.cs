using Cyberia.Salamandra;

namespace Cyberia
{
    public sealed class CyberiaConfig
    {
        public bool EnableCheckCytrus { get; init; }
        public int CheckCytrusInterval { get; init; }

        public bool EnableCheckLang { get; init; }
        public int CheckLangInterval { get; init; }

        public bool EnableCheckBetaLang { get; init; }
        public int CheckBetaLangInterval { get; init; }

        public bool EnableCheckTemporisLang { get; init; }
        public int CheckTemporisLangInterval { get; init; }

        public BotConfig BotConfig { get; init; }

        public CyberiaConfig()
        {
            BotConfig = new();
        }
    }
}
