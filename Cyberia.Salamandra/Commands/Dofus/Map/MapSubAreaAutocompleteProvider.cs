using Cyberia.Api.DatacenterNS;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class MapSubAreaAutocompleteProvider : AutocompleteProvider
    {
        public override Task<IEnumerable<DiscordAutoCompleteChoice>> Provider(AutocompleteContext ctx)
        {
            string? value = ctx.OptionValue.ToString();
            if (value is null || value.Length < MIN_LENGTH_AUTOCOMPLETE)
                return Task.FromResult(Enumerable.Empty<DiscordAutoCompleteChoice>());

            List<DiscordAutoCompleteChoice> choices = new();

            foreach (MapSubArea mapSubArea in Bot.Instance.Api.Datacenter.MapsData.GetMapSubAreasByName(value).Take(MAX_AUTOCOMPLETE_CHOICE))
                choices.Add(new($"{mapSubArea.Name.WithMaxLength(90)} ({mapSubArea.Id})", mapSubArea.Id.ToString()));

            return Task.FromResult(choices.AsEnumerable());
        }
    }
}
