using Cyberia.Langzilla.Enums;

using System.Reflection;
using System.Text.Json;

namespace Cyberia.Api.Data;

public abstract class DofusLocalizedRepository
{
    /// <summary>
    /// Load a localized repository from a JSON file.
    /// </summary>
    /// <typeparam name="T">The type of the localized repository.</typeparam>
    /// <param name="identifier">The identifier of the langs.</param>
    /// <returns>The loaded localized repository.</returns>
    /// <exception cref="EntryPointNotFoundException">Thrown when the internal constructor of the repository is not found.</exception>
    internal static T Load<T>(LangsIdentifier identifier)
        where T : DofusLocalizedRepository, IDofusRepository
    {
        var constructor = typeof(T).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, Type.EmptyTypes) ??
            throw new EntryPointNotFoundException($"Non public parameter-less constructor for {typeof(T).Name} not found");

        var filePath = Path.Join(DofusApi.OutputPath, identifier.Type.ToStringFast().ToLower(), identifier.Language.ToStringFast(), T.FileName);
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
