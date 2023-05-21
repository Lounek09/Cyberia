using Cyberia.Api.DatacenterNS;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class ItemAutocompleteProvider : AutocompleteProvider
    {
        public override Task<IEnumerable<DiscordAutoCompleteChoice>> Provider(AutocompleteContext ctx)
        {
            string? value = ctx.OptionValue.ToString();
            if (value is null || value.Length < MIN_LENGTH_AUTOCOMPLETE)
                return Task.FromResult(Enumerable.Empty<DiscordAutoCompleteChoice>());

            List<DiscordAutoCompleteChoice> choices = new();

            foreach (Item item in Bot.Instance.Api.Datacenter.ItemsData.GetItemsByName(value).Take(MAX_AUTOCOMPLETE_CHOICE))
                choices.Add(new($"{item.Name.WithMaxLength(90)} ({item.Id})", item.Id.ToString()));

            return Task.FromResult(choices.AsEnumerable());
        }
    }
}
