using Cyberia.Langzilla;
using Cyberia.Langzilla.Enums;

namespace Cyberia.Tests.Langzilla;

[TestClass]
public sealed class LangsWatcherTests
{
    [TestInitialize]
    public void Setup()
    {
        LangsWatcher.Initialize();
        LangsWatcher.HttpRetryPolicy = SharedData.HttpRetryPolicy;
    }

    [TestCleanup]
    public void Cleanup()
    {
        if (Directory.Exists(LangsWatcher.OUTPUT_PATH))
        {
            Directory.Delete(LangsWatcher.OUTPUT_PATH, true);
        }
    }

    #region GetRoute

    [TestMethod]
    public void GetRoute_WhenTypeIsOfficial_ReturnsDefaultRoute()
    {
        var result = LangsWatcher.GetRoute(LangType.Official);

        Assert.AreEqual("lang", result);
    }

    #endregion

    #region GetOutputPath

    [TestMethod]
    public void GetOutputPath_WhenTypeIsOfficialAndLanguageFrench_ReturnsCorrectPath()
    {
        var result = LangsWatcher.GetOutputPath(LangType.Official, LangLanguage.FR);

        Assert.AreEqual(Path.Join(LangsWatcher.OUTPUT_PATH, "official", "fr"), result);
    }

    #endregion
}
