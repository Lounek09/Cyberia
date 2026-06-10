using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands;

public interface ICustomMessageBuilder
{
    Task<T> BuildAsync<T>() where T : IDiscordMessageBuilder, new();
}
