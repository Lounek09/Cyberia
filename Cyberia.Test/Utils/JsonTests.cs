using System.Text.Json.Nodes;

namespace Cyberia.Test.Utils;

[TestClass]
public class JsonTests
{
    #region Diff JsonNode

    [TestMethod]
    public void Diff_WithEqualJsonNodes_ReturnsEmptyJsonObject()
    {
        var value = JsonNode.Parse("{\"name\":\"Fallanster\", \"age\":40}")!;

        var result = value.Diff(value);

        Assert.AreEqual(0, result.AsObject().Count);
    }

    [TestMethod]
    public void Diff_WithDifferentJsonNodes_ReturnsDifferences()
    {
        var current = JsonNode.Parse("{\"name\":\"Fallanster\", \"age\":40}")!;
        var model = JsonNode.Parse("{\"name\":\"Allisteria\", \"age\":18}")!;

        var result = current.Diff(model);

        Assert.IsTrue(result.AsObject().ContainsKey("name"));
        Assert.IsTrue(result.AsObject().ContainsKey("age"));
    }

    #endregion

    #region Diff string

    [TestMethod]
    public void DiffString_WithEqualJsonStrings_ReturnsEmptyString()
    {
        var value = "{\"name\":\"Fallanster\", \"age\":40}";

        var result = Json.Diff(value, value);

        Assert.AreEqual(string.Empty, result);
    }

    [TestMethod]
    public void DiffString_WithDifferentJsonStrings_ReturnsDifferences()
    {
        var current = "{\"name\":\"Fallanster\", \"age\":40}";
        var model = "{\"name\":\"Allisteria\", \"age\":18}";

        var result = Json.Diff(current, model);

        Assert.IsFalse(string.IsNullOrEmpty(result));
        Assert.IsTrue(result.Contains("\"name\""));
        Assert.IsTrue(result.Contains("\"age\""));
    }

    [TestMethod]
    public void DiffString_WithInvalidJson_ReturnsEmptyString()
    {
        var invalidJson = "This is not a JSON string";
        var validJson = "{\"name\":\"Allisteria\", \"age\":18}";

        var resultFromInvalidCurrent = Json.Diff(invalidJson, validJson);
        var resultFromInvalidModel = Json.Diff(validJson, invalidJson);

        Assert.AreEqual(string.Empty, resultFromInvalidCurrent);
        Assert.AreEqual(string.Empty, resultFromInvalidModel);
    }

    #endregion
}
