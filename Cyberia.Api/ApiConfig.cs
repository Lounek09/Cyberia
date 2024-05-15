using Cyberia.Langzilla.Enums;

namespace Cyberia.Api;

public sealed class ApiConfig
{
    public string CdnUrl { get; init; }
    public LangType Type { get; init; }

    public ApiConfig()
    {
        CdnUrl = string.Empty;
    }
}
