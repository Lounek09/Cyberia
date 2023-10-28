using Cyberia.Api;
using Cyberia.Api.Data;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class ItemSetAutocompleteProvider : AutocompleteProvider
    {
        public override Task<IEnumerable<DiscordAutoCompleteChoice>> Provider(AutocompleteContext ctx)
        {
            string? value = ctx.OptionValue.ToString();
            if (value is null || value.Length < MIN_LENGTH_AUTOCOMPLETE)
            {
                return Task.FromResult(Enumerable.Empty<DiscordAutoCompleteChoice>());
            }

            List<DiscordAutoCompleteChoice> choices = new();

            foreach (ItemSetData itemSetData in DofusApi.Datacenter.ItemSetsData.GetItemSetsDataByName(value).Take(MAX_AUTOCOMPLETE_CHOICE))
            {
                choices.Add(new($"{itemSetData.Name.WithMaxLength(90)} ({itemSetData.Id})", itemSetData.Id.ToString()));
            }

            return Task.FromResult(choices.AsEnumerable());
        }
    }
}
