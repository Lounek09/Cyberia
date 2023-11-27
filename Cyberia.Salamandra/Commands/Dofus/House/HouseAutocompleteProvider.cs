using Cyberia.Api;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus;

public sealed class HouseAutocompleteProvider : AutocompleteProvider
{
    public override Task<IEnumerable<DiscordAutoCompleteChoice>> Provider(AutocompleteContext ctx)
    {
        var value = ctx.OptionValue.ToString();
        if (value is null || value.Length < MIN_LENGTH_AUTOCOMPLETE)
        {
            return Task.FromResult(Enumerable.Empty<DiscordAutoCompleteChoice>());
        }

        List<DiscordAutoCompleteChoice> choices = [];

        foreach (var houseData in DofusApi.Datacenter.HousesData.GetHousesDataByName(ctx.OptionValue.ToString()!).Take(MAX_AUTOCOMPLETE_CHOICE))
        {
            choices.Add(new($"{houseData.Name.WithMaxLength(90)} ({houseData.Id})", houseData.Id.ToString()));
        }

        return Task.FromResult(choices.AsEnumerable());
    }
}
