using System.Text.Json.Nodes;

namespace Cyberia.Tests.Utils;

[TestClass]
public class JsonTests
{
    private const string CURRENT_JSON = """
        {
            "name": "Fallanster",
            "age": 40
        }
        """;

    private const string MODEL_JSON = """
        {
            "name": "Allisteria",
            "age": 18
        }
        """;


    #region Diff JsonNode

    [TestMethod]
    public void Diff_WithEqualJsonNodes_ReturnsEmptyJsonObject()
    {
        var value = JsonNode.Parse(CURRENT_JSON)!;

        var result = value.Diff(value);

        Assert.AreEqual(0, result.AsObject().Count);
    }

    [TestMethod]
    public void Diff_WithDifferentJsonNodes_ReturnsDifferences()
    {
        var current = JsonNode.Parse(CURRENT_JSON)!;
        var model = JsonNode.Parse(MODEL_JSON)!;

        var result = current.Diff(model);

        Assert.IsTrue(result.AsObject().ContainsKey("name"));
        Assert.IsTrue(result.AsObject().ContainsKey("age"));
    }

    #endregion

    #region Diff string

    [TestMethod]
    public void DiffString_WithEqualJsonStrings_ReturnsEmptyString()
    {
        var result = Json.Diff(CURRENT_JSON, CURRENT_JSON);

        Assert.AreEqual(string.Empty, result);
    }

    [TestMethod]
    public void DiffString_WithDifferentJsonStrings_ReturnsDifferences()
    {
        var result = Json.Diff(CURRENT_JSON, MODEL_JSON);

        Assert.IsFalse(string.IsNullOrEmpty(result));
        Assert.IsTrue(result.Contains("name"));
        Assert.IsTrue(result.Contains("age"));
    }

    [TestMethod]
    public void DiffString_WithInvalidJson_ReturnsEmptyString()
    {
        var invalidJson = "This is not a JSON string";
        var validJson = MODEL_JSON;

        var resultFromInvalidCurrent = Json.Diff(invalidJson, validJson);
        var resultFromInvalidModel = Json.Diff(validJson, invalidJson);

        Assert.AreEqual(string.Empty, resultFromInvalidCurrent);
        Assert.AreEqual(string.Empty, resultFromInvalidModel);
    }

    #endregion
}
