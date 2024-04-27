using Cyberia.Langzilla.Enums;

using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Commands.Trees;

namespace Cyberia.Salamandra.Commands.Data.Cytrus;

internal class LangLanguageChoiceProvider : IChoiceProvider
{
    public ValueTask<IReadOnlyDictionary<string, object>> ProvideAsync(CommandParameter parameter)
    {
        return new ValueTask<IReadOnlyDictionary<string, object>>(
            Enum.GetNames<LangLanguage>().ToDictionary(x => x, x => (object)x));
    }
}
