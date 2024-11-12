using Cyberia.Cytrusaurus.Models.FlatBuffers;

namespace Cyberia.Cytrusaurus.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="Fragment"/> struct.
/// </summary>
public static class FragmentExtensions
{
    /// <summary>
    /// Gets the game files from the <see cref="Fragment"/>.
    /// </summary>
    /// <param name="fragment">The <see cref="Fragment"/> from which to get the game files.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> containing all the <see cref="GameFile"/>.</returns>
    internal static IEnumerable<GameFile> GetGameFiles(this Fragment fragment)
    {
        for (var i = 0; i < fragment.FilesLength; i++)
        {
            var file = fragment.Files(i);
            if (file.HasValue)
            {
                yield return file.Value;
            }
        }
    }

    /// <summary>
    /// Compares two fragments and returns a list of differences.
    /// </summary>
    /// <param name="current">The current fragment.</param>
    /// <param name="model">The model fragment to compare with.</param>
    /// <returns>A string representing the differences between the two fragments. Each difference is on a new line.</returns>
    internal static string Diff(this Fragment current, Fragment model)
    {
        List<KeyValuePair<int, string>> diff = new();

        var currentGameFiles = current.GetGameFiles().ToList();
        var modelGameFiles = model.GetGameFiles().ToList();

        var modelGameFilesByIndex = modelGameFiles
            .Index()
            .ToDictionary(x => x.Item.Name, x => x.Index);

        for (var i = 0; i < currentGameFiles.Count; i++)
        {
            var currentGameFile = currentGameFiles[i];

            if (modelGameFilesByIndex.TryGetValue(currentGameFile.Name, out var y))
            {
                var modelGameFile = modelGameFiles[y];

                if (!currentGameFile.GetHashArray().SequenceEqual(modelGameFile.GetHashArray()))
                {
                    var size = modelGameFile.Size == currentGameFile.Size ? string.Empty : $" ({modelGameFile.Size} -> {currentGameFile.Size})";
                    diff.Add(new KeyValuePair<int, string>(y, $"~ {currentGameFile.Name}{size}"));
                }

                modelGameFilesByIndex.Remove(currentGameFile.Name);
                continue;
            }

            diff.Add(new KeyValuePair<int, string>(i, $"+ {currentGameFile.Name}"));
        }

        foreach (var i in modelGameFilesByIndex.Values)
        {
            var modelGameFile = modelGameFiles[i];

            diff.Add(new KeyValuePair<int, string>(i, $"- {modelGameFile.Name}"));
        }

        if (diff.Count > 0)
        {
            diff.Add(new KeyValuePair<int, string>(-1, $@"// {current.Name.ToUpper()} \\"));
        }

        return string.Join('\n', diff.OrderBy(x => x.Key).Select(x => x.Value));
    }
}
