using Cyberia.Api.Factories.Effects;
using Cyberia.Langzilla.Enums;

using System.Globalization;

namespace Cyberia.Api.Utils;

/// <summary>
/// Provides utilities for game-specific DateTime operations and formatting.
/// </summary>
public static class GameDateFormatter
{
    /// <summary>
    /// Creates a DateTime from effect parameters.
    /// </summary>
    /// <param name="parameters">The effect parameters containing the compressed date information.</param>
    /// <returns>The corresponding DateTime, or <see cref="DateTime.MaxValue"/> if the parameters indicate an infinite date.</returns>
    public static DateTime CreateDateTimeFromEffectParameters(EffectParameters parameters)
    {
        if (parameters.Param1 == -1)
        {
            return DateTime.MaxValue;
        }

        var year = (int)parameters.Param1;
        var month = (int)Math.Floor(parameters.Param2 / 100D) + 1;
        var day = (int)parameters.Param2 - (month - 1) * 100;
        var hour = (int)Math.Floor(parameters.Param3 / 100D);
        var minute = (int)parameters.Param3 - hour * 100;

        if (year == 0)
        {
            year = 1;
        }

        if (month == 0)
        {
            month = 1;
        }

        if (day == 0)
        {
            day = 1;
        }

        return new DateTime(year, month, day, hour, minute, 0);
    }

    /// <summary>
    /// Converts a real-world DateTime to an in-game DateTime.
    /// </summary>
    /// <param name="dateTime">The real-world DateTime to convert.</param>
    /// <returns>The equivalent in-game DateTime.</returns>
    public static DateTime ToInGameDateTime(this DateTime dateTime)
    {
        return dateTime.AddYears(DofusApi.Datacenter.TimeZonesRepository.YearLess);
    }

    /// <inheritdoc cref="ToLongRolePlayString(DateTime, CultureInfo?)"/>
    /// <param name="language">The language to use for formatting.</param>
    public static string ToLongRolePlayString(this DateTime dateTime, Language language)
    {
        return ToLongRolePlayString(dateTime, language.ToCulture());
    }

    /// <summary>
    /// Converts a DateTime to a role-playing string representation in the specified culture.
    /// </summary>
    /// <param name="dateTime">The DateTime to format.</param>
    /// <param name="culture">The culture to use for formatting, or <see langword="null"/> for current culture.</param>
    /// <returns>A formatted string representing the date in role-playing format.</returns>
    public static string ToLongRolePlayString(this DateTime dateTime, CultureInfo? culture = null)
    {
        var month = DofusApi.Datacenter.TimeZonesRepository.GetMonthNameByDayOfYear(dateTime.DayOfYear, culture);
        var time = dateTime.ToString(
            culture?.DateTimeFormat.ShortTimePattern ?? CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern);

        return $"{dateTime.Day} {month} {dateTime.Year} - {time}";
    }

    /// <inheritdoc cref="ToLongRolePlayString(DateTime, Language)"/>
    public static string ToShortRolePlayString(this DateTime dateTime, Language language)
    {
        return ToShortRolePlayString(dateTime, language.ToCulture());
    }

    /// <inheritdoc cref="ToLongRolePlayString(DateTime, CultureInfo)"/>
    public static string ToShortRolePlayString(this DateTime dateTime, CultureInfo? culture = null)
    {
        var time = dateTime.ToString(
            culture?.DateTimeFormat.ShortTimePattern ?? CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern);

        return $"{dateTime:dd/MM/yyy} {time}";
    }
}
