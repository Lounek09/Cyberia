using Cyberia.Api.DatacenterNS;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class QuestAutocompleteProvider : AutocompleteProvider
    {
        public override Task<IEnumerable<DiscordAutoCompleteChoice>> Provider(AutocompleteContext ctx)
        {
            string? value = ctx.OptionValue.ToString();

            List<DiscordAutoCompleteChoice> choices = new();
            if (value is not null && value.Length >= MIN_LENGTH_AUTOCOMPLETE)
            {
                foreach (Quest quest in Bot.Instance.Api.Datacenter.QuestsData.GetQuestsByName(value).Take(25))
                    choices.Add(new($"{quest.Name.WithMaxLength(90)} ({quest.Id})", quest.Id.ToString()));
            }

            return Task.FromResult(choices.AsEnumerable());
        }
    }
}
