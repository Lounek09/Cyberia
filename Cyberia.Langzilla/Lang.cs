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

            string directoryPath = GetDirectoryPath();
            IsNew = !Directory.Exists(directoryPath);
            Directory.CreateDirectory(directoryPath);
        }

        public string GetDirectoryPath()
        {
            return Path.Join(LangsWatcher.GetOutputDirectoryPath(Type, Language), Name);
        }

        public string GetFileName()
        {
            return $"{Name}_{Language.ToString().ToLower()}_{Version}.swf";
        }

        public string GetFileUrl()
        {
            return $"{LangsWatcher.BASE_URL}/{LangsWatcher.GetRoute(Type)}/swf/{GetFileName()}";
        }

        public string GetFilePath()
        {
            return Path.Join(GetDirectoryPath(), GetFileName());
        }

        public string GetCurrentDecompiledFilePath()
        {
            return Path.Join(GetDirectoryPath(), "current.as");
        }

        public string GetOldDecompiledFilePath()
        {
            return Path.Join(GetDirectoryPath(), "old.as");
        }

        public string GetDiffFilePath()
        {
            return Path.Join(GetDirectoryPath(), "diff.as");
        }

        public string GenerateDiff(Lang lang)
        {
            string currentDecompiledFilePath = GetCurrentDecompiledFilePath();
            if (!File.Exists(currentDecompiledFilePath))
            {
                return "";
            }
            string[] currentLines = File.ReadAllLines(currentDecompiledFilePath);

            string modelDecompiledFilePath = lang == this ? GetOldDecompiledFilePath() : lang.GetCurrentDecompiledFilePath();
            if (!File.Exists(modelDecompiledFilePath))
            {
                return string.Join('\n', currentLines.Select(x => $"+ {x}"));
            }
            string[] modelLines = File.ReadAllLines(modelDecompiledFilePath);

            List<(int Index, string Row)> diff = new();

            int index = 0;
            Dictionary<int, string> currentRows = currentLines.ToDictionary(x => index++);

            index = 0;
            Dictionary<int, string> modelRows = modelLines.ToDictionary(x => index++);

            foreach (KeyValuePair<int, string> row in currentRows)
            {
                if (!modelRows.RemoveByValue(row.Value, true))
                {
                    diff.Add((row.Key, $"+ {row.Value}"));
                }
            }

            foreach (KeyValuePair<int, string> row in modelRows)
            {
                diff.Add((row.Key, $"- {row.Value}"));
            }

            return string.Join('\n', diff.OrderBy(x => x.Index).Select(x => x.Row));
        }

        public async Task DownloadExtractAndDiffAsync()
        {
            await DownloadAsync();
            Extract();
            Diff();
        }

        private async Task DownloadAsync()
        {
            Array.ForEach(Directory.GetFiles(GetDirectoryPath(), "*.swf"), File.Delete);

            string fileUrl = GetFileUrl();

            try
            {
                using HttpResponseMessage response = await LangsWatcher.HttpRetryPolicy.ExecuteAsync(() => LangsWatcher.HttpClient.GetAsync(fileUrl));
                response.EnsureSuccessStatusCode();

                using FileStream fileStream = new(GetFilePath(), FileMode.Create);
                await response.Content.CopyToAsync(fileStream);

                return;
            }
            catch (HttpRequestException e)
            {
                Log.Error(e, "An error occured while sending Get request to {url}}", fileUrl);
            }
        }

        private void Extract()
        {
            string filePath = GetFilePath();
            if (!File.Exists(filePath))
            {
                return;
            }

            string currentDecompiledFilePath = GetCurrentDecompiledFilePath();
            string oldDecompiledFilePath = GetOldDecompiledFilePath();

            if (!Flare.TryExtractSwf(filePath, out string warningMessage, out string flareFilePath))
            {
                Log.Error("Error when decompiling {filePath}\nWarning : {warningMessage}", filePath, warningMessage);
                return;
            }

            List<string> content = new();
            foreach (string line in File.ReadAllLines(flareFilePath, Encoding.UTF8).Skip(7).SkipLast(3))
            {
                string temp = line.Trim();

                if (temp.Length == 0 || temp.Equals("}") || temp.Equals("frame 1 {"))
                {
                    continue;
                }

                content.Add(temp);
            }

            if (File.Exists(currentDecompiledFilePath))
            {
                File.Move(currentDecompiledFilePath, oldDecompiledFilePath, true);
            }
            File.WriteAllLines(currentDecompiledFilePath, content, Encoding.UTF8);
            File.Delete(flareFilePath);
        }

        private void Diff()
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
