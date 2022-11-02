namespace Salamandra.Langs
{
    public sealed class Lang
    {
        public string Name { get; private set; }
        public int Version { get; private set; }
        public LangType Type { get; private set; }
        public Language Language { get; private set; }
        public bool IsNew { get; private set; }
        public string FilePath { get; private set; }
        public string DirectoryPath { get; private set; }
        public string FileRoute { get; private set; }

        private readonly Logger _logger;
        private readonly HttpClient _httpClient;

        internal Lang(Logger logger, HttpClient httpClient, string name, int version, LangType type, Language language, bool isNew, string filePath, string directoryPath, string fileRoute)
        {
            _logger = logger;
            _httpClient = httpClient;
            Name = name;
            Version = version;
            Type = type;
            Language = language;
            IsNew = isNew;
            FilePath = filePath;
            DirectoryPath = directoryPath;
            FileRoute = fileRoute;
        }

        /// <summary>
        /// Download the lang file.
        /// </summary>
        /// <returns>True if the file is successfully downloaded</returns>
        public async Task<bool> Download()
        {
            try
            {
                using (HttpResponseMessage response = await _httpClient.GetAsync(FileRoute).ConfigureAwait(false))
                {
                    response.EnsureSuccessStatusCode();

                    Directory.CreateDirectory(DirectoryPath);

                    using (FileStream fileStream = new(FilePath, FileMode.Create))
                        await response.Content.CopyToAsync(fileStream).ConfigureAwait(false);

                    return true;
                }
            }
            catch (HttpRequestException e)
            {
                _logger.Error(e.Message);
                return false;
            }
        }
    }
}
