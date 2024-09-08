using Cyberia.Api.Managers;
using Cyberia.Langzilla.Enums;

using System.Reflection;
using System.Text.Json;

namespace Cyberia.Api.Data;

public abstract class DofusLocalizedRepository
{
    internal static T Load<T>(LangType type, LangLanguage language)
        where T : DofusLocalizedRepository, IDofusRepository
    {
        var constructor = typeof(T).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, Type.EmptyTypes) ??
            throw new EntryPointNotFoundException($"Non public parameter-less constructor for {typeof(T).Name} not found");

        var filePath = Path.Join(LangParserManager.GetOutputDirectoryPath(type, language), T.FileName);
        if (!File.Exists(filePath))
        {
            Log.Warning("File {FilePath} not found to initialize {TypeName}", filePath, typeof(T).Name);
            return (T)constructor.Invoke(null);
        }

        using var json = File.OpenRead(filePath);

        try
        {
            return JsonSerializer.Deserialize<T>(json) ?? (T)constructor.Invoke(null);
        }
        catch (Exception e)
        {
            Log.Error(e, "Failed to deserialize the JSON located at {FilePath} to initialize {TypeName}", filePath, typeof(T).Name);
        }

        return (T)constructor.Invoke(null);
    }
}
