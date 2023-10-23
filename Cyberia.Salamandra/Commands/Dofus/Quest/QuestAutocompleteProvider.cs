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
            if (value is null || value.Length < MIN_LENGTH_AUTOCOMPLETE)
            {
                return Task.FromResult(Enumerable.Empty<DiscordAutoCompleteChoice>());
            }

            List<DiscordAutoCompleteChoice> choices = new();

            foreach (QuestData questData in Bot.Instance.Api.Datacenter.QuestsData.GetQuestsDataByName(value).Take(MAX_AUTOCOMPLETE_CHOICE))
            {
                choices.Add(new($"{questData.Name.WithMaxLength(90)} ({questData.Id})", questData.Id.ToString()));
            }

            return Task.FromResult(choices.AsEnumerable());
        }
    }
}
