using Cyberia.Api;

using DSharpPlus.Commands.Processors.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus;

public sealed class CraftAutocompleteProvider : AutoCompleteProvider
{
    protected override IReadOnlyDictionary<string, object> InternalAutoComplete(AutoCompleteContext ctx)
    {
        return DofusApi.Datacenter.CraftsData.GetCraftsDataByItemName(ctx.UserInput)
           .Take(Constant.MaxChoice)
           .ToDictionary(x =>
           {
               var itemName = DofusApi.Datacenter.ItemsData.GetItemNameById(x.Id);
               return $"{itemName.WithMaxLength(90)} ({x.Id})";
           },
           x => (object)x.Id.ToString());
    }
}
