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

            foreach (RuneData runeData in Bot.Instance.Api.Datacenter.RunesData.GetRunesDataByName(ctx.OptionValue.ToString()!).Take(MAX_AUTOCOMPLETE_CHOICE))
                choices.Add(new(runeData.Name, runeData.Name));

            return Task.FromResult(choices.AsEnumerable());
        }
    }
}
