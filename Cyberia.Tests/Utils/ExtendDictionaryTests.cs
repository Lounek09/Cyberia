namespace Cyberia.Tests.Utils;

[TestClass]
public sealed class ExtendDictionaryTests
{
    #region RemoveByValue

    [TestMethod]
    public void RemoveByValue_WhithSingleValue_RemovesCorrectly()
    {
        Dictionary<int, string> value = new()
        {
            { 1, "Emeraude" },
            { 2, "Ebene" },
            { 3, "Ivoire" }
        };

        var result = value.RemoveByValue("Ebene");

        Assert.IsTrue(result);
        Assert.AreEqual(2, value.Count);
        Assert.IsFalse(value.ContainsValue("Ebene"));
    }

    [TestMethod]
    public void RemoveByValue_WithMultipleValues_RemovesTheFirstInstance()
    {
        Dictionary<int, string> value = new()
        {
            { 1, "Emeraude" },
            { 2, "Pourpre" },
            { 3, "Pourpre" }
        };

        var result = value.RemoveByValue("Pourpre");

        Assert.IsTrue(result);
        Assert.AreEqual(2, value.Count);
        Assert.IsTrue(value.ContainsValue("Pourpre"));
    }

    [TestMethod]
    public void RemoveByValue_WhenValueDoesNotExist_ReturnsFalse()
    {
        Dictionary<int, string> value = new()
        {
            { 1, "Emeraude" },
            { 2, "Turquoise" },
            { 3, "Ivoire" }
        };

        var result = value.RemoveByValue("Ebene");

        Assert.IsFalse(result);
        Assert.AreEqual(3, value.Count);
    }

    [TestMethod]
    public void RemoveByValue_WithNullValue_RemovesCorrectly()
    {
        Dictionary<int, string?> value = new()
        {
            { 1, "Emeraude" },
            { 2, null },
            { 3, "Turquoise" }
        };

        var result = value.RemoveByValue(null);

        Assert.IsTrue(result);
        Assert.AreEqual(2, value.Count);
        Assert.IsFalse(value.ContainsValue(null));
    }

    #endregion

    #region RemoveAllByValue

    [TestMethod]
    public void RemoveAllByValue_WithSingleValue_RemovesCorrectly()
    {
        Dictionary<int, string> value = new()
        {
            { 1, "Emeraude" },
            { 2, "Ebene" },
            { 3, "Ivoire" }
        };

        var result = value.RemoveAllByValue("Ebene");

        Assert.IsTrue(result);
        Assert.AreEqual(2, value.Count);
        Assert.IsFalse(value.ContainsValue("Ebene"));
    }

    [TestMethod]
    public void RemoveAllByValue_WithMultipleValues_RemovesAllInstance()
    {
        Dictionary<int, string> value = new()
        {
            { 1, "Emeraude" },
            { 2, "Pourpre" },
            { 3, "Pourpre" }
        };

        var result = value.RemoveAllByValue("Pourpre");

        Assert.IsTrue(result);
        Assert.AreEqual(1, value.Count);
        Assert.IsFalse(value.ContainsValue("Pourpre"));
    }

    [TestMethod]
    public void RemoveAllByValue_WhenValueDoesNotExist_ReturnsFalse()
    {
        Dictionary<int, string> value = new()
        {
            { 1, "Emeraude" },
            { 2, "Turquoise" },
            { 3, "Ivoire" }
        };

        var result = value.RemoveAllByValue("Ebene");

        Assert.IsFalse(result);
        Assert.AreEqual(3, value.Count);
    }

    [TestMethod]
    public void RemoveAllByValue_WithNullValue_RemovesCorrectly()
    {
        Dictionary<int, string?> value = new()
        {
            { 1, "Emeraude" },
            { 2, null },
            { 3, null }
        };

        var result = value.RemoveAllByValue(null);

        Assert.IsTrue(result);
        Assert.AreEqual(1, value.Count);
        Assert.IsFalse(value.ContainsValue(null));
    }

    #endregion
}
