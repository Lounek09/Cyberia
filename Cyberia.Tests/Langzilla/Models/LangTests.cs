using Cyberia.Langzilla.Enums;
using Cyberia.Langzilla.Models;

namespace Cyberia.Tests.Langzilla.Models;

[TestClass]
public sealed class LangTests
{
    private Lang _lang = default!;

    [TestInitialize]
    public void Setup()
    {
        _lang = new("ranks", 1178, LangType.Official, LangLanguage.fr);
    }

    #region Diff

    [TestMethod]
    public void Diff_WhenCurrentAndOldExtractedFileExist_ReturnsTrueAndWritesDiffFile()
    {
        Directory.CreateDirectory(_lang.OutputPath);
        File.WriteAllText(_lang.CurrentDecompiledFilePath, File.ReadAllText(SharedData.RanksCurrentPath));
        File.WriteAllText(_lang.OldDecompiledFilePath, File.ReadAllText(SharedData.RanksOldPath));
        var expected = File.ReadAllText(SharedData.RanksDiffPath);

        var success = _lang.SelfDiff();
        var result = File.ReadAllText(_lang.DiffFilePath);

        Assert.IsTrue(success);
        Assert.IsTrue(File.Exists(_lang.DiffFilePath));
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Diff_WhenCurrentAndOldExtractedFileExistButAreTheSame_ReturnsTrueAndDeleteDiffFile()
    {
        Directory.CreateDirectory(_lang.OutputPath);
        File.WriteAllText(_lang.CurrentDecompiledFilePath, File.ReadAllText(SharedData.RanksCurrentPath));
        File.WriteAllText(_lang.OldDecompiledFilePath, File.ReadAllText(SharedData.RanksCurrentPath));
        File.WriteAllText(_lang.DiffFilePath, File.ReadAllText(SharedData.RanksDiffPath));

        var success = _lang.SelfDiff();

        Assert.IsFalse(success);
        Assert.IsFalse(File.Exists(_lang.DiffFilePath));
    }

    [TestMethod]
    public void Diff_WhenNoCurrentExtractedFileExists_ReturnsFalse()
    {
        var result = _lang.SelfDiff();

        Assert.IsFalse(result);
        Assert.IsFalse(File.Exists(_lang.DiffFilePath));
    }

    #endregion
}
