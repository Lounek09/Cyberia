using Cyberia.Langzilla;
using Cyberia.Langzilla.Enums;
using Cyberia.Salamandra.DsharpPlus;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Data;

public sealed class LangNameAutocompleteProvider : AutocompleteProvider
{
    protected override IEnumerable<DiscordAutoCompleteChoice> InternalProvider(AutocompleteContext ctx, string value)
    {
        var typeStr = ctx.GetOption<string>("type");
        if (string.IsNullOrEmpty(typeStr))
        {
            return Enumerable.Empty<DiscordAutoCompleteChoice>();
        }

        var languageStr = ctx.GetOption<string>("langue");
        if (string.IsNullOrEmpty(languageStr))
        {
            return Enumerable.Empty<DiscordAutoCompleteChoice>();
        }

        var type = Enum.Parse<LangType>(typeStr);
        var language = Enum.Parse<LangLanguage>(languageStr);

        return LangsWatcher.LangRepositories[(type, language)]
            .GetAllByName(value)
            .Take(Constant.MaxChoice)
            .Select(x => new DiscordAutoCompleteChoice(x.Name, x.Name));
    }
}
