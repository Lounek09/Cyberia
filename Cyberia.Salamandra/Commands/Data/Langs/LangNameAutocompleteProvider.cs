using Cyberia.Langzilla;
using Cyberia.Langzilla.Enums;
using Cyberia.Salamandra.DsharpPlus;

using DSharpPlus.Commands.Processors.SlashCommands;

namespace Cyberia.Salamandra.Commands.Data.Cytrus;

public sealed class LangNameAutocompleteProvider : AutoCompleteProvider
{
    protected override IReadOnlyDictionary<string, object> InternalAutoComplete(AutoCompleteContext ctx)
    {
        var type = ctx.GetArgument<LangType>("type");
        var language = ctx.GetArgument<LangLanguage>("language");

        return LangsWatcher.LangRepositories[(type, language)]
           .GetAllByName(ctx.UserInput)
           .Take(Constant.MaxChoice)
           .ToDictionary(x => x.Name, x => (object)x.Name);
    }
}
