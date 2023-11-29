using System.Globalization;

namespace Cyberia.Utils;

public static class ExtendInt
{
    public static string ToStringThousandSeparator(this int value)
    {
        var numberFormatInfo = new NumberFormatInfo()
        {
            NumberGroupSeparator = " "
        };

        return value.ToString("#,0", numberFormatInfo);
    }
}
