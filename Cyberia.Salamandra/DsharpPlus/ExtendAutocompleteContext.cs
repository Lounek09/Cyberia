using DSharpPlus.Commands.Processors.SlashCommands;

namespace Cyberia.Salamandra.DsharpPlus;

public static class ExtendAutocompleteContext
{
    public static T? GetArgument<T>(this AutoCompleteContext ctx, string name)
    {
        var argument = ctx.Arguments.FirstOrDefault(x => x.Key.Name.Equals(name));
        if (argument.Value is T value)
        {
            return value;
        }

        return default;
    }
}
