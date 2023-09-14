using System.Text.RegularExpressions;

namespace Cyberia.Api
{
    public enum FormatType
    {
        None,
        MarkDown,
        Html
    }

    internal static partial class Formatter
    {
        [GeneratedRegex(@"([`\*_~<>\[\]\(\)""@\!\&#:\|])", RegexOptions.ECMAScript)]
        private static partial Regex SanitizeMarkDownRegex();

        internal static string Bold(this string value)
        {
            return DofusApi.Instance.Config.FormatType switch
            {
                FormatType.Html => $"<b>{value}</b>",
                FormatType.MarkDown => $"**{value}**",
                _ => value,
            };
        }

        internal static string Italic(this string value)
        {
            return DofusApi.Instance.Config.FormatType switch
            {
                FormatType.Html => $"<i>{value}</i>",
                FormatType.MarkDown => $"*{value}*",
                _ => value,
            };
        }

        internal static string Underline(this string value)
        {
            return DofusApi.Instance.Config.FormatType switch
            {
                FormatType.Html => $"<u>{value}</u>",
                FormatType.MarkDown => $"__{value}__",
                _ => value,
            };
        }

        internal static string Strike(this string value)
        {
            return DofusApi.Instance.Config.FormatType switch
            {
                FormatType.Html => $"<s>{value}</s>",
                FormatType.MarkDown => $"~~{value}~~",
                _ => value,
            };
        }

        internal static string BlockCode(this string value, string language = "")
        {
            return DofusApi.Instance.Config.FormatType switch
            {
                FormatType.Html => $"<code>{value}</code>",
                FormatType.MarkDown => $"```{language}\n{value}\n```",
                _ => value,
            };
        }

        internal static string InlineCode(this string value)
        {
            return DofusApi.Instance.Config.FormatType switch
            {
                FormatType.Html => $"<code>{value}</code>",
                FormatType.MarkDown => $"`{value}`",
                _ => value,
            };
        }

        internal static string SpoilerCode(this string value)
        {
            return DofusApi.Instance.Config.FormatType switch
            {
                FormatType.Html => $"<details><summary>spoiler</summary>{value}</details>",
                FormatType.MarkDown => $"||{value}||",
                _ => value,
            };
        }

        internal static string SanitizeMarkDown(this string value)
        {
            if (DofusApi.Instance.Config.FormatType == FormatType.MarkDown)
            {
                Regex regex = SanitizeMarkDownRegex();

                return regex.Replace(value, m => $"\\{m.Groups[1].Value}");
            }

            return value;
        }
    }
}
