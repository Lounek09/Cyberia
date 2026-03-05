using Cyberia.Database.Models;
using Cyberia.Database.Repositories;
using Cyberia.Langzilla.Primitives;

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Cyberia.Langzilla.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="Lang"/> class.
/// </summary>
public static class LangExtensions
{
    extension(Lang lang)
    {
        /// <summary>
        /// Gets the name of the swf file.
        /// </summary>
        public string GetFileName()
        {
            return $"{lang.Name}_{lang.Language.ToStringFast()}_{lang.Version}.swf";
        }

        /// <summary>
        /// Gets the route of the swf file.
        /// </summary>
        public string GetRoute()
        {
            return $"{LangsWatcher.GetRoute(lang.Type)}/swf/{lang.GetFileName()}";
        }

        /// <summary>
        /// Gets the directory output path of the lang.
        /// </summary>
        public string GetOutputPath()
        {
            return Path.Join(LangRepository.GetOutputPath(new LangsIdentifier(lang.Type, lang.Language)), lang.Name);
        }

        /// <summary>
        /// Gets the file path of the lang.
        /// </summary>
        public string GetFilePath()
        {
            return Path.Join(lang.GetOutputPath(), lang.GetFileName());
        }

        /// <summary>
        /// Gets the generated decompiled file path of the lang.
        /// </summary>
        public string GetDecompiledFilePath()
        {
            return Path.Join(lang.GetOutputPath(), "current.as");
        }

        /// <summary>
        /// Gets the old generated decompiled file path of the lang.
        /// </summary>
        public string GetOldDecompiledFilePath()
        {
            return Path.Join(lang.GetOutputPath(), "old.as");
        }

        /// <summary>
        /// Gets the generated diff file path of the lang.
        /// </summary>
        public string GetDiffFilePath()
        {
            return Path.Join(lang.GetOutputPath(), "diff.as");
        }

        /// <summary>
        /// Computes the difference between two Langs.
        /// </summary>
        /// <param name="current">The current Lang.</param>
        /// <param name="model">The model Lang.</param>
        /// <returns>A string representing the difference between the current and model Langs.</returns>
        public static string Diff(Lang current, Lang model)
        {
            var currentDecompiledFilePath = current.GetDecompiledFilePath();
            var modelDecompiledFilePath = current.Equals(model)
                ? current.GetOldDecompiledFilePath()
                : model.GetDecompiledFilePath();

            var currentFileExists = File.Exists(currentDecompiledFilePath);
            var modelFileExists = File.Exists(modelDecompiledFilePath);

            if (!currentFileExists && !modelFileExists)
            {
                return string.Empty;
            }

            if (currentFileExists && !modelFileExists)
            {
                return PrefixLines(currentDecompiledFilePath, '+');
            }

            if (!currentFileExists && modelFileExists)
            {
                return PrefixLines(modelDecompiledFilePath, '-');
            }

            var currentLines = File.ReadAllLines(currentDecompiledFilePath);
            var modelLines = File.ReadAllLines(modelDecompiledFilePath);

            Dictionary<string, int> modelLineCounts = new(modelLines.Length);
            foreach (var line in modelLines)
            {
                ref var count = ref CollectionsMarshal.GetValueRefOrAddDefault(modelLineCounts, line, out _);
                count++;
            }

            var diffLines = new List<(char Symbol, string Line)>(Math.Min(64, currentLines.Length));

            foreach (var line in currentLines)
            {
                ref var count = ref CollectionsMarshal.GetValueRefOrNullRef(modelLineCounts, line);

                if (!Unsafe.IsNullRef(ref count) && count > 0)
                {
                    count--;
                    continue;
                }

                diffLines.Add(('+', line));
            }

            foreach (var (line, count) in modelLineCounts)
            {
                for (var i = 0; i < count; i++)
                {
                    diffLines.Add(('-', line));
                }
            }

            diffLines.Sort(static (a, b) => StringComparer.InvariantNumeric.Compare(a.Line, b.Line));

            var capacity = diffLines.Sum(x => x.Line.Length + 3);
            StringBuilder builder = new(capacity);

            foreach (var (symbol, line) in diffLines)
            {
                builder.Append(symbol).Append(' ').Append(line).Append('\n');
            }

            return builder.ToString();

            static string PrefixLines(string filePath, char prefix)
            {
                var lines = File.ReadAllLines(filePath);
                var capacity = lines.Sum(x => x.Length + 3);
                StringBuilder builder = new(capacity);

                foreach (var line in lines)
                {
                    builder.Append(prefix).Append(' ').Append(line).Append('\n');
                }

                return builder.ToString();
            }
        }
    }
}
