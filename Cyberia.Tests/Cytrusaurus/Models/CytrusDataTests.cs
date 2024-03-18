using Cyberia.Cytrusaurus.Models;

using System.Text;

namespace Cyberia.Tests.Cytrusaurus.Models;

[TestClass]
public sealed class CytrusDataTests
{
    internal const string JSON = """
        {
          "version": 6,
          "name": "production",
          "games": {
            "dofus": {
              "name": "Dofus",
              "order": 1,
              "gameId": 1,
              "assets": {
                "meta": {
                  "beta": "d28d6cdb0550117ad7c88b88772ce110a7b1d0e3",
                  "main": "3f551abf0f6f56febbbd2fd27350943f527e6056"
                }
              },
              "platforms": {
                "darwin": {
                  "beta": "6.0_2.71.3.11",
                  "main": "6.0_2.70.7.12"
                },
                "linux": {
                  "beta": "6.0_2.71.3.11",
                  "main": "6.0_2.70.7.12"
                },
                "windows": {
                  "beta": "6.0_2.71.3.11",
                  "main": "6.0_2.70.7.12"
                }
              }
            }
          }
        }
        """;

    #region LoadFromFile

    [TestMethod]
    public void LoadFromFile_FileExists_LoadsDataCorrectly()
    {
        var tempFilePath = Path.GetTempFileName();
        File.WriteAllText(tempFilePath, JSON, Encoding.UTF8);

        try
        {
            var cytrusData = CytrusData.LoadFromFile(tempFilePath);

            Assert.IsNotNull(cytrusData);
            Assert.AreEqual(6, cytrusData.Version);
            Assert.AreEqual("production", cytrusData.Name);
            Assert.AreNotEqual(0, cytrusData.Games.Count);
        }
        finally
        {

            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }
        }
    }

    #endregion

    #region Load

    [TestMethod]
    public void Load_ValidJson_ReturnsCorrectCytrusData()
    {
        var cytrusData = CytrusData.Load(JSON);

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
        var cytrusData = CytrusData.Load(JSON);
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
        var cytrusData = CytrusData.Load(JSON);
        var game = cytrusData.GetGameByName("undefined");

        Assert.IsNull(game);
    }

    #endregion
}
