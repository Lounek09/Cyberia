using Cyberia.Langzilla.Enums;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Data;

public sealed class LangTypeChoiceProvider : IChoiceProvider
{
    public Task<IEnumerable<DiscordApplicationCommandOptionChoice>> Provider()
    {
        List<DiscordApplicationCommandOptionChoice> choices = [];

        foreach (var type in Enum.GetValues<LangType>())
        {
            choices.Add(new(type.ToString(), type.ToString()));
        }

        return Task.FromResult(choices.AsEnumerable());
    }
}
