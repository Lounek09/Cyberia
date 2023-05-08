using Cyberia.Api.DatacenterNS;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class IncarnationAutocompleteProvider : AutocompleteProvider
    {
        public override Task<IEnumerable<DiscordAutoCompleteChoice>> Provider(AutocompleteContext ctx)
        {
            string? value = ctx.OptionValue.ToString();

            List<DiscordAutoCompleteChoice> choices = new();
            if (value is not null && value.Length >= MIN_LENGTH_AUTOCOMPLETE)
            {
                foreach (Incarnation incarnation in Bot.Instance.Api.Datacenter.IncarnationsData.GetIncarnationsByName(value).Take(25))
                    choices.Add(new($"{incarnation.Name.WithMaxLength(90)} ({incarnation.Id})", incarnation.Id.ToString()));
            }

            return Task.FromResult(choices.AsEnumerable());
        }
    }
}
