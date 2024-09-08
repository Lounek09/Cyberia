using Cyberia.Salamandra.Managers;

using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Commands.Trees;

namespace Cyberia.Salamandra.Commands;

public abstract class ChoiceProvider : IChoiceProvider
{
    protected readonly static IReadOnlyDictionary<string, object> s_empty = new Dictionary<string, object>();

    public ValueTask<IReadOnlyDictionary<string, object>> ProvideAsync(CommandParameter parameter)
    {
        CommandManager.SetCulture();

        return ValueTask.FromResult(InternalProvide(parameter));
    }

    protected abstract IReadOnlyDictionary<string, object> InternalProvide(CommandParameter parameter);
}
