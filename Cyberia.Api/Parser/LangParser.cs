using Cyberia.Langzilla;
using Cyberia.Langzilla.Enums;

using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Cyberia.Api.Parser
{
    public static class LangParser
    {
        public static readonly List<string> LangsToParse = new()
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

        private const string KEY_VALUE_SEPARATOR = " = ";

        private static readonly string[] _ignoredLines = new string[]
        {
            "new Object();",
            "new Array();"
        };

        public static bool Launch(string langName, out string message)
        {
            if (langName.Equals("all"))
            {
                foreach (string langNameToParse in LangsToParse)
                {
                    if (!TryParse(langNameToParse))
                    {
                        message = $"An error occured while parsing {langNameToParse} lang";
                        DofusApi.Instance.Logger.Error(message);
                        return false;
                    }
                }

                message = $"Successful parsing of all langs";
                DofusApi.Instance.Logger.Info(message);
                return true;
            }

            if (!LangsToParse.Contains(langName))
            {
                message = $"{langName} lang doesn't exist or should not be parsed";
                DofusApi.Instance.Logger.Error(message);
                return false;
            }

            if (!TryParse(langName))
            {
                message = $"An error occured while parsing {langName} lang";
                DofusApi.Instance.Logger.Error(message);
                return false;
            }

            message = $"Successful parsing of {langName} lang";
            DofusApi.Instance.Logger.Info(message);
            return true;
        }

        private static bool TryParse(string langName)
        {
            LangsData langsData = DofusApi.Instance.DofusLangs.GetLangsData(DofusApi.Instance.Temporis ? LangType.Temporis : LangType.Official, Language.FR);
            Lang? lang = langsData.GetLangByName(langName);
            if (lang is null)
            {
                DofusApi.Instance.Logger.Error($"The lang {langName} doesn't exist");
                return false;
            }

            if (!File.Exists(lang.GetCurrentDecompiledFilePath()))
            {
                DofusApi.Instance.Logger.Error($"The lang {langName} has never been decompiled");
                return false;
            }

            DofusApi.Instance.Logger.Debug($"Start parsing {langName} lang");

            string[] lines = File.ReadAllLines(lang.GetCurrentDecompiledFilePath());
            if (!TryParseLines(lines, out string json))
            {
                DofusApi.Instance.Logger.Error($"Error while parsing {langName} lang");
                return false;
            }

            try
            {
                json = Json.Indent(json);
            }
            catch (Exception e)
            {
                DofusApi.Instance.Logger.Error(e);
                return false;
            }

            File.WriteAllText($"{DofusApi.OUTPUT_PATH}/{langName}.json", json);
            return true;
        }

        private static bool TryParseLines(string[] lines, out string json)
        {
            json = "{";

            string lastLineName = "";
            bool lastLineHasId = false;
            foreach (string line in lines)
            {
                if (_ignoredLines.Any(x => line.EndsWith(x)))
                    continue;

                string[] lineSplit = line.Split(KEY_VALUE_SEPARATOR, 2);
                if (lineSplit.Length < 2)
                    return false;

                string key = lineSplit[0];

                Match regex = Regex.Match(key, @"(?'name'[A-Z]+(?:\.[a-z]+|))(?:\[(?'intId'-?\d+)\]|\.(?'stringId'[\w|]+)|)");
                string currentLineName = regex.Groups[1].Value;
                bool currentLineHasId = regex.Groups[2].Success || regex.Groups[3].Success;

                if (!currentLineName.Equals(lastLineName))
                {
                    if (!string.IsNullOrEmpty(lastLineName))
                        json = $"{json[..^1]}{(lastLineHasId ? "]" : "")},";

                    json += $"\"{currentLineName.Replace(".", "")}\":{(currentLineHasId ? "[" : "")}";
                    lastLineName = currentLineName;
                    lastLineHasId = currentLineHasId;
                }

                if (regex.Groups[2].Success)
                    json += $"{{\"id\":{regex.Groups[2].Value},";
                else if (regex.Groups[3].Success)
                    json += $"{{\"id\":\"{regex.Groups[3].Value}\",";

                string value = lineSplit[1].Replace("' + '\"' + '", "\\\"");
                value = Regex.Replace(value, @"(?<!\\)'", "\"").Replace(@"\'", "'");
                value = HttpUtility.JavaScriptStringEncode(value).Replace("\\\"", "\"").Replace(@"\\", @"\");

                if (value.StartsWith('{') && currentLineHasId)
                    json += $"{(currentLineHasId ? "" : "{")}{value[1..^1]},";
                else
                    json += currentLineHasId ? $"\"v\":{value[..^1]}}}," : $"{value[..^1]},";
            }

            json = $"{json[..^1]}{(lastLineHasId ? "]" : "")}}}";
            return true;
        }
    }
}
