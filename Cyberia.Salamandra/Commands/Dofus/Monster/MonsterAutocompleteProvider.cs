using Cyberia.Api;

using DSharpPlus.Commands.Processors.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus;

public sealed class MonsterAutocompleteProvider : AutoCompleteProvider
{
    protected override IReadOnlyDictionary<string, object> InternalAutoComplete(AutoCompleteContext ctx)
    {
        return DofusApi.Datacenter.MonstersData.GetMonstersDataByName(ctx.UserInput)
            .Take(Constant.MaxChoice)
            .ToDictionary(x => $"{$"{x.Name}{(x.BreedSummon ? " (invocation)" : string.Empty)}".WithMaxLength(90)} ({x.Id})", x => (object)x.Id.ToString());
    }
}
