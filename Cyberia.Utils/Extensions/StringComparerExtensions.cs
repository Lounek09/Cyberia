using System.Globalization;

namespace Cyberia.Utils.Extensions;

public static class StringComparerExtensions
{
    private static readonly StringComparer s_invariantNumeric = StringComparer.Create(CultureInfo.InvariantCulture, CompareOptions.NumericOrdering);

    extension(StringComparer)
    {
        public static StringComparer InvariantNumeric => StringComparer.Create(CultureInfo.InvariantCulture, CompareOptions.NumericOrdering);
    }
}
