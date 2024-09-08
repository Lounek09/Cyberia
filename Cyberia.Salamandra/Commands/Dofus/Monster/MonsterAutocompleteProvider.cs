using Cyberia.Api;

using DSharpPlus.Commands.Processors.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus.Monster;

public sealed class MonsterAutocompleteProvider : CultureAutoCompleteProvider
{
    protected override IReadOnlyDictionary<string, object> InternalAutoComplete(AutoCompleteContext ctx)
    {
        return DofusApi.Datacenter.MonstersRepository.GetMonstersDataByName(ctx.UserInput)
            .Take(Constant.MaxChoice)
            .ToDictionary(x => $"{$"{x.Name}{(x.BreedSummon ? $" ({BotTranslations.Summon})" : string.Empty)}".WithMaxLength(90)} ({x.Id})", x => (object)x.Id.ToString());
    }
}
