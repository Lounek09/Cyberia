using Cyberia.Langzilla;
using Cyberia.Langzilla.Enums;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Data;

public sealed class LangNameAutocompleteProvider : AutocompleteProvider
{
    public override Task<IEnumerable<DiscordAutoCompleteChoice>> Provider(AutocompleteContext ctx)
    {
        var value = ctx.OptionValue.ToString();
        if (value is null || value.Length < MIN_LENGTH_AUTOCOMPLETE)
        {
            return Task.FromResult(Enumerable.Empty<DiscordAutoCompleteChoice>());
        }

        var typeStr = CreateFromOption<string>(ctx, "type");
        if (string.IsNullOrEmpty(typeStr))
        {
            return Task.FromResult(Enumerable.Empty<DiscordAutoCompleteChoice>());
        }

        var languageStr = CreateFromOption<string>(ctx, "langue");
        if (string.IsNullOrEmpty(languageStr))
        {
            return Task.FromResult(Enumerable.Empty<DiscordAutoCompleteChoice>());
        }

        List<DiscordAutoCompleteChoice> choices = [];

        var type = Enum.Parse<LangType>(typeStr);
        var language = Enum.Parse<LangLanguage>(languageStr);

        foreach (var langData in LangsWatcher.GetLangsByType(type).GetLangsByLanguage(language).GetLangsByName(value).Take(MAX_AUTOCOMPLETE_CHOICE))
        {
            choices.Add(new(langData.Name, langData.Name));
        }

        return Task.FromResult(choices.AsEnumerable());
    }
}
