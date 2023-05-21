using Cyberia.Api.DatacenterNS;

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
                return Task.FromResult(Enumerable.Empty<DiscordAutoCompleteChoice>());

            List<DiscordAutoCompleteChoice> choices = new();

            foreach (ItemSet itemSet in Bot.Instance.Api.Datacenter.ItemSetsData.GetItemSetsByName(value).Take(MAX_AUTOCOMPLETE_CHOICE))
                choices.Add(new($"{itemSet.Name.WithMaxLength(90)} ({itemSet.Id})", itemSet.Id.ToString()));

            return Task.FromResult(choices.AsEnumerable());
        }
    }
}
