using Cyberia.Cytrusaurus;
using Cyberia.Langzilla;
using Cyberia.Salamandra.Managers;
using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.Entities;

using Microsoft.Extensions.DependencyInjection;

namespace Cyberia.Salamandra.Extensions;

public static class ServiceProviderExtensions
{
    public static IServiceProvider RegisterSalamandraEvents(this IServiceProvider provider)
    {
        CytrusWatcher.NewCytrusFileDetected += provider.GetRequiredService<CytrusService>().OnNewCytrusDetected;
        LangsWatcher.CheckLangFinished += LangManager.OnCheckLangFinished;

        return provider;
    }

    public static async Task<IServiceProvider> StartSalamandraAsync(this IServiceProvider provider)
    {
        DiscordActivity activity = new("Dofus Retro", DiscordActivityType.Playing);
        await provider.GetRequiredService<DiscordClient>().ConnectAsync(activity);

        return provider;
    }
}
