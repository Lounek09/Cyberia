using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Salamandra.Bot.Commands.Data
{
    public sealed class CytrusNewVersionAutocompleteProvider : AutocompleteProvider
    {
        public override Task<IEnumerable<DiscordAutoCompleteChoice>> Provider(AutocompleteContext ctx)
        {
            HashSet<DiscordAutoCompleteChoice> choices = new();

            string? value = ctx.OptionValue.ToString();
            if (value is not null)
            {
                string? game = GetValueFromOption<string>(ctx, "game");
                string? platform = GetValueFromOption<string>(ctx, "platform");
                string? release = GetValueFromOption<string>(ctx, "new_release");

                if (!string.IsNullOrEmpty(game) && !string.IsNullOrEmpty(platform) && !string.IsNullOrEmpty(release))
                {
                    string? version = DiscordBot.Instance.Cytrus.CytrusData.Games[game].GetVersionFromPlatformAndRelease(platform, release);

                    if (!string.IsNullOrEmpty(version))
                        choices.Add(new(version, version));
                }
            }

            return Task.FromResult(choices.AsEnumerable());
        }
    }
}
