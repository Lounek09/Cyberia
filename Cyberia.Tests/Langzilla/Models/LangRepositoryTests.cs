using Cyberia.Langzilla;
using Cyberia.Langzilla.Enums;

using System.Net;

namespace Cyberia.Tests.Langzilla.Models;

[TestClass]
public sealed class LangRepositoryTests
{
    private string _versions = default!;

    [TestInitialize]
    public void Setup()
    {
        _versions = File.ReadAllText(SharedData.VersionsPath);          

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

    #region LoadFromFile

    [TestMethod]
    public void LoadFromFile_FileExists_LoadsDataCorrectly()
    {
        var repository = LangRepository.LoadFromFile(SharedData.LangRepositoryDataPath);

        Assert.IsNotNull(repository);
        Assert.AreEqual(LangType.Official, repository.Type);
        Assert.AreEqual(LangLanguage.fr, repository.Language);
        Assert.AreEqual(DateTime.Parse("2024-03-19T08:24:23Z").ToUniversalTime(), repository.LastChange);
        Assert.AreNotEqual(0, repository.Langs.Count);
        Assert.AreEqual(Path.Join(LangsWatcher.OutputPath, "official", "fr"), repository.OutputPath);
        Assert.AreEqual("versions_fr.txt", repository.VersionFileName);
        Assert.AreEqual("lang/versions_fr.txt", repository.VersionFileRoute);
        Assert.AreEqual(Path.Join(LangsWatcher.OutputPath, "official", "fr", "versions_fr.txt"), repository.VersionFilePath);
    }

    #endregion

    #region Load

    [TestMethod]
    public void Load_ValidJson_ReturnsCorrectLangRepositoryData()
    {
        var json = File.ReadAllText(SharedData.LangRepositoryDataPath);
        var repository = LangRepository.Load(json);

        Assert.IsNotNull(repository);
        Assert.AreEqual(LangType.Official, repository.Type);
        Assert.AreEqual(LangLanguage.fr, repository.Language);
        Assert.AreEqual(DateTime.Parse("2024-03-19T08:24:23Z").ToUniversalTime(), repository.LastChange);
        Assert.AreNotEqual(0, repository.Langs.Count);
        Assert.AreEqual(Path.Join(LangsWatcher.OutputPath, "official", "fr"), repository.OutputPath);
        Assert.AreEqual("versions_fr.txt", repository.VersionFileName);
        Assert.AreEqual("lang/versions_fr.txt", repository.VersionFileRoute);
        Assert.AreEqual(Path.Join(LangsWatcher.OutputPath, "official", "fr", "versions_fr.txt"), repository.VersionFilePath);
    }

    #endregion

    #region GetByName

    [TestMethod]
    public void GetByName_ValidLangName_ReturnsLang()
    {
        var repository = LangRepository.LoadFromFile(SharedData.LangRepositoryDataPath);
        var lang = repository.GetByName("items");

        Assert.IsNotNull(lang);
        Assert.AreEqual("items", lang.Name);
        Assert.AreEqual(1184, lang.Version);
        Assert.AreEqual(LangType.Official, lang.Type);
        Assert.AreEqual(LangLanguage.fr, lang.Language);
        Assert.IsTrue(lang.New);
        Assert.AreEqual(Path.Join(LangsWatcher.OutputPath, "official", "fr", "items"), lang.OutputPath);
        Assert.AreEqual("items_fr_1184.swf", lang.FileName);
        Assert.AreEqual("lang/swf/items_fr_1184.swf", lang.FileRoute);
        Assert.AreEqual(Path.Join(LangsWatcher.OutputPath, "official", "fr", "items", "items_fr_1184.swf"), lang.FilePath);
        Assert.AreEqual(Path.Join(LangsWatcher.OutputPath, "official", "fr", "items", "current.as"), lang.CurrentDecompiledFilePath);
        Assert.AreEqual(Path.Join(LangsWatcher.OutputPath, "official", "fr", "items", "old.as"), lang.OldDecompiledFilePath);
        Assert.AreEqual(Path.Join(LangsWatcher.OutputPath, "official", "fr", "items", "diff.as"), lang.DiffFilePath);
    }

    [TestMethod]
    public void GetByName_InvalidLangName_ReturnsLang()
    {
        var repository = LangRepository.LoadFromFile(SharedData.LangRepositoryDataPath);
        var lang = repository.GetByName("undefined");

        Assert.IsNull(lang);
    }

    #endregion

    #region GetAllByName

    [TestMethod]
    public void GetAllByName_ValidLangName_ReturnsLangs()
    {
        var repository = LangRepository.LoadFromFile(SharedData.LangRepositoryDataPath);
        var langs = repository.GetAllByName("item");

        Assert.AreEqual(4, langs.Count());
        Assert.IsTrue(langs.All(x => x.Name.Contains("item")));
    }

    [TestMethod]
    public void GetAllByName_InvalidLangName_ReturnsEmpty()
    {
        var repository = LangRepository.LoadFromFile(SharedData.LangRepositoryDataPath);
        var langs = repository.GetAllByName("undefined");

        Assert.AreEqual(0, langs.Count());
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

        LangsWatcher.HttpClient = new HttpClient(mock.Object)
        {
            BaseAddress = new Uri(LangsWatcher.BaseUrl)
        };

        var repository = LangRepository.LoadFromFile(SharedData.LangRepositoryDataPath);
        var version = await repository.FetchVersionsAsync();

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

        LangsWatcher.HttpClient = new HttpClient(mock.Object)
        {
            BaseAddress = new Uri(LangsWatcher.BaseUrl)
        };

        var repository = LangRepository.LoadFromFile(SharedData.LangRepositoryDataPath);
        var version = await repository.FetchVersionsAsync();

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

        LangsWatcher.HttpClient = new HttpClient(mock.Object)
        {
            BaseAddress = new Uri(LangsWatcher.BaseUrl)
        };

        var repository = LangRepository.LoadFromFile(SharedData.LangRepositoryDataPath);
        var version = await repository.FetchVersionsAsync(true);

        Assert.AreEqual(_versions, version);
        Assert.IsTrue(File.Exists(repository.VersionFilePath));
    }

    #endregion

    #region GetUpdatedLangsFromVersions

    [TestMethod]
    public void GetUpdatedLangsFromVersions_ValidVersions_ReturnsUpdatedLangs()
    {
        var repository = LangRepository.LoadFromFile(SharedData.LangRepositoryDataPath);
        var langs = repository.GetUpdatedLangsFromVersions(_versions);

        Assert.AreEqual(38, langs.Count());
    }

    [TestMethod]
    public void GetUpdatedLangsFromVersions_InvalidVersions_ReturnsUpdatedLangs()
    {
        var repository = LangRepository.LoadFromFile(SharedData.LangRepositoryDataPath);
        var langs = repository.GetUpdatedLangsFromVersions(string.Empty);

        Assert.IsFalse(langs.Any());
    }

    #endregion
}
