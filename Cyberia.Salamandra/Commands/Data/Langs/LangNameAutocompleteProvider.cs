using Cyberia.Langzilla;
using Cyberia.Langzilla.Enums;
using Cyberia.Salamandra.Extensions.DSharpPlus;

using DSharpPlus.Commands.Processors.SlashCommands;

namespace Cyberia.Salamandra.Commands.Data.Langs;

public sealed class LangNameAutocompleteProvider : AutoCompleteProvider
{
    private readonly LangsWatcher _langsWatcher;

    public LangNameAutocompleteProvider(LangsWatcher langsWatcher)
    {
        _langsWatcher = langsWatcher;
    }

    protected override IReadOnlyDictionary<string, object> InternalAutoComplete(AutoCompleteContext ctx)
    {
        var type = ctx.GetArgument<LangType>("type");
        var language = ctx.GetArgument<LangLanguage>("language");

        return _langsWatcher.GetRepository(type, language)
           .GetAllByName(ctx.UserInput)
           .Take(Constant.MaxChoice)
           .ToDictionary(x => x.Name, x => (object)x.Name);
    }
}
