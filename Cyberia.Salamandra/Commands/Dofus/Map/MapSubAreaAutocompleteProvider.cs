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

            List<DiscordAutoCompleteChoice> choices = new();
            if (value is not null && value.Length >= MIN_LENGTH_AUTOCOMPLETE)
            {
                foreach (MapSubArea mapSubArea in Bot.Instance.Api.Datacenter.MapsData.GetMapSubAreasByName(value).Take(25))
                    choices.Add(new($"{mapSubArea.Name.WithMaxLength(90)} ({mapSubArea.Id})", mapSubArea.Id.ToString()));
            }

            return Task.FromResult(choices.AsEnumerable());
        }
    }
}
