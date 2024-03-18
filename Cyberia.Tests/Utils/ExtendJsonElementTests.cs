using System.Text.Json;

namespace Cyberia.Tests.Utils;

[TestClass]
public class ExtendJsonElementTests
{
    #region GetInt32OrDefault

    [TestMethod]
    public void GetInt32OrDefault_WithValue_ReturnsValue()
    {
        var json = "{\"number\": 123}";
        var value = JsonDocument.Parse(json).RootElement.GetProperty("number");

        var result = value.GetInt32OrDefault();

        Assert.AreEqual(123, result);
    }

    [TestMethod]
    public void GetInt32OrDefault_WithNonNumber_ReturnsDefault()
    {
        var json = "{\"number\": \"Bouftou\"}";
        var value = JsonDocument.Parse(json).RootElement.GetProperty("number");

        var result = value.GetInt32OrDefault();

        Assert.AreEqual(default, result);
    }

    [TestMethod]
    public void GetInt32OrDefault_WithNumberExceedingIntMaxValue_ReturnsDefault()
    {
        var json = $"{{\"number\": {((long)int.MaxValue) + 1}}}";
        var value = JsonDocument.Parse(json).RootElement.GetProperty("number");

        var result = value.GetInt32OrDefault();

        Assert.AreEqual(default, result);
    }

    #endregion

    #region GetInt64OrDefault

    [TestMethod]
    public void GetInt64OrDefault_WithValue_ReturnsValue()
    {
        var json = "{\"number\": 1234567890123}";
        var value = JsonDocument.Parse(json).RootElement.GetProperty("number");

        var result = value.GetInt64OrDefault();

        Assert.AreEqual(1234567890123L, result);
    }

    [TestMethod]
    public void GetInt64OrDefault_WithNonNumber_ReturnsDefault()
    {
        var json = "{\"number\": \"Bouftou\"}";
        var value = JsonDocument.Parse(json).RootElement.GetProperty("number");

        var result = value.GetInt64OrDefault();

        Assert.AreEqual(default, result);
    }

    #endregion

    #region GetStringOrEmpty

    [TestMethod]
    public void GetStringOrEmpty_WithValue_ReturnsValue()
    {
        var json = "{\"text\": \"Bouftou\"}";
        var value = JsonDocument.Parse(json).RootElement.GetProperty("text");

        var result = value.GetStringOrEmpty();

        Assert.AreEqual("Bouftou", result);
    }

    [TestMethod]
    public void GetStringOrEmpty_WithNonString_ReturnsEmpty()
    {
        var json = "{\"text\": 123}";
        var value = JsonDocument.Parse(json).RootElement.GetProperty("text");

        var result = value.GetStringOrEmpty();

        Assert.AreEqual(string.Empty, result);
    }

    #endregion
}
