using Cyberia.Api;
using Cyberia.Salamandra.EventHandlers;

using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;

namespace Cyberia.Salamandra.Commands.Dofus.Monster;

public sealed class MonsterAutocompleteProvider : IAutoCompleteProvider
{
    private readonly CultureService _cultureService;

    public MonsterAutocompleteProvider(CultureService cultureService)
    {
        _cultureService = cultureService;
    }

    public async ValueTask<IReadOnlyDictionary<string, object>> AutoCompleteAsync(AutoCompleteContext ctx)
    {
        await _cultureService.SetCultureAsync(ctx.Interaction);

        return DofusApi.Datacenter.MonstersRepository.GetMonstersDataByName(ctx.UserInput)
            .Take(Constant.MaxChoice)
            .ToDictionary(x => $"{$"{x.Name}{(x.BreedSummon ? $" ({BotTranslations.Summon})" : string.Empty)}".WithMaxLength(90)} ({x.Id})", x => (object)x.Id.ToString());
    }
}
