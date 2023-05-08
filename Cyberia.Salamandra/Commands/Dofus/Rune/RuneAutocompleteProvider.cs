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

            List<DiscordAutoCompleteChoice> choices = new();
            if (value is not null)
            {
                foreach (Rune rune in Bot.Instance.Api.Datacenter.RunesData.GetRunesByName(ctx.OptionValue.ToString()!).Take(25))
                    choices.Add(new(rune.Name, rune.Name));
            }

            return Task.FromResult(choices.AsEnumerable());
        }
    }
}
