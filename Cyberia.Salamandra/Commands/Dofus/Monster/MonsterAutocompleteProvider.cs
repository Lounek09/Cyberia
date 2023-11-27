using Cyberia.Api;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus;

public sealed class MonsterAutocompleteProvider : AutocompleteProvider
{
    public override Task<IEnumerable<DiscordAutoCompleteChoice>> Provider(AutocompleteContext ctx)
    {
        var value = ctx.OptionValue.ToString();
        if (value is null || value.Length < MIN_LENGTH_AUTOCOMPLETE)
        {
            return Task.FromResult(Enumerable.Empty<DiscordAutoCompleteChoice>());
        }

        List<DiscordAutoCompleteChoice> choices = [];

        foreach (var monsterData in DofusApi.Datacenter.MonstersData.GetMonstersDataByName(value).Take(MAX_AUTOCOMPLETE_CHOICE))
        {
            choices.Add(new($"{$"{monsterData.Name} {(monsterData.BreedSummon ? "(invocation)" : "")}".WithMaxLength(90)} ({monsterData.Id})", monsterData.Id.ToString()));
        }

        return Task.FromResult(choices.AsEnumerable());
    }
}
