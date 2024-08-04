using Cyberia.Cytrusaurus.Models.FlatBuffers;

using Google.FlatBuffers;

using System.Text;

namespace Cyberia.Cytrusaurus;

/// <summary>
/// Provides methods for retrieving and comparing game manifests of Cytrus.
/// </summary>
public static class CytrusManifest
{
    /// <summary>
    /// Asynchronously gets the game manifest.
    /// </summary>
    /// <param name="game">The name of the game.</param>
    /// <param name="platform">The platform for which the game is released.</param>
    /// <param name="release">The release version of the game.</param>
    /// <param name="version">The version of the game.</param>
    /// <returns>The Manifest object or <see langword="null"/> if the operation fails.</returns>
    public static async Task<Manifest?> GetManifestAsync(string game, string platform, string release, string version)
    {
        var route = GetManifestRoute(game, platform, release, version);

        try
        {
            using var response = await CytrusWatcher.HttpRetryPolicy.ExecuteAsync(() => CytrusWatcher.HttpClient.GetAsync(route));
            response.EnsureSuccessStatusCode();

            var metafile = await response.Content.ReadAsByteArrayAsync();
            ByteBuffer buffer = new(metafile);

            return Manifest.GetRootAsManifest(buffer);
        }
        catch (HttpRequestException e)
        {
            Log.Error(e, "An error occurred while sending Get request to {CytrusManifestUrl}", $"{CytrusWatcher.BaseUrl}/{route}");
        }

        return null;
    }

    /// <summary>
    /// Compares two manifests and returns a list of differences.
    /// </summary>
    /// <param name="current">The current manifest.</param>
    /// <param name="model">The model manifest to compare with.</param>
    /// <returns>A string representing the differences between the two manifests. Each difference is on a new line.</returns>
    public static string Diff(Manifest current, Manifest model)
    {
        List<KeyValuePair<int, string>> diff = new();

        var currentFragments = current.GetFragments().ToList();
        var modelFragments = model.GetFragments().ToList();

        var modelFragmentsByIndex = modelFragments
            .Select((fragment, index) => (fragment.Name, Index: index)) //TODO: .NET9 Use new Index() instead
            .ToDictionary(x => x.Name, x => x.Index);

        for (var i = 0; i < currentFragments.Count; i++)
        {
            var currentFragment = currentFragments[i];

            if (modelFragmentsByIndex.TryGetValue(currentFragment.Name, out var y))
            {
                var modelFragment = modelFragments[y];
                var fragmentDiff = DiffFragment(currentFragment, modelFragment);

                if (!string.IsNullOrEmpty(fragmentDiff))
                {
                    diff.Add(new KeyValuePair<int, string>(y, fragmentDiff + '\n'));
                }
                
                modelFragmentsByIndex.Remove(currentFragment.Name);
            }
            else if (currentFragment.FilesLength > 0)
            {
                StringBuilder fragmentDiff = new($"// {currentFragment.Name.ToUpper()} \\");

                foreach (var gameFile in currentFragment.GetGameFiles())
                {
                    fragmentDiff.Append("+ ");
                    fragmentDiff.Append(gameFile.Name);
                    fragmentDiff.Append('\n');
                }

                diff.Add(new KeyValuePair<int, string>(i, fragmentDiff.ToString()));
            }
        }

        foreach (var i in modelFragmentsByIndex.Values)
        {
            var fragment = modelFragments[i];

            if (fragment.FilesLength > 0)
            {
                StringBuilder fragmentDiff = new($"// {fragment.Name.ToUpper()} \\");

                foreach (var gameFile in fragment.GetGameFiles())
                {
                    fragmentDiff.Append("- ");
                    fragmentDiff.AppendLine(gameFile.Name);
                    fragmentDiff.Append('\n');
                }

                diff.Add(new KeyValuePair<int, string>(i, fragmentDiff.ToString()));
            }
        }

        return string.Join('\n', diff.OrderBy(x => x.Key).Select(x => x.Value));
    }

    /// <summary>
    /// Gets the route of the game manifest.
    /// </summary>
    /// <param name="game">The name of the game.</param>
    /// <param name="platform">The platform for which the game is released.</param>
    /// <param name="release">The release version of the game.</param>
    /// <param name="version">The version of the game.</param>
    /// <returns>The route of the game manifest.</returns>
    internal static string GetManifestRoute(string game, string platform, string release, string version)
    {
        return $"{game}/releases/{release}/{platform}/{version}.manifest";
    }

    /// <summary>
    /// Compares two fragments and returns a list of differences.
    /// </summary>
    /// <param name="current">The current fragment.</param>
    /// <param name="model">The model fragment to compare with.</param>
    /// <returns>A string representing the differences between the two fragments. Each difference is on a new line.</returns>
    internal static string DiffFragment(Fragment current, Fragment model)
    {
        List<KeyValuePair<int, string>> diff = new();

        var currentGameFiles = current.GetGameFiles().ToList();
        var modelGameFiles = model.GetGameFiles().ToList();

        var modelGameFilesByIndex = modelGameFiles
            .Select((file, index) => (file.Name, Index: index)) //TODO: .NET9 Use new Index() instead
            .ToDictionary(x => x.Name, x => x.Index);

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
            diff.Add(new(-1, $@"// {current.Name.ToUpper()} \\"));
        }

        return string.Join('\n', diff.OrderBy(x => x.Key).Select(x => x.Value));
    }

    /// <summary>
    /// Gets the fragments from the manifest.
    /// </summary>
    /// <param name="manifest">The manifest from which to get the fragments.</param>
    /// <returns>An IEnumerable containing all the <see cref="Fragment"/>.</returns>
    internal static IEnumerable<Fragment> GetFragments(this Manifest manifest)
    {
        for (var i = 0; i < manifest.FragmentsLength; i++)
        {
            var fragment = manifest.Fragments(i);
            if (fragment.HasValue)
            {
                yield return fragment.Value;
            }
        }
    }

    /// <summary>
    /// Gets the game files from the fragment.
    /// </summary>
    /// <param name="fragment">The fragment from which to get the game files.</param>
    /// <returns>An IEnumerable containing all the <see cref="GameFile"/>.</returns>
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
}
