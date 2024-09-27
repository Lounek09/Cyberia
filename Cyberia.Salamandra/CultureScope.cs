using System.Globalization;

namespace Cyberia.Salamandra;

/// <summary>
/// Represents a scope to change the culture of the current thread.
/// </summary>
public sealed class CultureScope : IDisposable
{
    private readonly CultureInfo _originalCulture;
    private readonly CultureInfo _originalUICulture;

    /// <summary>
    /// Initializes a new instance of the <see cref="CultureScope"/> class.
    /// </summary>
    /// <param name="culture">The culture to set the current thread to.</param>
    /// <param name="UICulture">The UI culture to set the current thread to.</param>
    public CultureScope(CultureInfo culture, CultureInfo UICulture)
    {
        _originalCulture = CultureInfo.CurrentCulture;
        _originalUICulture = CultureInfo.CurrentUICulture;

        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = UICulture;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CultureScope"/> class.
    /// </summary>
    /// <param name="culture">The culture to set the current thread to.</param>
    public CultureScope(CultureInfo culture)
        : this(culture, culture)
    {

    }

    /// <summary>
    /// Restores the original culture of the current thread.
    /// </summary>
    public void Dispose()
    {
        CultureInfo.CurrentCulture = _originalCulture;
        CultureInfo.CurrentUICulture = _originalUICulture;
    }
}
