using System.Globalization;

namespace Salamandra.Utils
{
    public static class ExtendInt
    {
        public static string ToStringThousandSeparator(this int value)
        {
            NumberFormatInfo numberFormatInfo = new()
            {
                NumberGroupSeparator = " "
            };

            return value.ToString("#,0", numberFormatInfo);
        }
    }
}
