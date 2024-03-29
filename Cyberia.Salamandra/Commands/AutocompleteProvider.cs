using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands;

public abstract class AutocompleteProvider : IAutocompleteProvider
{
    public Task<IEnumerable<DiscordAutoCompleteChoice>> Provider(AutocompleteContext ctx)
    {
        var value = ctx.OptionValue.ToString();
        if (value is null)
        {
            return Task.FromResult(Enumerable.Empty<DiscordAutoCompleteChoice>());
        }

        return Task.FromResult(InternalProvider(ctx, value));
    }

    protected abstract IEnumerable<DiscordAutoCompleteChoice> InternalProvider(AutocompleteContext ctx, string value);
}
