using Cyberia.Api.Data;
using Cyberia.Salamandra.Services;

using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus.Quest;

public sealed class QuestAutocompleteProvider : IAutoCompleteProvider
{
    private readonly ICultureService _cultureService;
    private readonly DofusDatacenter _dofusDatacenter;

    public QuestAutocompleteProvider(ICultureService cultureService, DofusDatacenter dofusDatacenter)
    {
        _cultureService = cultureService;
        _dofusDatacenter = dofusDatacenter;
    }

    public ValueTask<IEnumerable<DiscordAutoCompleteChoice>> AutoCompleteAsync(AutoCompleteContext ctx)
    {
        var culture = _cultureService.GetCulture(ctx.Interaction);

        var choices = _dofusDatacenter.QuestsRepository.GetQuestsDataByName(ctx.UserInput ?? string.Empty, culture)
            .Take(Constant.MaxChoice)
            .Select(x =>
            {
                return new DiscordAutoCompleteChoice($"{x.Name.ToString(culture).WithMaxLength(90)} ({x.Id})", x.Id.ToString());
            });

        return ValueTask.FromResult(choices);
    }
}
