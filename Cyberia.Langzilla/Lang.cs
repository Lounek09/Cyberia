using Cyberia.Langzilla.Enums;

using System.Text;

namespace Cyberia.Langzilla
{
    public sealed class Lang
    {
        public string Name { get; init; }
        public int Version { get; init; }
        public LangType Type { get; init; }
        public Language Language { get; init; }
        public bool IsNew { get; init; }

        public Lang()
        {
            Name = string.Empty;
        }

        internal Lang(string name, int version, LangType type, Language language)
        {
            Name = name;
            Version = version;
            Type = type;
            Language = language;
            IsNew = !Directory.Exists(GetDirectoryPath());

            Directory.CreateDirectory(GetDirectoryPath());
        }

        public string GetDirectoryPath()
        {
            return $"{DofusLangs.GetDirectoryPath(Type, Language)}/{Name.ToLower()}";
        }

        public string GetFileName()
        {
            return $"{Name}_{Language.ToString().ToLower()}_{Version}.swf";
        }

        public string GetFileUrl()
        {
            return $"{DofusLangs.BASE_URL}/{DofusLangs.GetRoute(Type)}/swf/{GetFileName()}";
        }

        public string GetFilePath()
        {
            return $"{GetDirectoryPath()}/{GetFileName()}";
        }

        public string GetCurrentDecompiledFilePath()
        {
            return $"{GetDirectoryPath()}/current.as";
        }

        public string GetOldDecompiledFilePath()
        {
            return $"{GetDirectoryPath()}/old.as";
        }

        public string GetDiffFilePath()
        {
            return $"{GetDirectoryPath()}/diff.as";
        }

        public string GenerateDiff(Lang lang)
        {
            if (!File.Exists(GetCurrentDecompiledFilePath()))
                return "";

            List<KeyValuePair<int, string>> diff = new();

            int currentIndex = 0;
            Dictionary<int, string> currentRows = File.ReadAllLines(GetCurrentDecompiledFilePath())
                .ToDictionary(x => currentIndex++);

            int oldIndex = 0;
            string oldDecompiledFilePath = lang == this ? GetOldDecompiledFilePath() : lang.GetCurrentDecompiledFilePath();
            Dictionary<int, string> oldRows = File.Exists(oldDecompiledFilePath) ? File.ReadAllLines(oldDecompiledFilePath).ToDictionary(x => oldIndex++) : new();

            foreach (KeyValuePair<int, string> row in currentRows)
            {
                if (!oldRows.RemoveByValue(row.Value, true))
                    diff.Add(new(row.Key, $"+ {row.Value}"));
            }

            foreach (KeyValuePair<int, string> row in oldRows)
                diff.Add(new(row.Key, $"- {row.Value}"));

            return string.Join("\n", diff.OrderBy(x => x.Key).Select(x => x.Value));
        }

        internal async Task DownloadAsync()
        {
            Array.ForEach(Directory.GetFiles(GetDirectoryPath(), "*.swf"), File.Delete);

            int retries = 5;
            int waitTime = 1000;
            while (true)
            {
                try
                {
                    using HttpResponseMessage response = await DofusLangs.Instance.HttpClient.GetAsync(GetFileUrl()).ConfigureAwait(false);
                    response.EnsureSuccessStatusCode();

                    using FileStream fileStream = new(GetFilePath(), FileMode.Create);
                    await response.Content.CopyToAsync(fileStream).ConfigureAwait(false);

                    return;
                }
                catch (HttpRequestException e)
                {
                    if (retries-- == 0)
                    {
                        DofusLangs.Instance.Logger.Error($"Unable to find {GetFileUrl()}");
                        return;
                    }

                    DofusLangs.Instance.Logger.Error($"{retries} retry left to download {GetFileUrl()}", e);

                    await Task.Delay(waitTime);
                    waitTime *= 2;
                }
                catch (TaskCanceledException e)
                {
                    DofusLangs.Instance.Logger.Error($"The request to get {GetFileUrl()} was cancelled", e);
                    return;
                }
            }
        }

        internal void Extract()
        {
            string filePath = GetFilePath();
            if (!File.Exists(filePath))
                return;

            string currentDecompiledFilePath = GetCurrentDecompiledFilePath();
            string oldDecompiledFilePath = GetOldDecompiledFilePath();

            if (!Flare.ExtractSwf(filePath, out string warningMessage))
            {
                DofusLangs.Instance.Logger.Error($"Error when decompiled '{filePath}'\nWarning : {warningMessage}");
                return;
            }

            string flareOutputFilePath = $"{filePath.TrimEnd(".swf")}.flr";

            List<string> content = new();
            foreach (string line in File.ReadAllLines(flareOutputFilePath, Encoding.UTF8).Skip(7).SkipLast(3))
            {
                string temp = line.Trim();

                if (temp.Length == 0 || temp.Equals("}") || temp.Equals("frame 1 {"))
                    continue;

                content.Add(temp);
            }

            if (File.Exists(currentDecompiledFilePath))
                File.Move(currentDecompiledFilePath, oldDecompiledFilePath, true);
            File.WriteAllLines(currentDecompiledFilePath, content, Encoding.UTF8);
            File.Delete(flareOutputFilePath);
        }

        internal void Diff()
        {
            string diff = GenerateDiff(this);

            if (string.IsNullOrEmpty(diff))
            {
                File.Delete(GetDiffFilePath());
                return;
            }

            File.WriteAllText(GetDiffFilePath(), diff, Encoding.UTF8);
        }
    }
}
