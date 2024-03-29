using Cyberia.Cytrusaurus.Models;

namespace Cyberia.Tests.Cytrusaurus.Models;

[TestClass]
public sealed class CytrusTests
{
    #region LoadFromFile

    [TestMethod]
    public void LoadFromFile_FileExists_LoadsDataCorrectly()
    {
        var cytrus = Cytrus.LoadFromFile(SharedData.CYTRUS_JSON_PATH);

        Assert.IsNotNull(cytrus);
        Assert.AreEqual(6, cytrus.Version);
        Assert.AreEqual("production", cytrus.Name);
        Assert.AreEqual(9, cytrus.Games.Count);
    }

    #endregion

    #region Load

    [TestMethod]
    public void Load_ValidJson_ReturnsCorrectCytrus()
    {
        var json = File.ReadAllText(SharedData.CYTRUS_JSON_PATH);
        var cytrus = Cytrus.Load(json);

        Assert.IsNotNull(cytrus);
        Assert.AreEqual(6, cytrus.Version);
        Assert.AreEqual("production", cytrus.Name);
        Assert.AreEqual(9, cytrus.Games.Count);
    }

    #endregion

    #region GetGameByName

    [TestMethod]
    public void GetGameByName_ValidGameName_ReturnsGame()
    {
        var cytrus = Cytrus.LoadFromFile(SharedData.CYTRUS_JSON_PATH);
        var game = cytrus.GetGameByName("dofus");

        Assert.IsNotNull(game);
        Assert.AreEqual("Dofus", game.Name);
        Assert.AreEqual(1, game.Order);
        Assert.AreEqual(1, game.GameId);
        Assert.AreEqual(1, game.Assets.Count);
        Assert.AreEqual(3, game.Platforms.Count);
    }

    [TestMethod]
    public void GetGameByName_InvalidGameName_ReturnsNull()
    {
        var cytrus = Cytrus.LoadFromFile(SharedData.CYTRUS_JSON_PATH);
        var game = cytrus.GetGameByName("undefined");

        Assert.IsNull(game);
    }

    #endregion
}
