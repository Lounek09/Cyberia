using Cyberia.Api.Managers;
using Cyberia.Langzilla.Enums;

using System.Reflection;
using System.Text.Json;

namespace Cyberia.Api.Data;

public abstract class DofusRepository
{
    /// <summary>
    /// Load a repository from a JSON file.
    /// </summary>
    /// <typeparam name="T">The type of the repository.</typeparam>
    /// <param name="type">The type of the lang to load.</param>
    /// <returns>The loaded repository.</returns>
    /// <exception cref="EntryPointNotFoundException">Thrown when the internal constructor of the repository is not found.</exception>
    internal static T Load<T>(LangType type)
        where T : DofusRepository, IDofusRepository
    {
        var constructor = typeof(T).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, Type.EmptyTypes) ??
            throw new EntryPointNotFoundException($"Non public parameter-less constructor for {typeof(T).Name} not found");

        var filePath = Path.Join(LangParserManager.GetOutputDirectoryPath(type, DofusApi.Config.BaseLanguage), T.FileName);
        if (!File.Exists(filePath))
        {
            Log.Warning("File {FilePath} not found to initialize {TypeName}", filePath, typeof(T).Name);
            return (T)constructor.Invoke(null);
        }

        using var json = File.OpenRead(filePath);

        T? repository = null;
        try
        {
            repository = JsonSerializer.Deserialize<T>(json);
        }
        catch (Exception e)
        {
            Log.Error(e, "Failed to deserialize the JSON located at {FilePath} to initialize {TypeName}", filePath, typeof(T).Name);
        }

        if (repository is null)
        {
            return (T)constructor.Invoke(null);
        }

        repository.LoadCustomData();

        foreach (var language in Enum.GetValues<LangLanguage>())
        {
            if (language == DofusApi.Config.BaseLanguage)
            {
                continue;
            }

            repository.LoadLocalizedData(type, language);
        }

        repository.FinalizeLoading();

        return repository;
    }

    /// <summary>
    /// Load custom data from manually generated JSON files.
    /// </summary>
    protected virtual void LoadCustomData()
    {

    }

    /// <summary>
    /// Load localized data from another language.
    /// </summary>
    /// <param name="type">The type of the lang to load.</param>
    /// <param name="language">The language of the lang to load.</param>
    protected virtual void LoadLocalizedData(LangType type, LangLanguage language)
    {

    }

    /// <summary>
    /// Finalize the loading process of the repository.
    /// </summary>
    protected virtual void FinalizeLoading()
    {

    }
}
