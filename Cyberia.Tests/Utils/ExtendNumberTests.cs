﻿namespace Cyberia.Tests.Utils;

[TestClass]
public sealed class ExtendNumberTests
{
    #region ToFormattedString

    [TestMethod]
    public void ToFormattedString_WithSmallNumber_FormatsCorrectly()
    {
        var value = 123;

        var result = value.ToFormattedString();

        Assert.AreEqual("123", result);
    }

    [TestMethod]
    public void ToFormattedString_WithLargeNumber_FormatsCorrectly()
    {
        var value = 1234567;

        var result = value.ToFormattedString();

        Assert.AreEqual("1 234 567", result);
    }

    [TestMethod]
    public void ToFormattedString_WithVeryLargeNumber_FormatsCorrectly()
    {
        var value = int.MaxValue + 42L;

        var result = value.ToFormattedString();

        Assert.AreEqual("2 147 483 689", result);
    }

    [TestMethod]
    public void ToFormattedString_WithNegativeNumber_FormatsCorrectly()
    {
        var value = -1234567;

        var result = value.ToFormattedString();

        Assert.AreEqual("-1 234 567", result);
    }

    [TestMethod]
    public void ToFormattedString_WithZero_FormatsCorrectly()
    {
        var value = 0;

        var result = value.ToFormattedString();

        Assert.AreEqual("0", result);
    }

    #endregion
}