using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands;

public abstract class AutocompleteProvider : IAutocompleteProvider
{
    public const int MIN_LENGTH_AUTOCOMPLETE = 2;
    public const int MAX_AUTOCOMPLETE_CHOICE = 25;

    protected static T? CreateFromOption<T>(AutocompleteContext ctx, string name)
    {
        var option = ctx.Options.FirstOrDefault(x => x.Name.Equals(name));

        if (option is not null && option.Value is T value)
        {
            return value;
        }

        return default;
    }

    public abstract Task<IEnumerable<DiscordAutoCompleteChoice>> Provider(AutocompleteContext ctx);
}
