using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Salamandra.Bot.Commands.Data
{
    public sealed class CytrusReleaseAutocompleteProvider : AutocompleteProvider
    {
        public override Task<IEnumerable<DiscordAutoCompleteChoice>> Provider(AutocompleteContext ctx)
        {
            HashSet<DiscordAutoCompleteChoice> choices = new();

            string? value = ctx.OptionValue.ToString();
            if (value is not null)
            {
                string? game = GetValueFromOption(ctx, "game");
                string? platform = GetValueFromOption(ctx, "platform");

                if (!string.IsNullOrEmpty(game) && !string.IsNullOrEmpty(platform))
                {
                    foreach (KeyValuePair<string, string> release in DiscordBot.Instance.Cytrus.CytrusData.Games[game].GetReleasesFromPlatform(platform))
                        choices.Add(new(release.Key.Capitalize(), release.Key));
                }
            }

            return Task.FromResult(choices.AsEnumerable());
        }
    }
}
