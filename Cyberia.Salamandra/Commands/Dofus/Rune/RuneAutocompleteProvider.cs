using Cyberia.Api;

using DSharpPlus.Commands.Processors.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus;

public sealed class RuneAutocompleteProvider : AutoCompleteProvider
{
    protected override IReadOnlyDictionary<string, object> InternalAutoComplete(AutoCompleteContext ctx)
    {
        return DofusApi.Datacenter.RunesData.GetRunesDataByName(ctx.UserInput)
            .Take(Constant.MaxChoice)
            .ToDictionary(x => x.Name, x => (object)x.Name);
    }
}
