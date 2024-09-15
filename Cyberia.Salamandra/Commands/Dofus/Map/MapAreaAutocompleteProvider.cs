using Cyberia.Api;
using Cyberia.Salamandra.Services;

using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;

namespace Cyberia.Salamandra.Commands.Dofus.Map;

public sealed class MapAreaAutocompleteProvider : IAutoCompleteProvider
{
    private readonly CultureService _cultureService;

    public MapAreaAutocompleteProvider(CultureService cultureService)
    {
        _cultureService = cultureService;
    }

    public async ValueTask<IReadOnlyDictionary<string, object>> AutoCompleteAsync(AutoCompleteContext ctx)
    {
        using CultureScope scope = new(await _cultureService.GetCultureAsync(ctx.Interaction));

        return DofusApi.Datacenter.MapsRepository.GetMapAreasDataByName(ctx.UserInput)
             .Take(Constant.MaxChoice)
             .ToDictionary(x => $"{StringExtensions.WithMaxLength(x.Name, 90)} ({x.Id})", x => (object)x.Id.ToString());
    }
}
