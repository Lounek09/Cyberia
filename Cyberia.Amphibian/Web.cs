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

        builder.Services.AddRouting(options => options.LowercaseUrls = true);
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();
        builder.Services.AddAuthorization();

        Application = builder.Build();

        Application.UseHttpsRedirection();
        Application.UseStaticFiles();
        Application.UseRouting();
        Application.UseAuthorization();

        Application.MapRazorPages();
    }

    public static async Task LaunchAsync()
    {
        await Task.Run(() => Application.RunAsync());
    }
}
