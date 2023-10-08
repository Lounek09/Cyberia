using Cyberia.Api.DatacenterNS;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class SpellAutocompleteProvider : AutocompleteProvider
    {
        public override Task<IEnumerable<DiscordAutoCompleteChoice>> Provider(AutocompleteContext ctx)
        {
            string? value = ctx.OptionValue.ToString();
            if (value is null || value.Length < MIN_LENGTH_AUTOCOMPLETE)
                return Task.FromResult(Enumerable.Empty<DiscordAutoCompleteChoice>());

            List<DiscordAutoCompleteChoice> choices = new();

            foreach (SpellData spellData in Bot.Instance.Api.Datacenter.SpellsData.GetSpellsDataByName(value).Take(MAX_AUTOCOMPLETE_CHOICE))
                choices.Add(new($"{spellData.Name.WithMaxLength(90)} ({spellData.Id})", spellData.Id.ToString()));

            return Task.FromResult(choices.AsEnumerable());
        }
    }
}
