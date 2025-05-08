using Cyberia.Langzilla;
using Cyberia.Langzilla.Enums;
using Cyberia.Salamandra.Extensions.DSharpPlus;

using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Data.Langs;

public sealed class LangNameAutocompleteProvider : IAutoCompleteProvider
{
    private readonly ILangsWatcher _langsWatcher;

    public LangNameAutocompleteProvider(ILangsWatcher langsWatcher)
    {
        _langsWatcher = langsWatcher;
    }

    public ValueTask<IEnumerable<DiscordAutoCompleteChoice>> AutoCompleteAsync(AutoCompleteContext ctx)
    {
        var type = ctx.GetArgument<LangType>("type");
        var language = ctx.GetArgument<Language>("language");

        var choices = _langsWatcher.GetRepository(type, language)
           .GetAllByName(ctx.UserInput ?? string.Empty)
           .Take(Constant.MaxChoice)
           .Select(x => new DiscordAutoCompleteChoice(x.Name, x.Name));

        return ValueTask.FromResult(choices);
    }
}
