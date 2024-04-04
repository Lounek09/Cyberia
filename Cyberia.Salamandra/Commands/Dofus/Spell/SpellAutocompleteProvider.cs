using Cyberia.Api;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus;

public sealed class SpellAutocompleteProvider : AutocompleteProvider
{
    protected override IEnumerable<DiscordAutoCompleteChoice> InternalProvider(AutocompleteContext ctx, string value)
    {
        return DofusApi.Datacenter.SpellsData.GetSpellsDataByName(value)
            .Take(Constant.MaxChoice)
            .Select(x => new DiscordAutoCompleteChoice($"{x.Name.WithMaxLength(90)} ({x.Id})", x.Id.ToString()));
    }
}
