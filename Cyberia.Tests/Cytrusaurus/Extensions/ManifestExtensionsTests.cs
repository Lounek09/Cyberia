using Cyberia.Cytrusaurus.Extensions;
using Cyberia.Cytrusaurus.Models.FlatBuffers;

using Google.FlatBuffers;

namespace Cyberia.Tests.Cytrusaurus.Extensions;

[TestClass]
public sealed class ManifestExtensionsTests
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

    #region GetFragments

    [TestMethod]
    public void GetFragments_ReturnsCorrectNumberOfFragments()
    {
        var fragments = _currentManifest.GetFragments();

        Assert.AreEqual(_currentManifest.FragmentsLength, fragments.Count());
    }

    #endregion

    #region Diff

    [TestMethod]
    public void Diff_WhithDifferentManifest_ReturnsCorrectDifferences()
    {
        var diff = _currentManifest.Diff(_modelManifest);

        var expected = File.ReadAllText(SharedData.ManifestDiffPath);

        Assert.AreEqual(expected, diff);
    }

    #endregion
}
