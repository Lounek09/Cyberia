using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Salamandra.Bot.Commands.Data
{
    public sealed class CytrusPlatformAutocompleteProvider : AutocompleteProvider
    {
        public override Task<IEnumerable<DiscordAutoCompleteChoice>> Provider(AutocompleteContext ctx)
        {
            HashSet<DiscordAutoCompleteChoice> choices = new();

            string? value = ctx.OptionValue.ToString();
            if (value is not null)
            {
                string? game = GetValueFromOption(ctx, "game");

                if (!string.IsNullOrEmpty(game))
                {
                    foreach (KeyValuePair<string, Dictionary<string, string>> platform in DiscordBot.Instance.Cytrus.CytrusData.Games[game].Platforms)
                        choices.Add(new(platform.Key.Capitalize(), platform.Key));
                }
            }

            return Task.FromResult(choices.AsEnumerable());
        }
    }
}
