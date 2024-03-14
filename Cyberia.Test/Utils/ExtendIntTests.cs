namespace Cyberia.Test.Utils;

[TestClass]
public sealed class ExtendIntTests
{
    #region ToStringThousandSeparator

    [TestMethod]
    public void ToStringThousandSeparator_WithSmallNumber_FormatsCorrectly()
    {
        var value = 123;

        var result = value.ToStringThousandSeparator();

        Assert.AreEqual("123", result);
    }

    [TestMethod]
    public void ToStringThousandSeparator_WithLargeNumber_FormatsCorrectly()
    {
        var value = 1234567;

        var result = value.ToStringThousandSeparator();

        Assert.AreEqual("1 234 567", result);
    }

    [TestMethod]
    public void ToStringThousandSeparator_WithNegativeNumber_FormatsCorrectly()
    {
        var value = -1234567;

        var result = value.ToStringThousandSeparator();

        Assert.AreEqual("-1 234 567", result);
    }

    [TestMethod]
    public void ToStringThousandSeparator_WithZero_FormatsCorrectly()
    {
        var value = 0;

        var result = value.ToStringThousandSeparator();

        Assert.AreEqual("0", result);
    }

    #endregion
}
