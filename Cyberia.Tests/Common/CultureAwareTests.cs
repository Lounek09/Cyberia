using Cyberia.Tests.Attributes;

using System.Reflection;

namespace Cyberia.Tests.Common;
public abstract class CultureAwareTest
{
    private CultureAttribute? _cultureAttribute;

    public TestContext TestContext { get; set; } = default!;

    [TestInitialize]
    public void InitializeCulture()
    {
        var testName = TestContext.TestName;
        if (testName is null)
        {
            return;
        }

        var method = GetType().GetMethod(testName);
        if (method is null)
        {
            return;
        }

        _cultureAttribute = method.GetCustomAttribute<CultureAttribute>();
        _cultureAttribute?.SetCulture();
    }

    [TestCleanup]
    public void CleanupCulture()
    {
        _cultureAttribute?.RestoreCulture();
    }
}
