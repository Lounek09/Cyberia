using Cyberia.Api;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus;

public sealed class RuneAutocompleteProvider : AutocompleteProvider
{
    public override Task<IEnumerable<DiscordAutoCompleteChoice>> Provider(AutocompleteContext ctx)
    {
        var value = ctx.OptionValue.ToString();
        if (value is null)
        {
            return Task.FromResult(Enumerable.Empty<DiscordAutoCompleteChoice>());
        }

        List<DiscordAutoCompleteChoice> choices = [];

        foreach (var runeData in DofusApi.Datacenter.RunesData.GetRunesDataByName(value).Take(MAX_AUTOCOMPLETE_CHOICE))
        {
            choices.Add(new(runeData.Name, runeData.Name));
        }

        return Task.FromResult(choices.AsEnumerable());
    }
}
