using Cyberia.Api.DatacenterNS;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class RuneAutocompleteProvider : AutocompleteProvider
    {
        public override Task<IEnumerable<DiscordAutoCompleteChoice>> Provider(AutocompleteContext ctx)
        {
            string? value = ctx.OptionValue.ToString();
            if (value is null || value.Length < MIN_LENGTH_AUTOCOMPLETE)
                return Task.FromResult(Enumerable.Empty<DiscordAutoCompleteChoice>());

            List<DiscordAutoCompleteChoice> choices = new();

            foreach (Rune rune in Bot.Instance.Api.Datacenter.RunesData.GetRunesByName(ctx.OptionValue.ToString()!).Take(MAX_AUTOCOMPLETE_CHOICE))
                choices.Add(new(rune.Name, rune.Name));

            return Task.FromResult(choices.AsEnumerable());
        }
    }
}
