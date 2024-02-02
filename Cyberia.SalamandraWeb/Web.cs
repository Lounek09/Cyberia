namespace Cyberia.SalamandraWeb;

public static class Web
{
    public static WebApplication Application { get; private set; } = default!;

    public static void Initialize()
    {
        var builder = WebApplication.CreateBuilder();

        builder.Host.UseSerilog(Log.Logger);
        builder.WebHost.UseUrls("https://localhost:5001", "http://localhost:5000");
        builder.Services.AddRazorPages().AddApplicationPart(typeof(Web).Assembly);

        Application = builder.Build();

#if DEBUG
        Application.UseDeveloperExceptionPage();
#else
        Application.UseExceptionHandler("/Error");
        Application.UseHsts();
#endif

        Application.UseHttpsRedirection();
        Application.UseStaticFiles();
        Application.MapRazorPages();
    }

    public static async Task LaunchAsync()
    {
        await Application.RunAsync();
    }
}
