using System.Globalization;

namespace Cyberia.Tests.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class CultureAttribute : Attribute
{
    private readonly CultureInfo _culture;
    private CultureInfo? _originalCulture;
    private CultureInfo? _originalUICulture;

    public CultureAttribute(string culture)
    {
        _culture = new(culture);
    }

    public void SetCulture()
    {
        _originalCulture = Thread.CurrentThread.CurrentCulture;
        _originalUICulture = Thread.CurrentThread.CurrentUICulture;

        Thread.CurrentThread.CurrentCulture = _culture;
        Thread.CurrentThread.CurrentUICulture = _culture;
    }

    public void RestoreCulture()
    {
        if (_originalCulture is not null && _originalUICulture is not null)
        {
            Thread.CurrentThread.CurrentCulture = _originalCulture;
            Thread.CurrentThread.CurrentUICulture = _originalUICulture;
        }
    }
}
