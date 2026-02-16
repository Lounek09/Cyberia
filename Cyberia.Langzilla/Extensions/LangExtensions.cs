using Cyberia.Database.Models;
using Cyberia.Database.Repositories;
using Cyberia.Langzilla.Primitives;

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
        public string FileName => $"{lang.Name}_{lang.Language.ToStringFast()}_{lang.Version}.swf";

        /// <summary>
        /// Gets the route of the swf file.
        /// </summary>
        public string Route => $"{LangsWatcher.GetRoute(lang.Type)}/swf/{lang.FileName}";

        /// <summary>
        /// Gets the directory output path of the lang.
        /// </summary>
        public string OutputPath => Path.Join(LangRepository.GetOutputPath(new LangsIdentifier(lang.Type, lang.Language)), lang.Name);

        /// <summary>
        /// Gets the file path of the lang.
        /// </summary>
        public string FilePath => Path.Join(lang.OutputPath, lang.FileName);

        /// <summary>
        /// Gets the generated decompiled file path of the lang.
        /// </summary>
        public string DecompiledFilePath => Path.Join(lang.OutputPath, "current.as");

        /// <summary>
        /// Gets the generated diff file path of the lang.
        /// </summary>
        public string DiffFilePath => Path.Join(lang.OutputPath, "diff.as");
    }
}
