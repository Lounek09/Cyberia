using Cyberia.Langzilla;
using Cyberia.Langzilla.Enums;

using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Web;

namespace Cyberia.Api.Parser
{
    public static partial class LangParser
    {
        private const string KEY_VALUE_SEPARATOR = " = ";

        private static readonly string[] _ignoredLangs = ["dungeons", "effects", "lang"];
        private static readonly string[] _ignoredLines = ["new Object();", "new Array();"];

        public static bool Launch(LangType type, LangLanguage language)
        {
            LangsType langsType = LangsWatcher.GetLangsByType(type);
            LangDataCollection langDataCollection = langsType.GetLangsByLanguage(language);

            foreach (LangData langData in langDataCollection.Items)
            {
                if (_ignoredLangs.Contains(langData.Name))
                {
                    continue;
                }

                if (!TryParseLangData(langData))
                {
                    Log.Error("An error occurred while parsing {LangName} lang", langData.Name);
                    return false;
                }
            }

            return true;
        }

        [GeneratedRegex(@"(?'name'[A-Z]+(?:\.[a-z]+|))(?:\[(?'intId'-?\d+)\]|\.(?'stringId'[\w|]+)|)", RegexOptions.Compiled)]
        private static partial Regex KeyRegex();

        [GeneratedRegex(@"(?<!\\)'")]
        private static partial Regex EscapedQuoteRegex();

        private static bool TryParseLangData(LangData langData)
        {
            string filePath = langData.GetCurrentDecompiledFilePath();
            if (!File.Exists(filePath))
            {
                Log.Error("The lang {LangName} has never been decompiled", langData.Name);
                return false;
            }

            Log.Debug("Start parsing {LangName} lang", langData.Name);

            string[] lines = File.ReadAllLines(filePath);
            string json = ParseLines(lines);

            try
            {
                JsonDocument.Parse(json);
            }
            catch (Exception e)
            {
                Log.Error(e, "The JSON generated from {LangName} lang is not valid", langData.Name);
                return false;
            }

            File.WriteAllText(Path.Join(DofusApi.OUTPUT_PATH, $"{langData.Name}.json"), json);
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
                {
                    continue;
                }

                string[] lineSplit = line.Split(KEY_VALUE_SEPARATOR, 2);
                if (lineSplit.Length < 2)
                {
                    continue;
                }

                Match key = KeyRegex().Match(lineSplit[0]);
                string currentLineName = key.Groups["name"].Value;
                bool currentLineHasId = key.Groups["intId"].Success || key.Groups["stringId"].Success;

                if (!currentLineName.Equals(lastLineName))
                {
                    if (!string.IsNullOrEmpty(lastLineName))
                    {
                        json.Remove(json.Length - 1, 1);
                        if (lastLineHasId)
                        {
                            json.Append(']');
                        }

                        json.Append(',');
                    }

                    json.AppendFormat("\"{0}\":", currentLineName);
                    if (currentLineHasId)
                    {
                        json.Append('[');
                    }

                    lastLineName = currentLineName;
                    lastLineHasId = currentLineHasId;
                }

                if (key.Groups["intId"].Success)
                {
                    json.AppendFormat("{{\"id\":{0},", key.Groups["intId"].Value);
                }
                else if (key.Groups["stringId"].Success)
                {
                    json.AppendFormat("{{\"id\":\"{0}\",", key.Groups["stringId"].Value);
                }

                string value = lineSplit[1].Replace("' + '\"' + '", @"\""");
                value = EscapedQuoteRegex().Replace(value, "\"").Replace(@"\'", "'");
                value = HttpUtility.JavaScriptStringEncode(value).Replace(@"\""", "\"").Replace(@"\\", @"\");

                if (currentLineHasId)
                {
                    if (value.StartsWith('{'))
                    {
                        json.Append(value[1..^1]);
                    }
                    else
                    {
                        json.AppendFormat("\"v\":{0}}}", value[..^1]);
                    }
                }
                else
                {
                    json.Append(value[..^1]);
                }

                json.Append(',');
            }

            if (json.Length > 0)
            {
                json.Remove(json.Length - 1, 1);
                if (lastLineHasId)
                {
                    json.Append(']');
                }
            }

            return "{" + json.ToString() + "}";
        }
    }
}
