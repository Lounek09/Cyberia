using Cyberia.Api;
using Cyberia.Salamandra.EventHandlers;

using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;

namespace Cyberia.Salamandra.Commands.Dofus.Map;

public sealed class MapSubAreaAutocompleteProvider : IAutoCompleteProvider
{
    private readonly CultureService _cultureService;

    public MapSubAreaAutocompleteProvider(CultureService cultureService)
    {
        _cultureService = cultureService;
    }

    public async ValueTask<IReadOnlyDictionary<string, object>> AutoCompleteAsync(AutoCompleteContext ctx)
    {
        await _cultureService.SetCultureAsync(ctx.Interaction);

        return DofusApi.Datacenter.MapsRepository.GetMapSubAreasDataByName(ctx.UserInput)
            .Take(Constant.MaxChoice)
            .ToDictionary(x => $"{StringExtensions.WithMaxLength(x.Name, 90)} ({x.Id})", x => (object)x.Id.ToString());
    }
}
