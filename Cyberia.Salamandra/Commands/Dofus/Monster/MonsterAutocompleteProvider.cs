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
        var culture = await _cultureService.GetCultureAsync(ctx.Interaction);

        return DofusApi.Datacenter.MonstersRepository.GetMonstersDataByName(ctx.UserInput ?? string.Empty, culture)
            .Take(Constant.MaxChoice)
            .Select(x =>
            {
                var name = $"{$"{x.Name.ToString(culture)}{(x.BreedSummon ? $" ({Translation.Get<BotTranslations>("Summon", culture)})" : string.Empty)}".WithMaxLength(90)} ({x.Id})";
                return new DiscordAutoCompleteChoice(name, x.Id.ToString());
            });
    }
}
