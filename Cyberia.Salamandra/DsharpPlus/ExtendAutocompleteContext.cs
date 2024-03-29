using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.DsharpPlus;

public static class ExtendAutocompleteContext
{
    public static T? GetOption<T>(this AutocompleteContext ctx, string name)
    {
        var option = ctx.Options.FirstOrDefault(x => x.Name.Equals(name));

        if (option is not null && option.Value is T value)
        {
            return value;
        }

        return default;
    }
}
