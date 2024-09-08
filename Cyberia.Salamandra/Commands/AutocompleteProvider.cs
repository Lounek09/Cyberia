using Cyberia.Salamandra.Managers;

using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;

namespace Cyberia.Salamandra.Commands;

public abstract class AutoCompleteProvider : IAutoCompleteProvider
{
    protected readonly static IReadOnlyDictionary<string, object> s_empty = new Dictionary<string, object>();

    public virtual ValueTask<IReadOnlyDictionary<string, object>> AutoCompleteAsync(AutoCompleteContext ctx)
    {
        CultureManager.SetCulture(ctx.Interaction);

        return ValueTask.FromResult(InternalAutoComplete(ctx));
    }

    protected abstract IReadOnlyDictionary<string, object> InternalAutoComplete(AutoCompleteContext ctx);
}
