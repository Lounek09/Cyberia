using System.Text.Json.Nodes;

namespace Cyberia.Tests.Utils;

[TestClass]
public class JsonTests
{
    private const string c_currentJson =
    """
    {
        "name": "Fallanster",
        "age": 40
    }
    """;

    private const string c_modelJson =
    """
    {
        "name": "Allisteria",
        "age": 18
    }
    """;

    #region Diff string

    [TestMethod]
    public void DiffString_WithEqualJsonStrings_ReturnsEmptyString()
    {
        var result = Json.Diff(c_currentJson, c_currentJson);

        Assert.AreEqual(string.Empty, result);
    }

    [TestMethod]
    public void DiffString_WithDifferentJsonStrings_ReturnsDifferences()
    {
        var result = Json.Diff(c_currentJson, c_modelJson);

        Assert.IsFalse(string.IsNullOrEmpty(result));
        Assert.Contains(result, "name");
        Assert.Contains(result, "age");
    }

    [TestMethod]
    public void DiffString_WithInvalidJson_ReturnsEmptyString()
    {
        var invalidJson = "This is not a JSON string";

        var resultFromInvalidCurrent = Json.Diff(invalidJson, c_modelJson);
        var resultFromInvalidModel = Json.Diff(c_modelJson, invalidJson);

        Assert.AreEqual(string.Empty, resultFromInvalidCurrent);
        Assert.AreEqual(string.Empty, resultFromInvalidModel);
    }

    #endregion

    #region Diff JsonNode

    [TestMethod]
    public void Diff_WithEqualJsonNodes_ReturnsEmptyJsonObject()
    {
        var value = JsonNode.Parse(c_currentJson)!;

        var result = Json.Diff(value, value);

        Assert.AreEqual(0, result.AsObject().Count);
    }

    [TestMethod]
    public void Diff_WithDifferentJsonNodes_ReturnsDifferences()
    {
        var current = JsonNode.Parse(c_currentJson)!;
        var model = JsonNode.Parse(c_modelJson)!;

        var result = Json.Diff(current, model);

        Assert.IsTrue(result.AsObject().ContainsKey("name"));
        Assert.IsTrue(result.AsObject().ContainsKey("age"));
    }

    #endregion
}
