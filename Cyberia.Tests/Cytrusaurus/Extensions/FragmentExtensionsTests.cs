using Cyberia.Cytrusaurus.Extensions;
using Cyberia.Cytrusaurus.Models.FlatBuffers;

using Google.FlatBuffers;

namespace Cyberia.Tests.Cytrusaurus.Extensions;

[TestClass]
public sealed class FragmentExtensionsTests
{
    private Manifest _currentManifest = default!;
    private Manifest _modelManifest = default!;

    [TestInitialize]
    public void Initialize()
    {
        var bytes = File.ReadAllBytes(SharedData.CurrentNanifestPath);
        ByteBuffer buffer = new(bytes);
        _currentManifest = Manifest.GetRootAsManifest(buffer);

        bytes = File.ReadAllBytes(SharedData.ModelManifestPath);
        buffer = new(bytes);
        _modelManifest = Manifest.GetRootAsManifest(buffer);
    }

    #region GetGameFiles

    [TestMethod]
    public void GetGameFiles_ReturnsCorrectNumberOfGameFiles()
    {
        var fragment = _currentManifest.Fragments(0)!.Value;
        var gameFiles = fragment.GetGameFiles();

        Assert.AreEqual(fragment.FilesLength, gameFiles.Count());
    }

    #endregion

    #region Diff

    [TestMethod]
    public void Diff_WithDifferentFragment_ReturnsCorrectDifferences()
    {
        var currentFragment = _currentManifest.Fragments(0)!.Value;
        var modelFragment = _modelManifest.Fragments(0)!.Value;

        var diff = currentFragment.Diff(modelFragment);

        var expected = File.ReadAllText(SharedData.FragmentDiffPath);

        Assert.AreEqual(expected, diff);
    }

    #endregion
}
