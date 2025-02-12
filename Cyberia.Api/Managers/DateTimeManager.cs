using Cyberia.Api.Factories;
using Cyberia.Langzilla.Enums;

using System.Globalization;

namespace Cyberia.Api.Managers;

public static class DateTimeManager
{
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

        return new DateTime(year, month, day, hour, minute, 0);
    }

    public static DateTime ToInGameDateTime(this DateTime dateTime)
    {
        return dateTime.AddYears(DofusApi.Datacenter.TimeZonesRepository.YearLess);
    }

    public static string ToRolePlayString(this DateTime dateTime, Language language)
    {
        return ToRolePlayString(dateTime, language.ToCulture());
    }

    public static string ToRolePlayString(this DateTime dateTime, CultureInfo? culture = null)
    {
        dateTime = dateTime.ToInGameDateTime();

        return $"{dateTime:dd} {DofusApi.Datacenter.TimeZonesRepository.GetMonthNameByDayOfYear(dateTime.DayOfYear, culture)} {dateTime:yyy}";
    }
}
