using Cyberia.Api;

using DSharpPlus.Commands.Processors.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus.Rune;

public sealed class RuneAutocompleteProvider : CultureAutoCompleteProvider
{
    protected override IReadOnlyDictionary<string, object> InternalAutoComplete(AutoCompleteContext ctx)
    {
        return DofusApi.Datacenter.RunesRepository.GetRunesDataByName(ctx.UserInput)
            .Take(Constant.MaxChoice)
            .ToDictionary(x => x.Name.ToString(), x => (object)x.Name);
    }
}
