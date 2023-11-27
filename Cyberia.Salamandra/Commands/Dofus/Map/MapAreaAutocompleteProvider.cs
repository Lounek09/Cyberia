using Cyberia.Api;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus;

public sealed class MapAreaAutocompleteProvider : AutocompleteProvider
{
    public override Task<IEnumerable<DiscordAutoCompleteChoice>> Provider(AutocompleteContext ctx)
    {
        var value = ctx.OptionValue.ToString();
        if (value is null || value.Length < MIN_LENGTH_AUTOCOMPLETE)
        {
            return Task.FromResult(Enumerable.Empty<DiscordAutoCompleteChoice>());
        }

        List<DiscordAutoCompleteChoice> choices = [];

        foreach (var mapAreaData in DofusApi.Datacenter.MapsData.GetMapAreasDataByName(value).Take(MAX_AUTOCOMPLETE_CHOICE))
        {
            choices.Add(new($"{mapAreaData.Name.WithMaxLength(90)} ({mapAreaData.Id})", mapAreaData.Id.ToString()));
        }

        return Task.FromResult(choices.AsEnumerable());
    }
}
