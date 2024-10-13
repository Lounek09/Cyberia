using Cyberia.Langzilla;
using Cyberia.Langzilla.Enums;
using Cyberia.Langzilla.Models;

namespace Cyberia.Tests.Langzilla.Models;

[TestClass]
public sealed class LangsRepositoryTests
{
    private LangsRepository _langsRepository = default!;

    [TestInitialize]
    public void Setup()
    {
        _langsRepository = LangsRepository.LoadFromFile(SharedData.LangRepositoryDataPath);
    }

    #region LoadFromFile

    [TestMethod]
    public void LoadFromFile_FileExists_LoadsDataCorrectly()
    {
        var repository = LangsRepository.LoadFromFile(SharedData.LangRepositoryDataPath);

        Assert.IsNotNull(repository);
        Assert.AreEqual(LangType.Official, repository.Type);
        Assert.AreEqual(Language.fr, repository.Language);
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
        var lang = _langsRepository.GetByName("items");

        Assert.IsNotNull(lang);
        Assert.AreEqual("items", lang.Name);
        Assert.AreEqual(1184, lang.Version);
        Assert.AreEqual(LangType.Official, lang.Type);
        Assert.AreEqual(Language.fr, lang.Language);
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
        var lang = _langsRepository.GetByName("undefined");

        Assert.IsNull(lang);
    }

    #endregion

    #region GetAllByName

    [TestMethod]
    public void GetAllByName_ValidLangName_ReturnsLangs()
    {
        var langs = _langsRepository.GetAllByName("item");

        Assert.AreEqual(4, langs.Count());
        Assert.IsTrue(langs.All(x => x.Name.Contains("item")));
    }

    [TestMethod]
    public void GetAllByName_InvalidLangName_ReturnsEmpty()
    {
        var langs = _langsRepository.GetAllByName("undefined");

        Assert.AreEqual(0, langs.Count());
    }

    #endregion
}
