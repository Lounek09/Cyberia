using Cyberia.Api;

using DSharpPlus.Commands.Processors.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus.Item;

public sealed class ItemAutocompleteProvider : AutoCompleteProvider
{
    protected override IReadOnlyDictionary<string, object> InternalAutoComplete(AutoCompleteContext ctx)
    {
        return DofusApi.Datacenter.ItemsData.GetItemsDataByName(ctx.UserInput)
            .Take(Constant.MaxChoice)
            .ToDictionary(x => $"{x.Name.WithMaxLength(90)} ({x.Id})", x => (object)x.Id.ToString());
    }
}
