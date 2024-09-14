using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;

namespace Cyberia.Salamandra.Commands;

public abstract class AutoCompleteProvider : IAutoCompleteProvider
{
    public virtual ValueTask<IReadOnlyDictionary<string, object>> AutoCompleteAsync(AutoCompleteContext ctx)
    {
        return ValueTask.FromResult(InternalAutoComplete(ctx));
    }

    protected abstract IReadOnlyDictionary<string, object> InternalAutoComplete(AutoCompleteContext ctx);
}
