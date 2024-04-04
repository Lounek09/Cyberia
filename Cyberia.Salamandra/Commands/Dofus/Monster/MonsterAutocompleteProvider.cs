using Cyberia.Api;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus;

public sealed class MonsterAutocompleteProvider : AutocompleteProvider
{
    protected override IEnumerable<DiscordAutoCompleteChoice> InternalProvider(AutocompleteContext ctx, string value)
    {
        return DofusApi.Datacenter.MonstersData.GetMonstersDataByName(value)
            .Take(Constant.MaxChoice)
            .Select(x =>
            {
                var name = $"{$"{x.Name}{(x.BreedSummon ? " (invocation)" : string.Empty)}".WithMaxLength(90)} ({x.Id})";
                return new DiscordAutoCompleteChoice(name, x.Id.ToString());
            });
    }
}
