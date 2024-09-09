using DSharpPlus.Commands.Processors.SlashCommands.Localization;

namespace Cyberia.Salamandra.Commands;

public abstract class InteractionLocalizer : IInteractionLocalizer
{
    protected const string c_name = ".name";
    protected const string c_description = ".description";
    protected const string c_parameters = ".parameters.";

    public ValueTask<IReadOnlyDictionary<DiscordLocale, string>> TranslateAsync(string fullSymbolName)
    {
        return ValueTask.FromResult(InternalTranslate(fullSymbolName));
    }

    protected abstract IReadOnlyDictionary<DiscordLocale, string> InternalTranslate(string fullSymbolName);
}
