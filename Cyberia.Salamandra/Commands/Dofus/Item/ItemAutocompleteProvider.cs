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

            List<DiscordAutoCompleteChoice> choices = new();
            if (value is not null && value.Length >= MIN_LENGTH_AUTOCOMPLETE)
            {
                foreach (Item item in Bot.Instance.Api.Datacenter.ItemsData.GetItemsByName(value).Take(25))
                    choices.Add(new($"{item.Name.WithMaxLength(90)} ({item.Id})", item.Id.ToString()));
            }

            return Task.FromResult(choices.AsEnumerable());
        }
    }
}
