using Cyberia.Salamandra;

namespace Cyberia
{
    public sealed class CyberiaConfig
    {
        public bool EnableCheckLang { get; init; }
        public bool EnableCheckBetaLang { get; init; }
        public bool EnableCheckTemporisLang { get; init; }
        public bool EnableCheckCytrus { get; init; }
        public BotConfig BotConfig { get; init; }

        public CyberiaConfig()
        {
            BotConfig = new();
        }
    }
}
