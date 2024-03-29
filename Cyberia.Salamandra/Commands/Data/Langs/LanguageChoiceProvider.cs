using Cyberia.Langzilla.Enums;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Data;

public sealed class LanguageChoiceProvider : IChoiceProvider
{
    public Task<IEnumerable<DiscordApplicationCommandOptionChoice>> Provider()
    {
        return Task.FromResult(
            Enum.GetValues<LangLanguage>()
                .Select(x =>
                {
                    var language = x.ToString();
                    return new DiscordApplicationCommandOptionChoice(language, language);
                }));
    }
}
