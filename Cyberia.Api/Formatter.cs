using System.Text.RegularExpressions;

namespace Cyberia.Api
{
    public enum FormatType
    {
        None,
        MarkDown,
        Html
    }

    internal static class Formatter
    {
        internal static string Bold(this string value)
        {
            switch (DofusApi.Instance.FormatType)
            {
                case FormatType.Html:
                    return $"<b>{value}</b>";
                case FormatType.MarkDown:
                    return $"**{value}**";
                default:
                    return value;
            }
        }

        internal static string Italic(this string value)
        {
            switch (DofusApi.Instance.FormatType)
            {
                case FormatType.Html:
                    return $"<i>{value}</i>";
                case FormatType.MarkDown:
                    return $"*{value}*";
                default:
                    return value;
            }
        }

        internal static string Underline(this string value)
        {
            switch (DofusApi.Instance.FormatType)
            {
                case FormatType.Html:
                    return $"<u>{value}</u>";
                case FormatType.MarkDown:
                    return $"__{value}__";
                default:
                    return value;
            }
        }

        internal static string Strike(this string value)
        {
            switch (DofusApi.Instance.FormatType)
            {
                case FormatType.Html:
                    return $"<s>{value}</s>";
                case FormatType.MarkDown:
                    return $"~~{value}~~";
                default:
                    return value;
            }
        }

        internal static string BlockCode(this string value, string language = "")
        {
            switch (DofusApi.Instance.FormatType)
            {
                case FormatType.Html:
                    return $"<code>{value}</code>";
                case FormatType.MarkDown:
                    return $"```{language}\n{value}\n```";
                default:
                    return value;
            }
        }

        internal static string InlineCode(this string value)
        {
            switch (DofusApi.Instance.FormatType)
            {
                case FormatType.Html:
                    return $"<code>{value}</code>";
                case FormatType.MarkDown:
                    return $"`{value}`";
                default:
                    return value;
            }
        }

        internal static string SpoilerCode(this string value)
        {
            switch (DofusApi.Instance.FormatType)
            {
                case FormatType.Html:
                    return $"<details><summary>spoiler</summary>{value}</details>";
                case FormatType.MarkDown:
                    return $"||{value}||";
                default:
                    return value;
            }
        }

        internal static string SanitizeMarkDown(this string value)
        {
            if (DofusApi.Instance.FormatType == FormatType.MarkDown)
            {
                Regex regex = new(@"([`\*_~<>\[\]\(\)""@\!\&#:\|])", RegexOptions.ECMAScript);

                return regex.Replace(value, m => $"\\{m.Groups[1].Value}");
            }

            return value;
        }
    }
}
