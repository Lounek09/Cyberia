using Cyberia.Api;
using Cyberia.Salamandra.Services;

using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;

namespace Cyberia.Salamandra.Commands.Dofus.Rune;

public sealed class RuneAutocompleteProvider : IAutoCompleteProvider
{
    private readonly CultureService _cultureService;

    public RuneAutocompleteProvider(CultureService cultureService)
    {
        _cultureService = cultureService;
    }

    public async ValueTask<IReadOnlyDictionary<string, object>> AutoCompleteAsync(AutoCompleteContext ctx)
    {
        await _cultureService.SetCultureAsync(ctx.Interaction);

        return DofusApi.Datacenter.RunesRepository.GetRunesDataByName(ctx.UserInput)
            .Take(Constant.MaxChoice)
            .ToDictionary(x => x.Name.ToString(), x => (object)x.Name);
    }
}
