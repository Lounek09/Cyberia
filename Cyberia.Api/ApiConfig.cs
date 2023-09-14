namespace Cyberia.Api
{
    public sealed class ApiConfig
    {
        public string CdnUrl { get; init; }
        public bool Temporis { get; init; }
        public FormatType FormatType { get; init; }

        public ApiConfig()
        {
            CdnUrl = string.Empty;
        }
    }
}
