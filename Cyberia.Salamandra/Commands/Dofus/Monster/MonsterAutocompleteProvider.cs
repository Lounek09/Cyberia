using Cyberia.Api.DatacenterNS;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class MonsterAutocompleteProvider : AutocompleteProvider
    {
        public override Task<IEnumerable<DiscordAutoCompleteChoice>> Provider(AutocompleteContext ctx)
        {
            string? value = ctx.OptionValue.ToString();

            List<DiscordAutoCompleteChoice> choices = new();
            if (value is not null && value.Length >= MIN_LENGTH_AUTOCOMPLETE)
            {
                foreach (Monster monster in Bot.Instance.Api.Datacenter.MonstersData.GetMonstersByName(value).Take(25))
                    choices.Add(new($"{$"{monster.Name} {(monster.BreedSummon ? "(invocation)" : "")}".WithMaxLength(90)} ({monster.Id})", monster.Id.ToString()));
            }

            return Task.FromResult(choices.AsEnumerable());
        }
    }
}
