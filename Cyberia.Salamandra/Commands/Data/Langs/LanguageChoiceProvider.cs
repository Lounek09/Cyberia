using Cyberia.Langzilla.Enums;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Data
{
    public sealed class LanguageChoiceProvider : IChoiceProvider
    {
        public Task<IEnumerable<DiscordApplicationCommandOptionChoice>> Provider()
        {
            List<DiscordApplicationCommandOptionChoice> choices = new();
            foreach (LangLanguage language in Enum.GetValues<LangLanguage>())
            {
                choices.Add(new(language.ToString(), language.ToString()));
            }

            return Task.FromResult(choices.AsEnumerable());
        }
    }
}
