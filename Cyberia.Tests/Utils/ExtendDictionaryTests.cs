namespace Cyberia.Tests.Utils;

[TestClass]
public sealed class ExtendDictionaryTests
{
    #region RemoveByValue

    [TestMethod]
    public void RemoveByValue_SingleValue_RemovesCorrectly()
    {
        Dictionary<int, string> value = new()
        {
            { 1, "Emeraude" },
            { 2, "Ebene" },
            { 3, "Ivoire" }
        };

        var result = value.RemoveByValue("Ebene");

        Assert.IsTrue(result);
        Assert.IsFalse(value.ContainsValue("Ebene"));
    }

    [TestMethod]
    public void RemoveByValue_MultipleValues_RemovesAllInstances()
    {
        Dictionary<int, string> value = new()
        {
            { 1, "Emeraude" },
            { 2, "Pourpre" },
            { 3, "Pourpre" }
        };

        var result = value.RemoveByValue("Pourpre");

        Assert.IsTrue(result);
        Assert.IsFalse(value.ContainsValue("Pourpre"));
    }

    [TestMethod]
    public void RemoveByValue_FirstOnly_RemovesSingleInstance()
    {
        Dictionary<int, string> value = new()
        {
            { 1, "Emeraude" },
            { 2, "Ocre" },
            { 3, "Ocre" }
        };

        var result = value.RemoveByValue("Ocre", firstOnly: true);

        Assert.IsTrue(result);
        Assert.AreEqual(2, value.Count);
        Assert.IsTrue(value.ContainsValue("Ocre"));
    }

    [TestMethod]
    public void RemoveByValue_ValueDoesNotExist_ReturnsFalse()
    {
        Dictionary<int, string> value = new()
        {
            { 1, "Emeraude" },
            { 2, "Turquoise" },
            { 3, "Ivoire" }
        };

        var result = value.RemoveByValue("Ebene");

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void RemoveByValue_NullValueInDictionary_RemovesCorrectly()
    {
        Dictionary<int, string?> value = new()
        {
            { 1, "Emeraude" },
            { 2, null },
            { 3, "Turquoise" }
        };

        var result = value.RemoveByValue(null);

        Assert.IsTrue(result);
        Assert.IsFalse(value.ContainsValue(null));
    }

    #endregion
}
