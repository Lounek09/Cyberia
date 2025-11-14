using Cyberia.Amphibian.Conventions;
using Cyberia.Amphibian.Middlewares;
using Cyberia.Langzilla.Enums;

using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;

namespace Cyberia.Amphibian.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="IServiceCollection"/> interface.
/// </summary>
public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        /// <summary>
        /// Adds the Amphibian dependencies to the service collection.
        /// </summary>
        /// <param name="config">The web configuration.</param>
        /// <param name="supportedLanguages">The supported languages.</param>
        /// <returns>The updated service collection.</returns>
        public IServiceCollection AddAmphibian(WebConfig config, IReadOnlyList<Language> supportedLanguages)
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", config.Environment);

            services.AddSingleton(config);

            services.AddSingleton(_ =>
            {
                var builder = WebApplication.CreateBuilder(new WebApplicationOptions()
                {
                    ApplicationName = "Cyberia.Amphibian",
                    EnvironmentName = config.Environment,
                    ContentRootPath = AppContext.BaseDirectory,
                    WebRootPath = "wwwroot"
                });

                foreach (var serviceDescriptor in services)
                {
                    builder.Services.Add(serviceDescriptor);
                }

                builder.Logging.ClearProviders().AddSerilog();

                builder.WebHost.UseUrls(config.Urls.ToArray());

                builder.Services.Configure<RequestLocalizationOptions>(options =>
                {
                    var supportedCultures = supportedLanguages.Select(lang => lang.ToStringFast()).ToArray();

                    options.SetDefaultCulture(supportedCultures[0]);
                    options.AddSupportedCultures(supportedCultures);
                    options.AddSupportedUICultures(supportedCultures);
                    options.RequestCultureProviders =
                    [
                        new RouteDataRequestCultureProvider(),
                    new CookieRequestCultureProvider(),
                    new AcceptLanguageHeaderRequestCultureProvider()
                    ];
                });

                builder.Services.AddRouting(options => options.LowercaseUrls = true);
                builder.Services.AddAuthorization();
                builder.Services.AddRazorPages(options => options.Conventions.Add(new CulturePageRouteModelConvention()));

                var application = builder.Build();

                application.UseHttpsRedirectionIfNeeded();
                application.UseStaticFiles();
                application.UseRouting();
                application.UseAuthorization();
                application.UseRequestLocalization();
                application.UseMiddleware<CultureRedirectMiddleware>();

                application.MapDefaultControllerRoute();
                application.MapRazorPages();

                return application;
            });

            return services;
        }
    }
}
