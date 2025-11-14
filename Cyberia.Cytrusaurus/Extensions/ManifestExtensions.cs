using Cyberia.Cytrusaurus.Models.FlatBuffers;

using System.Text;

namespace Cyberia.Cytrusaurus.Extensions;

public static class ManifestExtensions
{
    extension(Manifest manifest)
    {
        /// <summary>
        /// Gets the fragments from the <see cref="Manifest"/>.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> containing all the <see cref="Fragment"/>.</returns>
        internal IEnumerable<Fragment> GetFragments()
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
        /// Compares two manifests and returns a list of differences.
        /// </summary>
        /// <param name="model">The model manifest to compare with.</param>
        /// <returns>A string representing the differences between the two manifests. Each difference is on a new line.</returns>
        public string Diff(Manifest model)
        {
            List<KeyValuePair<int, string>> diff = new();

            var currentFragments = manifest.GetFragments().ToList();
            var modelFragments = model.GetFragments().ToList();

            var modelFragmentsByIndex = modelFragments
                .Index()
                .ToDictionary(x => x.Item.Name, x => x.Index);

            for (var i = 0; i < currentFragments.Count; i++)
            {
                var currentFragment = currentFragments[i];

                if (modelFragmentsByIndex.TryGetValue(currentFragment.Name, out var y))
                {
                    var modelFragment = modelFragments[y];
                    var fragmentDiff = currentFragment.Diff(modelFragment);

                    if (!string.IsNullOrEmpty(fragmentDiff))
                    {
                        diff.Add(new KeyValuePair<int, string>(y, fragmentDiff + '\n'));
                    }

                    modelFragmentsByIndex.Remove(currentFragment.Name);
                }
                else if (currentFragment.FilesLength > 0)
                {
                    StringBuilder fragmentDiff = new($@"// {currentFragment.Name.ToUpper()} \\");

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
                    StringBuilder fragmentDiff = new($@"// {fragment.Name.ToUpper()} \\");

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
    }
}
