using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Commands.Trees;

namespace Cyberia.Salamandra.Commands;

public abstract class ChoiceProvider : IChoiceProvider
{
    public ValueTask<IReadOnlyDictionary<string, object>> ProvideAsync(CommandParameter parameter)
    {
        return ValueTask.FromResult(InternalProvide(parameter));
    }

    protected abstract IReadOnlyDictionary<string, object> InternalProvide(CommandParameter parameter);
}
