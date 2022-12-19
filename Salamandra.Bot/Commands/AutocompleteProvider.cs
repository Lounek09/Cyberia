using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Salamandra.Bot.Commands
{
    public abstract class AutocompleteProvider : IAutocompleteProvider
    {
        protected static string? GetValueFromOption(AutocompleteContext ctx, string name)
        {
            DiscordInteractionDataOption? gameOption = ctx.Options.FirstOrDefault(x => x.Name.Equals(name));

            if (gameOption is not null)
                return gameOption.Value.ToString();

            return null;
        }

        public abstract Task<IEnumerable<DiscordAutoCompleteChoice>> Provider(AutocompleteContext ctx);
    }
}
