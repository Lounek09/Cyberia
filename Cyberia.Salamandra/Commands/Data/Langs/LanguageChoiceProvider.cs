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
            foreach (Language language in Enum.GetValues<Language>())
            {
                choices.Add(new(language.ToString(), language.ToString()));
            }

            return Task.FromResult(choices.AsEnumerable());
        }
    }
}
