using Cyberia.Api.Data;
using Cyberia.Api.Extensions;

using Microsoft.Extensions.DependencyInjection;

namespace Cyberia.Api;

/// <summary>
/// Represents the Dofus API that provides access to the Dofus datacenter.
/// </summary>
public static class DofusApi
{
    public const string OutputPath = "api";

    public static readonly string CustomPath = Path.Join(OutputPath, "custom");

    internal static DofusDatacenter Datacenter { get; set; } = default!;
    internal static DofusApiConfig Config { get; set; } = default!;
    internal static HttpClient HttpClient { get; set; } = default!;

    /// <summary>
    /// Initializes the Dofus API with the specified configuration.
    /// </summary>
    /// <param name="config">The configuration for the API.</param>
    /// <returns>The initialized DofusDatacenter instance.</returns>
    /// <remarks>
    /// If you use dependency injection, use <see cref="ServiceCollectionExtensions.AddDofusApi(IServiceCollection, DofusApiConfig)"/> instead.
    /// </remarks>
    public static DofusDatacenter Initialize(DofusApiConfig config)
    {
        Config = config;
        Datacenter = new();
        HttpClient = new();

        Datacenter.Load(config.Type);

        return Datacenter;
    }
}
