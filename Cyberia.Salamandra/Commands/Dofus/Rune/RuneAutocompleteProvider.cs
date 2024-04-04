using Cyberia.Api;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus;

public sealed class RuneAutocompleteProvider : AutocompleteProvider
{
    protected override IEnumerable<DiscordAutoCompleteChoice> InternalProvider(AutocompleteContext ctx, string value)
    {
        return DofusApi.Datacenter.RunesData.GetRunesDataByName(value)
            .Take(Constant.MaxChoice)
            .Select(x => new DiscordAutoCompleteChoice(x.Name, x.Name));
    }
}
