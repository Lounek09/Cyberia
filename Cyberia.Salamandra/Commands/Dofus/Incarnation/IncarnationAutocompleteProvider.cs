using Cyberia.Api;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus;

public sealed class IncarnationAutocompleteProvider : AutocompleteProvider
{
    public override Task<IEnumerable<DiscordAutoCompleteChoice>> Provider(AutocompleteContext ctx)
    {
        var value = ctx.OptionValue.ToString();
        if (value is null || value.Length < MIN_LENGTH_AUTOCOMPLETE)
        {
            return Task.FromResult(Enumerable.Empty<DiscordAutoCompleteChoice>());
        }

        List<DiscordAutoCompleteChoice> choices = [];

        foreach (var incarnationData in DofusApi.Datacenter.IncarnationsData.GetIncarnationsDataByName(value).Take(MAX_AUTOCOMPLETE_CHOICE))
        {
            choices.Add(new($"{incarnationData.Name.WithMaxLength(90)} ({incarnationData.Id})", incarnationData.Id.ToString()));
        }

        return Task.FromResult(choices.AsEnumerable());
    }
}
