using Cyberia.Database.Repositories;
using Cyberia.Langzilla.Primitives;

namespace Cyberia.Langzilla.Extensions;

public static class LangRepositoryExtensions
{
    extension(LangRepository repository)
    {
        /// <summary>
        /// Gets the name of the versions file.
        /// </summary>
        /// <param name="language">The language of the versions file.</param>
        /// <returns>The name of the versions file.</returns>
        public static string GetVersionsFileName(Language language)
        {
            return $"versions_{language.ToStringFast()}.txt";
        }

        /// <summary>
        /// Gets the route to the versions file.
        /// </summary>
        /// <param name="identifier">The identifier of the versions file.</param>
        /// <returns>The route to the versions file.</returns>
        public static string GetVersionsFileRoute(LangsIdentifier identifier)
        {
            return $"{LangsWatcher.GetRoute(identifier.Type)}/{GetVersionsFileName(identifier.Language)}";
        }

        /// <summary>
        /// Gets the output path.
        /// </summary>
        /// <param name="identifier">The identifier of the output path.</param>
        /// <returns>The output path.</returns>
        public static string GetOutputPath(LangsIdentifier identifier)
        {
            return Path.Join(LangsWatcher.OutputPath, identifier.Type.ToStringFast(), identifier.Language.ToStringFast());
        }

        /// <summary>
        /// Gets the path to the versions file.
        /// </summary>
        /// <param name="identifier">The identifier of the versions file.</param>
        /// <returns>The path to the versions file.</returns>
        public static string GetVersionsFilePath(LangsIdentifier identifier)
        {
            return Path.Join(GetOutputPath(identifier), GetVersionsFileName(identifier.Language));
        }
    }
}
