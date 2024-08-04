using Cyberia.Langzilla;
using Cyberia.Langzilla.Enums;

using System.Net;

namespace Cyberia.Tests.Langzilla.Models;

[TestClass]
public sealed class LangTests
{
    private Lang _lang = default!;

    [TestInitialize]
    public void Setup()
    {
        _lang = new("ranks", 1178, LangType.Official, LangLanguage.fr);

        LangsWatcher.Initialize();
        LangsWatcher.HttpRetryPolicy = SharedData.HttpRetryPolicy;
    }

    [TestCleanup]
    public void Cleanup()
    {
        if (Directory.Exists(LangsWatcher.OutputPath))
        {
            Directory.Delete(LangsWatcher.OutputPath, true);
        }
    }

    #region DownloadAync

    [TestMethod]
    public async Task DownloadAsync_WhithSuccessfullResponse_DownloadsAndSaveFileCorrectly()
    {
        var mock = SharedData.SetupMockHttpMessageHandlerForSuccessfullResponse(
            new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new ByteArrayContent(File.ReadAllBytes(SharedData.RanksSwfPath))
            }
        );

        LangsWatcher.HttpClient = new HttpClient(mock.Object)
        {
            BaseAddress = new Uri(LangsWatcher.BaseUrl)
        };

        var success = await _lang.DownloadAsync();

        Assert.IsTrue(success);
        Assert.IsTrue(File.Exists(_lang.FilePath));
    }

    public async Task DownloadAsync_WithException_ReturnsFalse()
    {
        var mock = SharedData.SetupMockHttpMessageHandlerForException(new HttpRequestException());

        LangsWatcher.HttpClient = new HttpClient(mock.Object)
        {
            BaseAddress = new Uri(LangsWatcher.BaseUrl)
        };

        var success = await _lang.DownloadAsync();

        Assert.IsFalse(success);
    }

    #endregion

    #region Extract

    [TestMethod]
    public void Extract_WhenFileExists_ExtractionSuccessful()
    {
        Directory.CreateDirectory(_lang.OutputPath);
        File.WriteAllBytes(_lang.FilePath, File.ReadAllBytes(SharedData.RanksSwfPath));
        var expected = File.ReadAllLines(SharedData.RanksCurrentPath);

        var success = _lang.Extract();
        var result = File.ReadAllLines(_lang.CurrentDecompiledFilePath);

        Assert.IsTrue(success);
        Assert.IsTrue(expected.SequenceEqual(result));
    }

    [TestMethod]
    public void Extract_WhenNoFileExists_ExtractionSuccessful()
    {
        var success = _lang.Extract();

        Assert.IsFalse(success);
    }

    #endregion

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
