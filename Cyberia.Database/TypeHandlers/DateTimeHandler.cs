using Dapper;

using System.Data;
using System.Globalization;

namespace Cyberia.Database.TypeHandlers;

/// <summary>
/// Handles the conversion between <see cref="DateTime"/> and its database representation.
/// </summary>
public sealed class DateTimeHandler : SqlMapper.TypeHandler<DateTime>
{
    public override DateTime Parse(object value)
    {
        if (value is string strValue)
        {
            return DateTime.Parse(strValue, null, DateTimeStyles.RoundtripKind);
        }

        if (value is DateTime dateTimeValue)
        {
            return DateTime.SpecifyKind(dateTimeValue, DateTimeKind.Utc);
        }

        throw new ArgumentException($"Invalid value type for {typeof(DateTimeHandler).Name}", nameof(value));
    }

    public override void SetValue(IDbDataParameter parameter, DateTime value)
    {
        parameter.Value = value;
    }
}
