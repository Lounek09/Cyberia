﻿using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Data
{
    public sealed class CytrusPlatformAutocompleteProvider : AutocompleteProvider
    {
        public override Task<IEnumerable<DiscordAutoCompleteChoice>> Provider(AutocompleteContext ctx)
        {
            HashSet<DiscordAutoCompleteChoice> choices = new();

            string? value = ctx.OptionValue.ToString();
            if (value is not null)
            {
                string? game = GetValueFromOption<string>(ctx, "game");

                if (!string.IsNullOrEmpty(game))
                {
                    foreach (KeyValuePair<string, Dictionary<string, string>> platform in Bot.Instance.Cytrus.CytrusData.Games[game].Platforms)
                        choices.Add(new(platform.Key.Capitalize(), platform.Key));
                }
            }

            return Task.FromResult(choices.AsEnumerable());
        }
    }
}