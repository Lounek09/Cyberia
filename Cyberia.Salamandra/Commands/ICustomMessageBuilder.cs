using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands
{
    public interface ICustomMessageBuilder
    {
        Task<T> GetMessageAsync<T>() where T : IDiscordMessageBuilder, new();
    }
}