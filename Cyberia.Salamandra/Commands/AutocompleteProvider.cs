using Cyberia.Salamandra.Managers;

using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;

namespace Cyberia.Salamandra.Commands;

public abstract class AutoCompleteProvider : IAutoCompleteProvider
{
    protected readonly static IReadOnlyDictionary<string, object> s_empty = new Dictionary<string, object>();

    public ValueTask<IReadOnlyDictionary<string, object>> AutoCompleteAsync(AutoCompleteContext ctx)
    {
        CommandManager.SetCulture();

        return ValueTask.FromResult(InternalAutoComplete(ctx));
    }

    protected abstract IReadOnlyDictionary<string, object> InternalAutoComplete(AutoCompleteContext ctx);
}
