using Cyberia.Api;
using Cyberia.Salamandra.Services;

using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus.Incarnation;

public sealed class IncarnationAutocompleteProvider : IAutoCompleteProvider
{
    private readonly CultureService _cultureService;

    public IncarnationAutocompleteProvider(CultureService cultureService)
    {
        _cultureService = cultureService;
    }

    public async ValueTask<IEnumerable<DiscordAutoCompleteChoice>> AutoCompleteAsync(AutoCompleteContext ctx)
    {
        using CultureScope scope = new(await _cultureService.GetCultureAsync(ctx.Interaction));

        return DofusApi.Datacenter.IncarnationsRepository.GetIncarnationsDataByItemName(ctx.UserInput ?? string.Empty)
            .Take(Constant.MaxChoice)
            .Select(x =>
            {
                var itemName = DofusApi.Datacenter.ItemsRepository.GetItemNameById(x.Id);
                return new DiscordAutoCompleteChoice($"{itemName.WithMaxLength(90)} ({x.Id})", x.Id.ToString());
            })
           .ToList();
    }
}
