using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands
{
    public abstract class AutocompleteProvider : IAutocompleteProvider
    {
        public const int MIN_LENGTH_AUTOCOMPLETE = 2;

        protected static T? GetValueFromOption<T>(AutocompleteContext ctx, string name)
        {
            DiscordInteractionDataOption? gameOption = ctx.Options.FirstOrDefault(x => x.Name.Equals(name));

            if (gameOption is not null && gameOption.Value is T value)
                return value;

            return default;
        }

        public abstract Task<IEnumerable<DiscordAutoCompleteChoice>> Provider(AutocompleteContext ctx);
    }
}
