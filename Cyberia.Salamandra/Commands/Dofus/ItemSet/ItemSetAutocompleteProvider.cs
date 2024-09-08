using Cyberia.Api;

using DSharpPlus.Commands.Processors.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus.ItemSet;

public sealed class ItemSetAutocompleteProvider : CultureAutoCompleteProvider
{
    protected override IReadOnlyDictionary<string, object> InternalAutoComplete(AutoCompleteContext ctx)
    {
        return DofusApi.Datacenter.ItemSetsRepository.GetItemSetsDataByName(ctx.UserInput)
            .Take(Constant.MaxChoice)
            .ToDictionary(x => $"{ExtendString.WithMaxLength(x.Name, 90)} ({x.Id})", x => (object)x.Id.ToString());
    }
}
