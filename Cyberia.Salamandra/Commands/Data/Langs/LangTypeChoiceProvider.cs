using Cyberia.Langzilla.Enums;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Data;

public sealed class LangTypeChoiceProvider : IChoiceProvider
{
    public Task<IEnumerable<DiscordApplicationCommandOptionChoice>> Provider()
    {
        return Task.FromResult(
            Enum.GetValues<LangType>()
                .Select(x =>
                {
                    var type = x.ToString();
                    return new DiscordApplicationCommandOptionChoice(type, type);
                }));
    }
}
