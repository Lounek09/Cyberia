using Cyberia.Cytrusaurus.Models;

namespace Cyberia.Tests.Cytrusaurus.Models;

[TestClass]
public sealed class CytrusDataTests
{
    private string _json = default!;

    [TestInitialize]
    public void Setup()
    {
        _json = File.ReadAllText(SharedData.CYTRUS_JSON_PATH);
    }


    #region LoadFromFile

    [TestMethod]
    public void LoadFromFile_FileExists_LoadsDataCorrectly()
    {
        var cytrusData = CytrusData.LoadFromFile(SharedData.CYTRUS_JSON_PATH);

        Assert.IsNotNull(cytrusData);
        Assert.AreEqual(6, cytrusData.Version);
        Assert.AreEqual("production", cytrusData.Name);
        Assert.AreNotEqual(0, cytrusData.Games.Count);
    }

    #endregion

    #region Load

    [TestMethod]
    public void Load_ValidJson_ReturnsCorrectCytrusData()
    {
        var cytrusData = CytrusData.Load(_json);

        Assert.IsNotNull(cytrusData);
        Assert.AreEqual(6, cytrusData.Version);
        Assert.AreEqual("production", cytrusData.Name);
        Assert.AreNotEqual(0, cytrusData.Games.Count);
    }

    #endregion

    #region GetGameByName

    [TestMethod]
    public void GetGameByName_ValidGameName_ReturnsGame()
    {
        var cytrusData = CytrusData.Load(_json);
        var game = cytrusData.GetGameByName("dofus");

        Assert.IsNotNull(game);
        Assert.AreEqual("Dofus", game.Name);
        Assert.AreEqual(1, game.Order);
        Assert.AreEqual(1, game.GameId);
        Assert.AreNotEqual(0, game.Assets.Count);
        Assert.AreNotEqual(0, game.Platforms.Count);
    }

    [TestMethod]
    public void GetGameByName_InvalidGameName_ReturnsNull()
    {
        var cytrusData = CytrusData.Load(_json);
        var game = cytrusData.GetGameByName("undefined");

        Assert.IsNull(game);
    }

    #endregion
}
