using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Data
{
    public sealed class CytrusOldVersionAutocompleteProvider : AutocompleteProvider
    {
        public override Task<IEnumerable<DiscordAutoCompleteChoice>> Provider(AutocompleteContext ctx)
        {
            HashSet<DiscordAutoCompleteChoice> choices = new();

            string? value = ctx.OptionValue.ToString();
            if (value is not null)
            {
                string? game = GetValueFromOption<string>(ctx, "game");
                string? platform = GetValueFromOption<string>(ctx, "platform");
                string? release = GetValueFromOption<string>(ctx, "old_release");

                if (!string.IsNullOrEmpty(game) && !string.IsNullOrEmpty(platform) && !string.IsNullOrEmpty(release))
                {
                    string? version = Bot.Instance.Cytrus.OldCytrusData.Games[game].GetVersionFromPlatformAndRelease(platform, release);

                    if (!string.IsNullOrEmpty(version))
                        choices.Add(new(version, version));
                }
            }

            return Task.FromResult(choices.AsEnumerable());
        }
    }
}
