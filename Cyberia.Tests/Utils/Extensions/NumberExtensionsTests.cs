using Cyberia.Tests.Attributes;
using Cyberia.Tests.Common;

namespace Cyberia.Tests.Utils.Extensions;

[TestClass]
public sealed class NumberExtensionsTests : CultureAwareTest
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

    #region Length

    [TestMethod]
    public void Length_WithSingleDigit_Returns1()
    {
        var value = 5;

        var result = value.Length();

        Assert.AreEqual(1, result);
    }

    [TestMethod]
    public void Length_WithTwoDigits_Returns2()
    {
        var value = 42;

        var result = value.Length();

        Assert.AreEqual(2, result);
    }

    [TestMethod]
    public void Length_WithNegativeNumber_ReturnsCorrectLength()
    {
        var value = -1234;

        var result = value.Length();

        Assert.AreEqual(5, result);
    }

    [TestMethod]
    public void Length_WithMaxValue_ReturnsCorrectLength()
    {
        var value = int.MaxValue;

        var result = value.Length();

        Assert.AreEqual(10, result);
    }

    [TestMethod]
    public void Length_WithMinValue_ReturnsCorrectLength()
    {
        var value = int.MinValue;

        var result = value.Length();

        Assert.AreEqual(11, result);
    }

    #endregion
}
