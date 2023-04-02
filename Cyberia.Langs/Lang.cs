namespace Cyberia.Langs
{
    public sealed class Lang
    {
        public string Name { get; init; }
        public int Version { get; init; }
        public LangType Type { get; init; }
        public Language Language { get; init; }
        public bool IsNew { get; init; }
        public string FilePath { get; init; }
        public string DirectoryPath { get; init; }
        public string FileRoute { get; init; }

        internal Lang(string name, int version, LangType type, Language language, bool isNew, string filePath, string directoryPath, string fileRoute)
        {
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
        public async Task<bool> DownloadAsync()
        {
            int retries = 5;
            int waitTime = 1000;

            while (true)
            {
                try
                {
                    using (HttpResponseMessage response = await DofusLangs.Instance.HttpClient.GetAsync(FileRoute).ConfigureAwait(false))
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
                    retries--;
                    DofusLangs.Instance.Logger.Error($"{retries} retry left for '{FilePath}'", e);

                    if (retries == 0)
                        return false;

                    await Task.Delay(waitTime);
                    waitTime *= 2;
                }
            }
        }
    }
}
