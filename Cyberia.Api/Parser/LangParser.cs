using Cyberia.Langzilla;
using Cyberia.Langzilla.Enums;

using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Web;

namespace Cyberia.Api.Parser;

public static partial class LangParser
{
    private const string KEY_VALUE_SEPARATOR = " = ";

    private static readonly string[] _ignoredLangs = ["dungeons", "effects", "lang"];
    private static readonly string[] _ignoredLines = ["new Object();", "new Array();"];

    public static bool Launch(LangType type, LangLanguage language)
    {
        var langsType = LangsWatcher.GetLangsByType(type);
        var langDataCollection = langsType.GetLangsByLanguage(language);

        foreach (var langData in langDataCollection)
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
        var filePath = langData.GetCurrentDecompiledFilePath();
        if (!File.Exists(filePath))
        {
            Log.Error("The lang {LangName} has never been decompiled", langData.Name);
            return false;
        }

        Log.Debug("Start parsing {LangName} lang", langData.Name);

        var lines = File.ReadAllLines(filePath);
        var json = ParseLines(lines);

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
        StringBuilder jsonBuilder = new(lines.Sum(x => x.Length));

        var lastLineName = string.Empty;
        var lastLineHasId = false;

        foreach (var line in lines)
        {
            if (_ignoredLines.Any(x => line.EndsWith(x)))
            {
                continue;
            }

            var lineSplit = line.Split(KEY_VALUE_SEPARATOR, 2);
            if (lineSplit.Length < 2)
            {
                continue;
            }

            var key = KeyRegex().Match(lineSplit[0]);
            var currentLineName = key.Groups["name"].Value;
            var currentLineHasId = key.Groups["intId"].Success || key.Groups["stringId"].Success;

            if (!currentLineName.Equals(lastLineName))
            {
                if (!string.IsNullOrEmpty(lastLineName))
                {
                    jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                    if (lastLineHasId)
                    {
                        jsonBuilder.Append(']');
                    }

                    jsonBuilder.Append(',');
                }

                jsonBuilder.Append('"');
                jsonBuilder.Append(currentLineName);
                jsonBuilder.Append("\":");

                if (currentLineHasId)
                {
                    jsonBuilder.Append('[');
                }

                lastLineName = currentLineName;
                lastLineHasId = currentLineHasId;
            }

            if (key.Groups["intId"].Success)
            {
                jsonBuilder.Append("{\"id\":");
                jsonBuilder.Append(key.Groups["intId"].Value);
                jsonBuilder.Append(',');
            }
            else if (key.Groups["stringId"].Success)
            {
                jsonBuilder.Append("{\"id\":\"");
                jsonBuilder.Append(key.Groups["stringId"].Value);
                jsonBuilder.Append("\",");
            }

            var value = lineSplit[1].Replace("' + '\"' + '", @"\""");
            value = EscapedQuoteRegex().Replace(value, "\"").Replace(@"\'", "'");
            value = HttpUtility.JavaScriptStringEncode(value).Replace(@"\""", "\"").Replace(@"\\", @"\");

            if (currentLineHasId)
            {
                if (value.StartsWith('{'))
                {
                    jsonBuilder.Append(value[1..^1]);
                }
                else
                {
                    jsonBuilder.Append("\"v\":");
                    jsonBuilder.Append(value[..^1]);
                    jsonBuilder.Append('}');
                }
            }
            else
            {
                jsonBuilder.Append(value[..^1]);
            }

            jsonBuilder.Append(',');
        }

        if (jsonBuilder.Length > 0)
        {
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            if (lastLineHasId)
            {
                jsonBuilder.Append(']');
            }
        }

        return "{" + jsonBuilder.ToString() + "}";
    }
}
