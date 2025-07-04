﻿using Dapper;

using System.Data;
using System.Globalization;

namespace Cyberia.Database.TypeHandlers;

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

        throw new ArgumentException("Invalid value type for DateTimeHandler", nameof(value));
    }

    public override void SetValue(IDbDataParameter parameter, DateTime value)
    {
        parameter.Value = value;
    }
}
