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
            {
                return Task.FromResult(Enumerable.Empty<DiscordAutoCompleteChoice>());
            }

            List<DiscordAutoCompleteChoice> choices = new();

            foreach (CraftData craftData in Bot.Instance.Api.Datacenter.CraftsData.GetCraftsDataByItemName(value).Take(MAX_AUTOCOMPLETE_CHOICE))
            {
                ItemData? itemData = craftData.GetItemData();
                if (itemData is not null)
                {
                    choices.Add(new($"{itemData.Name.WithMaxLength(90)} ({craftData.Id})", craftData.Id.ToString()));
                }
            }

            return Task.FromResult(choices.AsEnumerable());
        }
    }
}
