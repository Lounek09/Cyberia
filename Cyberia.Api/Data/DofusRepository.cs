using Cyberia.Api.Managers;
using Cyberia.Langzilla.Enums;

using System.Reflection;
using System.Text.Json;

namespace Cyberia.Api.Data;

public abstract class DofusRepository
{
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

        return repository;
    }

    protected virtual void LoadCustomData()
    {

    }

    protected virtual void LoadLocalizedData(LangType type, LangLanguage language)
    {

    }
}
