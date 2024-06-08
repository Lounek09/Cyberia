using Cyberia.Amphibian.Conventions;
using Cyberia.Amphibian.Middlewares;

using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;

namespace Cyberia.Amphibian;

public static class Web
{
    public static WebConfig Config { get; private set; } = default!;
    public static WebApplication Application { get; private set; } = default!;

    public static void Initialize(WebConfig config)
    {
        Config = config;

        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", Config.Environment);

        var builder = WebApplication.CreateBuilder(new WebApplicationOptions()
        {
            ApplicationName = "Cyberia.Amphibian",
            EnvironmentName = Config.Environment,
            ContentRootPath = AppContext.BaseDirectory,
            WebRootPath = "wwwroot"           
        });

        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog(Log.Logger);

        builder.WebHost.UseUrls(Config.Urls.ToArray());

        builder.Services.Configure<RequestLocalizationOptions>(options =>
        {
            string[] supportedCultures = ["en", "fr", "es", "de", "it", "nl", "pt"];

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


        Application = builder.Build();

        Application.UseHttpsRedirection();
        Application.UseStaticFiles();
        Application.UseRouting();
        Application.UseAuthorization();
        Application.UseRequestLocalization();
        Application.UseMiddleware<CultureRedirectMiddleware>();

        Application.MapRazorPages();
    }

    public static async Task LaunchAsync()
    {
        await Task.Run(() => Application.RunAsync());
    }
}
