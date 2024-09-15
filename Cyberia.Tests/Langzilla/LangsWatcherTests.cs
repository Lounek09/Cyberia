using Cyberia.Langzilla;
using Cyberia.Langzilla.Enums;
using Cyberia.Langzilla.Models;

using System.Net;

namespace Cyberia.Tests.Langzilla;

[TestClass]
public sealed class LangsWatcherTests
{
    private LangsWatcher _langsWatcher = default!;
    private string _versions = default!;
    private Lang _lang = default!;

    [TestInitialize]
    public void Setup()
    {
        _langsWatcher = new()
        {
            HttpRetryPolicy = SharedData.HttpRetryPolicy
        };
        _versions = File.ReadAllText(SharedData.VersionsPath);
        _lang = new("ranks", 1178, LangType.Official, LangLanguage.fr);
    }

    [TestCleanup]
    public void Cleanup()
    {
        if (Directory.Exists(LangsWatcher.OutputPath))
        {
            Directory.Delete(LangsWatcher.OutputPath, true);
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
        var result = LangsWatcher.GetOutputPath(LangType.Official, LangLanguage.fr);

        Assert.AreEqual(Path.Join(LangsWatcher.OutputPath, "official", "fr"), result);
    }

    #endregion

    #region FetchVersionsAsync

    [TestMethod]
    public async Task FetchVersionsAsync_LastMofiedNewer_ReturnsVersions()
    {
        var mock = SharedData.SetupMockHttpMessageHandlerForSuccessfullResponse(
            new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(_versions)
                {
                    Headers =
                    {
                        LastModified = new DateTimeOffset(DateTime.Parse("2024-03-19T08:25:00Z"))
                    }
                }
            }
        );

        _langsWatcher.HttpClient = new HttpClient(mock.Object)
        {
            BaseAddress = new Uri(LangsWatcher.BaseUrl)
        };

        var repository = LangsRepository.LoadFromFile(SharedData.LangRepositoryDataPath);
        var version = await _langsWatcher.FetchVersionsAsync(repository);

        Assert.AreEqual(_versions, version);
        Assert.IsTrue(File.Exists(repository.VersionFilePath));
    }

    [TestMethod]
    public async Task FetchVersionsAsync_LastMofiedOlder_ReturnsVersions()
    {
        var mock = SharedData.SetupMockHttpMessageHandlerForSuccessfullResponse(
            new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(_versions)
                {
                    Headers =
                    {
                        LastModified = new DateTimeOffset(DateTime.Parse("2024-03-19T08:24:00Z"))
                    }
                }
            }
        );

        _langsWatcher.HttpClient = new HttpClient(mock.Object)
        {
            BaseAddress = new Uri(LangsWatcher.BaseUrl)
        };

        var repository = LangsRepository.LoadFromFile(SharedData.LangRepositoryDataPath);
        var version = await _langsWatcher.FetchVersionsAsync(repository);

        Assert.AreNotEqual(_versions, version);
        Assert.IsFalse(File.Exists(repository.VersionFilePath));
    }

    [TestMethod]
    public async Task FetchVersionsAsync_ForcedWithLastModifiedOlder_ReturnsVersions()
    {
        var mock = SharedData.SetupMockHttpMessageHandlerForSuccessfullResponse(
            new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(_versions)
                {
                    Headers =
                    {
                        LastModified = new DateTimeOffset(DateTime.Parse("2024-03-19T08:24:00Z"))
                    }
                }
            }
        );

        _langsWatcher.HttpClient = new HttpClient(mock.Object)
        {
            BaseAddress = new Uri(LangsWatcher.BaseUrl)
        };

        var repository = LangsRepository.LoadFromFile(SharedData.LangRepositoryDataPath);
        var version = await _langsWatcher.FetchVersionsAsync(repository, true);

        Assert.AreEqual(_versions, version);
        Assert.IsTrue(File.Exists(repository.VersionFilePath));
    }

    #endregion

    #region GetUpdatedLangs

    [TestMethod]
    public void GetUpdatedLangs_ValidVersions_ReturnsUpdatedLangs()
    {
        var repository = LangsRepository.LoadFromFile(SharedData.LangRepositoryDataPath);
        var langs = LangsWatcher.GetUpdatedLangs(repository, _versions);

        Assert.AreEqual(38, langs.Count());
    }

    [TestMethod]
    public void GetUpdatedLangs_InvalidVersions_ReturnsUpdatedLangs()
    {
        var repository = LangsRepository.LoadFromFile(SharedData.LangRepositoryDataPath);
        var langs = LangsWatcher.GetUpdatedLangs(repository, string.Empty);

        Assert.IsFalse(langs.Any());
    }

    #endregion

    #region DownloadLangAync

    [TestMethod]
    public async Task DownloadLangAync_WhithSuccessfullResponse_DownloadsAndSaveFileCorrectly()
    {
        var mock = SharedData.SetupMockHttpMessageHandlerForSuccessfullResponse(
            new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new ByteArrayContent(File.ReadAllBytes(SharedData.RanksSwfPath))
            }
        );

        _langsWatcher.HttpClient = new HttpClient(mock.Object)
        {
            BaseAddress = new Uri(LangsWatcher.BaseUrl)
        };

        var success = await _langsWatcher.DownloadLangAsync(_lang);

        Assert.IsTrue(success);
        Assert.IsTrue(File.Exists(_lang.FilePath));
    }

    public async Task DownloadLangAync_WithException_ReturnsFalse()
    {
        var mock = SharedData.SetupMockHttpMessageHandlerForException(new HttpRequestException());

        _langsWatcher.HttpClient = new HttpClient(mock.Object)
        {
            BaseAddress = new Uri(LangsWatcher.BaseUrl)
        };

        var success = await _langsWatcher.DownloadLangAsync(_lang);

        Assert.IsFalse(success);
    }

    #endregion

    #region ExtractLang

    [TestMethod]
    public void ExtractLang_WhenFileExists_ExtractionSuccessful()
    {
        Directory.CreateDirectory(_lang.OutputPath);
        File.WriteAllBytes(_lang.FilePath, File.ReadAllBytes(SharedData.RanksSwfPath));
        var expected = File.ReadAllLines(SharedData.RanksCurrentPath);

        var success = LangsWatcher.ExtractLang(_lang);
        var result = File.ReadAllLines(_lang.CurrentDecompiledFilePath);

        Assert.IsTrue(success);
        Assert.IsTrue(expected.SequenceEqual(result));
    }

    [TestMethod]
    public void ExtractLang_WhenNoFileExists_ExtractionSuccessful()
    {
        var success = LangsWatcher.ExtractLang(_lang);

        Assert.IsFalse(success);
    }

    #endregion
}
