using Cyberia.Api.DatacenterNS;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class HouseAutocompleteProvider : AutocompleteProvider
    {
        public override Task<IEnumerable<DiscordAutoCompleteChoice>> Provider(AutocompleteContext ctx)
        {
            string? value = ctx.OptionValue.ToString();
            if (value is null || value.Length < MIN_LENGTH_AUTOCOMPLETE)
                return Task.FromResult(Enumerable.Empty<DiscordAutoCompleteChoice>());

            List<DiscordAutoCompleteChoice> choices = new();

            foreach (House house in Bot.Instance.Api.Datacenter.HousesData.GetHousesByName(ctx.OptionValue.ToString()!).Take(MAX_AUTOCOMPLETE_CHOICE))
                choices.Add(new($"{house.Name.WithMaxLength(90)} ({house.Id})", house.Id.ToString()));

            return Task.FromResult(choices.AsEnumerable());
        }
    }
}
