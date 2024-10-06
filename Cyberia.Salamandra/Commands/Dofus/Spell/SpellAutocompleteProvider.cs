using Cyberia.Api;
using Cyberia.Salamandra.Services;

using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus.Spell;

public sealed class SpellAutocompleteProvider : IAutoCompleteProvider
{
    private readonly CultureService _cultureService;

    public SpellAutocompleteProvider(CultureService cultureService)
    {
        _cultureService = cultureService;
    }

    public async ValueTask<IEnumerable<DiscordAutoCompleteChoice>> AutoCompleteAsync(AutoCompleteContext ctx)
    {
        using CultureScope scope = new(await _cultureService.GetCultureAsync(ctx.Interaction)); 

        return DofusApi.Datacenter.SpellsRepository.GetSpellsDataByName(ctx.UserInput ?? string.Empty)
            .Take(Constant.MaxChoice)
            .Select(x =>
            {
                return new DiscordAutoCompleteChoice($"{StringExtensions.WithMaxLength(x.Name, 90)} ({x.Id})", x.Id.ToString());
            })
           .ToList();
    }
}
