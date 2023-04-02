﻿namespace Cyberia.Utils
{
    public static class ExtendObject
    {
        public static bool IsNumber(this object? value)
        {
            return value is sbyte
                    || value is byte
                    || value is short
                    || value is ushort
                    || value is int
                    || value is uint
                    || value is nint
                    || value is long
                    || value is ulong
                    || value is float
                    || value is double
                    || value is decimal;
        }
    }
}
