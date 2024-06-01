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

        builder.Services.AddAuthorization();

        Application = builder.Build();

        Application.UseHttpsRedirection();
        Application.UseRouting();
        Application.UseAuthorization();

        Application.MapGet("/", (HttpContext httpContext) =>
        {
            return "Hello World!";
        });

        Application.MapGet("/test", (HttpContext httpContext) =>
        {
            throw new NotImplementedException();
        });
    }

    public static async Task LaunchAsync()
    {
        await Task.Run(() => Application.RunAsync());
    }
}
