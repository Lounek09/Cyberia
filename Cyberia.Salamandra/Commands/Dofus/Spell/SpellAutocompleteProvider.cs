﻿using Cyberia.Api.DatacenterNS;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class SpellAutocompleteProvider : AutocompleteProvider
    {
        public override Task<IEnumerable<DiscordAutoCompleteChoice>> Provider(AutocompleteContext ctx)
        {
            string? value = ctx.OptionValue.ToString();

            List<DiscordAutoCompleteChoice> choices = new();
            if (value is not null && value.Length >= MIN_LENGTH_AUTOCOMPLETE)
            {
                foreach (Spell spell in Bot.Instance.Api.Datacenter.SpellsData.GetSpellsByName(value).Take(25))
                    choices.Add(new($"{spell.Name.WithMaxLength(90)} ({spell.Id})", spell.Id.ToString()));
            }

            return Task.FromResult(choices.AsEnumerable());
        }
    }
}