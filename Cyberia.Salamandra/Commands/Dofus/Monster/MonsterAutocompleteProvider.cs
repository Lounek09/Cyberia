using Cyberia.Api;
using Cyberia.Salamandra.Services;

using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus.Monster;

public sealed class MonsterAutocompleteProvider : IAutoCompleteProvider
{
    private readonly CultureService _cultureService;

    public MonsterAutocompleteProvider(CultureService cultureService)
    {
        _cultureService = cultureService;
    }

    public async ValueTask<IEnumerable<DiscordAutoCompleteChoice>> AutoCompleteAsync(AutoCompleteContext ctx)
    {
        using CultureScope scope = new(await _cultureService.GetCultureAsync(ctx.Interaction));

        return DofusApi.Datacenter.MonstersRepository.GetMonstersDataByName(ctx.UserInput ?? string.Empty)
            .Take(Constant.MaxChoice)
            .Select(x =>
            {
                var name = $"{$"{x.Name}{(x.BreedSummon ? $" ({BotTranslations.Summon})" : string.Empty)}".WithMaxLength(90)} ({x.Id})";
                return new DiscordAutoCompleteChoice(name, x.Id.ToString());
            })
           .ToList();
    }
}
