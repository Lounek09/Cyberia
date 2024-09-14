using Cyberia.Tests.Attributes;
using Cyberia.Tests.Common;
using Cyberia.Utils.Extensions;

namespace Cyberia.Tests.Utils;

[TestClass]
public sealed class ExtendNumberTests : CultureAwareTest
{
    #region ToFormattedString

    [TestMethod]
    [Culture("fr")]
    public void ToFormattedString_WithSmallNumber_FormatsCorrectly()
    {
        var value = 123;

        var result = value.ToFormattedString();

        Assert.AreEqual("123", result);
    }

    [TestMethod]
    [Culture("fr")]
    public void ToFormattedString_WithLargeNumber_FormatsCorrectly()
    {
        var value = 1234567;

        var result = value.ToFormattedString();

        Assert.AreEqual("1 234 567", result);
    }

    [TestMethod]
    [Culture("en")]
    public void ToFormattedString_WithLargeNumberAndCultureEn_FormatsCorrectly()
    {
        var value = 1234567;

        var result = value.ToFormattedString();

        Assert.AreEqual("1,234,567", result);
    }

    [TestMethod]
    [Culture("fr")]
    public void ToFormattedString_WithVeryLargeNumber_FormatsCorrectly()
    {
        var value = int.MaxValue + 42L;

        var result = value.ToFormattedString();

        Assert.AreEqual("2 147 483 689", result);
    }

    [TestMethod]
    [Culture("fr")]
    public void ToFormattedString_WithNegativeNumber_FormatsCorrectly()
    {
        var value = -1234567;

        var result = value.ToFormattedString();

        Assert.AreEqual("-1 234 567", result);
    }

    [TestMethod]
    [Culture("fr")]
    public void ToFormattedString_WithZero_FormatsCorrectly()
    {
        var value = 0;

        var result = value.ToFormattedString();

        Assert.AreEqual("0", result);
    }

    #endregion
}
