using Cyberia.Cytrusaurus.Models;

namespace Cyberia.Tests.Cytrusaurus.Models;

[TestClass]
public sealed class CytrusGameTests
{
    private CytrusGame _game = default!;

    [TestInitialize]
    public void Setup()
    {
        var cytrus = Cytrus.LoadFromFile(SharedData.CytrusJsonPath);
        _game = cytrus.Games["dofus"];
    }

    #region GetAssetsByName

    [TestMethod]
    public void GetAssetsByName_ValidAssetsName_ReturnsAssets()
    {
        var assets = _game.GetAssetsByName(CytrusGame.MetaAssets);

        Assert.AreEqual(2, assets.Count);
        Assert.IsTrue(assets.ContainsKey(CytrusGame.BetaRelease));
    }

    [TestMethod]
    public void GetAssetsByName_InvalidAssetsName_ReturnsEmptyAssets()
    {
        var assets = _game.GetAssetsByName("undefined");

        Assert.AreEqual(0, assets.Count);
    }

    #endregion

    #region GetAssetHashByNameAndReleaseName

    [TestMethod]
    public void GetAssetHashByNameAndReleaseName_ValidInputs_ReturnsHash()
    {
        var hash = _game.GetAssetHashByNameAndReleaseName(CytrusGame.MetaAssets, CytrusGame.BetaRelease);

        Assert.AreEqual("d28d6cdb0550117ad7c88b88772ce110a7b1d0e3", hash);
    }

    [TestMethod]
    public void GetAssetHashByNameAndReleaseName_InvalidAssetsName_ReturnsEmptyString()
    {
        var hash = _game.GetAssetHashByNameAndReleaseName("undefined", CytrusGame.BetaRelease);

        Assert.AreEqual(string.Empty, hash);
    }

    [TestMethod]
    public void GetAssetHashByNameAndReleaseName_InvalidReleaseName_ReturnsEmptyString()
    {
        var hash = _game.GetAssetHashByNameAndReleaseName(CytrusGame.MetaAssets, "undefined");

        Assert.AreEqual(string.Empty, hash);
    }

    #endregion

    #region GetReleasesByPlatformName

    [TestMethod]
    public void GetReleasesByPlatformName_ValidPlatformName_ReturnsReleases()
    {
        var releases = _game.GetReleasesByPlatformName(CytrusGame.WindowsPlatform);

        Assert.AreEqual(2, releases.Count);
        Assert.IsTrue(releases.ContainsKey(CytrusGame.BetaRelease));
    }

    [TestMethod]
    public void GetReleasesByPlatformName_InvalidPlatformName_ReturnsEmptyDictionary()
    {
        var releases = _game.GetReleasesByPlatformName("undefined");

        Assert.AreEqual(0, releases.Count);
    }

    #endregion

    #region GetVersionByPlatformNameAndReleaseName

    [TestMethod]
    public void GetVersionByPlatformNameAndReleaseName_ValidInputs_ReturnsVersion()
    {
        var version = _game.GetVersionByPlatformNameAndReleaseName(CytrusGame.WindowsPlatform, CytrusGame.BetaRelease);

        Assert.AreEqual("6.0_2.71.3.12", version);
    }

    [TestMethod]
    public void GetVersionByPlatformNameAndReleaseName_InvalidPlatformName_ReturnsEmptyString()
    {
        var version = _game.GetVersionByPlatformNameAndReleaseName("undefined", CytrusGame.BetaRelease);

        Assert.AreEqual(string.Empty, version);
    }

    [TestMethod]
    public void GetVersionByPlatformNameAndReleaseName_InvalidReleaseName_ReturnsEmptyString()
    {
        var version = _game.GetVersionByPlatformNameAndReleaseName(CytrusGame.WindowsPlatform, "undefined");

        Assert.AreEqual(string.Empty, version);
    }

    #endregion
}
