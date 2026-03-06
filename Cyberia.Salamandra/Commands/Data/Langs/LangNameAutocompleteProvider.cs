using Cyberia.Database.Repositories;
using Cyberia.Langzilla.Primitives;
using Cyberia.Salamandra.Extensions.DSharpPlus;

using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Data.Langs;

public sealed class LangNameAutocompleteProvider : IAutoCompleteProvider
{
    private readonly LangRepository _langRepository;

    public LangNameAutocompleteProvider(LangRepository langRepository)
    {
        _langRepository = langRepository;
    }

    public ValueTask<IEnumerable<DiscordAutoCompleteChoice>> AutoCompleteAsync(AutoCompleteContext ctx)
    {
        var type = ctx.GetArgument<LangType>("type");
        var language = ctx.GetArgument<Language>("language");
        var input = ctx.UserInput ?? string.Empty;

        LangsIdentifier identifier = new(type, language);

        var choices = _langRepository.SearchByIdentifierAndName(identifier, input, Constant.MaxChoice)
            .Select(x => new DiscordAutoCompleteChoice(x.Name, x.Id));

        return ValueTask.FromResult(choices);
    }
}
