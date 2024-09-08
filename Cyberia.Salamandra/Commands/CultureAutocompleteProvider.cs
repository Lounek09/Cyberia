using Cyberia.Salamandra.Managers;

using DSharpPlus.Commands.Processors.SlashCommands;

namespace Cyberia.Salamandra.Commands;

public abstract class CultureAutoCompleteProvider : AutoCompleteProvider
{
    public override ValueTask<IReadOnlyDictionary<string, object>> AutoCompleteAsync(AutoCompleteContext ctx)
    {
        CultureManager.SetCulture(ctx.Interaction);

        return base.AutoCompleteAsync(ctx);
    }
}
