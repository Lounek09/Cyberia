using Cyberia.Api.DatacenterNS;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class CraftAutocompleteProvider : AutocompleteProvider
    {
        public override Task<IEnumerable<DiscordAutoCompleteChoice>> Provider(AutocompleteContext ctx)
        {
            string? value = ctx.OptionValue.ToString();
            if (value is null || value.Length < MIN_LENGTH_AUTOCOMPLETE)
                return Task.FromResult(Enumerable.Empty<DiscordAutoCompleteChoice>());

            List<DiscordAutoCompleteChoice> choices = new();

            foreach (Craft craft in Bot.Instance.Api.Datacenter.CraftsData.GetCraftsByItemName(value).Take(MAX_AUTOCOMPLETE_CHOICE))
            {
                Item? item = craft.GetItem();
                if (item is not null)
                    choices.Add(new($"{item.Name.WithMaxLength(90)} ({craft.Id})", craft.Id.ToString()));
            }

            return Task.FromResult(choices.AsEnumerable());
        }
    }
}
