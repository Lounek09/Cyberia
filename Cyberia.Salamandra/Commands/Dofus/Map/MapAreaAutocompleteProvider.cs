using Cyberia.Api;

using DSharpPlus.Commands.Processors.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus.Map;

public sealed class MapAreaAutocompleteProvider : AutoCompleteProvider
{
    protected override IReadOnlyDictionary<string, object> InternalAutoComplete(AutoCompleteContext ctx)
    {
        return DofusApi.Datacenter.MapsData.GetMapAreasDataByName(ctx.UserInput)
             .Take(Constant.MaxChoice)
             .ToDictionary(x => $"{x.Name.WithMaxLength(90)} ({x.Id})", x => (object)x.Id.ToString());
    }
}
