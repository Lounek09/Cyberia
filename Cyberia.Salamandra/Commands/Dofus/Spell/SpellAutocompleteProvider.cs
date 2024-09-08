using Cyberia.Api;

using DSharpPlus.Commands.Processors.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus.Spell;

public sealed class SpellAutocompleteProvider : CultureAutoCompleteProvider
{
    protected override IReadOnlyDictionary<string, object> InternalAutoComplete(AutoCompleteContext ctx)
    {
        return DofusApi.Datacenter.SpellsRepository.GetSpellsDataByName(ctx.UserInput)
            .Take(Constant.MaxChoice)
            .ToDictionary(x => $"{ExtendString.WithMaxLength(x.Name, 90)} ({x.Id})", x => (object)x.Id.ToString());
    }
}
