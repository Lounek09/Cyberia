using Cyberia.Api.Factories;

namespace Cyberia.Api.Managers;

public static class DateManager
{
    public static DateTime GetDateTimeFromEffectParameters(EffectParameters parameters)
    {
        if (parameters.Param1 == -1)
        {
            return DateTime.MaxValue;
        }

        var year = parameters.Param1;
        var month = (int)Math.Floor(parameters.Param2 / 100D) + 1;
        var day = parameters.Param2 - (month - 1) * 100;
        var hour = (int)Math.Floor(parameters.Param3 / 100D);
        var minute = parameters.Param3 - hour * 100;

        return new DateTime(year, month, day, hour, minute, 0);
    }

    public static DateTime ToInGameDateTime(this DateTime dateTime)
    {
        return dateTime.AddYears(DofusApi.Datacenter.TimeZonesData.YearLess);
    }

    public static string ToRolePlayString(this DateTime dateTime)
    {
        dateTime = dateTime.ToInGameDateTime();

        return $"{dateTime:dd} {DofusApi.Datacenter.TimeZonesData.GetMonth(dateTime.DayOfYear)} {dateTime:yyy}";
    }
}
