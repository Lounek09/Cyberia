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

            List<DiscordAutoCompleteChoice> choices = new();
            if (value is not null && value.Length >= MIN_LENGTH_AUTOCOMPLETE)
            {
                foreach (Craft craft in Bot.Instance.Api.Datacenter.CraftsData.GetCraftsByItemName(value).Take(25))
                {
                    Item? item = craft.GetItem();
                    if (item is not null)
                        choices.Add(new($"{item.Name.WithMaxLength(90)} ({craft.Id})", craft.Id.ToString()));
                }
            }

            return Task.FromResult(choices.AsEnumerable());
        }
    }
}
