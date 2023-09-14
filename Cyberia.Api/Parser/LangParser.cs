using Cyberia.Langzilla;

using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Web;

namespace Cyberia.Api.Parser
{
    public static partial class LangParser
    {
        private const string KEY_VALUE_SEPARATOR = " = ";

        private static readonly string[] _langsToParse = new string[]
        {
            "alignment",
            "audio",
            "classes",
            "crafts",
            "dialog",
            "emotes",
            "fightChallenge",
            "guilds",
            "hints",
            "houses",
            "interactiveobjects",
            "items",
            "itemsets",
            "itemstats",
            "jobs",
            "kb",
            "maps",
            "monsters",
            "names",
            "npc",
            "pvp",
            "quests",
            "ranks",
            "rides",
            "scripts",
            "servers",
            "skills",
            "speakingitems",
            "spells",
            "states",
            "timezones",
            "titles",
            "ttg"
        };
        private static readonly string[] _ignoredLines = new string[]
        {
            "new Object();",
            "new Array();"
        };

        public static bool Launch()
        {
            foreach (string langToParse in _langsToParse)
            {
                if (!TryParse(langToParse))
                {
                    DofusApi.Instance.Log.Error("An error occured while parsing {langToParse} lang", langToParse);
                    return false;
                }
            }

            return true;
        }

        [GeneratedRegex(@"(?'name'[A-Z]+(?:\.[a-z]+|))(?:\[(?'intId'-?\d+)\]|\.(?'stringId'[\w|]+)|)", RegexOptions.Compiled)]
        private static partial Regex KeyRegex();

        private static bool TryParse(string langName)
        {
            LangsData langsData = DofusApi.Instance.Config.Temporis ? DofusApi.Instance.LangsWatcher.Temporis.French : DofusApi.Instance.LangsWatcher.Official.French;
            
            Lang? lang = langsData.GetLangByName(langName);
            if (lang is null)
            {
                DofusApi.Instance.Log.Error("The lang {langName} doesn't exist", langName);
                return false;
            }

            string filePath = lang.GetCurrentDecompiledFilePath();
            if (!File.Exists(filePath))
            {
                DofusApi.Instance.Log.Error("The lang {langName} has never been decompiled", langName);
                return false;
            }

            DofusApi.Instance.Log.Debug("Start parsing {langName} lang", langName);

            string[] lines = File.ReadAllLines(filePath);
            string json = ParseLines(lines);

            try
            {
                JsonDocument.Parse(json);
            }
            catch (Exception e)
            {
                DofusApi.Instance.Log.Error(e, "The generated json is not valid for {lang} lang", langName);
                return false;
            }

            File.WriteAllText($"{DofusApi.OUTPUT_PATH}/{langName}.json", json);
            return true;
        }

        private static string ParseLines(string[] lines)
        {
            StringBuilder json = new();
            string lastLineName = "";
            bool lastLineHasId = false;

            foreach (string line in lines)
            {
                if (_ignoredLines.Any(x => line.EndsWith(x)))
                    continue;

                string[] lineSplit = line.Split(KEY_VALUE_SEPARATOR, 2);
                if (lineSplit.Length < 2)
                    continue;

                Match key = KeyRegex().Match(lineSplit[0]);
                string currentLineName = key.Groups["name"].Value;
                bool currentLineHasId = key.Groups["intId"].Success || key.Groups["stringId"].Success;

                if (!currentLineName.Equals(lastLineName))
                {
                    if (!string.IsNullOrEmpty(lastLineName))
                    {
                        json.Remove(json.Length - 1, 1);
                        if (lastLineHasId) json.Append(']');
                        json.Append(',');
                    }

                    json.AppendFormat("\"{0}\":", currentLineName);
                    if (currentLineHasId) json.Append('[');

                    lastLineName = currentLineName;
                    lastLineHasId = currentLineHasId;
                }

                if (key.Groups["intId"].Success)
                    json.AppendFormat("{{\"id\":{0},", key.Groups["intId"].Value);
                else if (key.Groups["stringId"].Success)
                    json.AppendFormat("{{\"id\":\"{0}\",", key.Groups["stringId"].Value);

                string value = lineSplit[1].Replace("' + '\"' + '", @"\""");
                value = Regex.Replace(value, @"(?<!\\)'", "\"").Replace(@"\'", "'");
                value = HttpUtility.JavaScriptStringEncode(value).Replace(@"\""", "\"").Replace(@"\\", @"\");

                if (currentLineHasId)
                {
                    if (value.StartsWith('{'))
                        json.Append(value[1..^1]);
                    else
                        json.AppendFormat("\"v\":{0}}}", value[..^1]);
                }
                else
                    json.Append(value[..^1]);

                json.Append(',');
            }

            if (json.Length > 0)
            {
                json.Remove(json.Length - 1, 1);
                if (lastLineHasId) json.Append(']');
            }

            return "{" + json.ToString() + "}";
        }
    }
}
