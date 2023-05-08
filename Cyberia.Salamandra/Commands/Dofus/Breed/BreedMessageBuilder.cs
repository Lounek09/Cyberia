using Cyberia.Api.DatacenterNS;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class BreedMessageBuilder : CustomMessageBuilder
    {
        private readonly Breed _breed;
        private readonly List<Spell> _spells;
        private readonly Spell? _breedSpell;
        private readonly ItemSet? _itemSet;

        public BreedMessageBuilder(Breed breed) :
            base()
        {
            _breed = breed;
            _spells = breed.GetSpells();
            _breedSpell = breed.GetBreedSpell();
            _itemSet = breed.GetItemSet();
        }

        protected override Task<DiscordEmbedBuilder> EmbedBuilder()
        {
            DiscordEmbedBuilder embed = EmbedManager.BuildDofusEmbed(DofusEmbedCategory.Breeds, "Classes")
                .WithTitle($"{_breed.LongName} ({_breed.Id})")
                .WithDescription(Formatter.Italic(_breed.Description))
                .WithThumbnail(_breed.GetIconImgPath())
                .WithImageUrl(_breed.GetPreferenceWeaponsImgPath())
                .AddField("Caractérisques :", _breed.GetCaracteristics());

            if (_spells.Count > 0)
            {
                HashSet<string> spellsName = new();
                foreach (Spell spell in _spells)
                    spellsName.Add($"Niv.{spell.GetNeededLevel()} {Formatter.Bold(spell.Name)}");

                embed.AddField("Sorts :", string.Join('\n', spellsName));
            }

            if (Bot.Instance.Api.Temporis)
                embed.AddField("Temporis :", $"{Formatter.Bold(_breed.TemporisPassiveName)} :\n{_breed.TemporisPassiveDescription}");

            return Task.FromResult(embed);
        }

        private DiscordSelectComponent SelectBuilder()
        {
            HashSet<DiscordSelectComponentOption> options = new();

            foreach (Spell spell in _spells)
                options.Add(new(spell.Name.WithMaxLength(100), spell.Id.ToString(), spell.Id.ToString()));

            return new("select", "Sélectionne un sort pour l'afficher", options);
        }

        private HashSet<DiscordButtonComponent> ButtonsBuilder()
        {
            HashSet<DiscordButtonComponent> buttons = new();

            if (_breedSpell is not null)
                buttons.Add(new(ButtonStyle.Success, "breedspell", _breedSpell.Name));

            if (_itemSet is not null)
                buttons.Add(new(ButtonStyle.Success, "itemset", _itemSet.Name));

            return buttons;
        }

        protected override async Task<DiscordInteractionResponseBuilder> InteractionResponseBuilder()
        {
            DiscordInteractionResponseBuilder response = await base.InteractionResponseBuilder();

            DiscordSelectComponent select = SelectBuilder();
            if (select.Options.Count > 1)
                response.AddComponents(select);

            HashSet<DiscordButtonComponent> buttons = ButtonsBuilder();
            if (buttons.Count > 0)
                response.AddComponents(buttons);

            return response;
        }

        protected override async Task<DiscordFollowupMessageBuilder> FollowupMessageBuilder()
        {
            DiscordFollowupMessageBuilder followupMessage = await base.FollowupMessageBuilder();

            DiscordSelectComponent select = SelectBuilder();
            if (select.Options.Count > 1)
                followupMessage.AddComponents(select);

            HashSet<DiscordButtonComponent> buttons = ButtonsBuilder();
            if (buttons.Count > 0)
                followupMessage.AddComponents(buttons);

            return followupMessage;
        }

        protected override async Task<bool> InteractionTreatment(ComponentInteractionCreateEventArgs e)
        {
            if (e.Id.Equals("select"))
            {
                int id = Convert.ToInt32(e.Interaction.Data.Values.First());

                Spell? spell = Bot.Instance.Api.Datacenter.SpellsData.GetSpellById(id);
                if (spell is not null)
                {
                    await new SpellMessageBuilder(spell).UpdateInteractionResponse(e.Interaction);
                    return true;
                }
            }
            else if (e.Id.Equals("breedspell"))
            {
                if (_breedSpell is not null)
                {
                    await new SpellMessageBuilder(_breedSpell).UpdateInteractionResponse(e.Interaction);
                    return true;
                }
            }
            else if (e.Id.Equals("itemset"))
            {
                if (_itemSet is not null)
                {
                    await new ItemSetMessageBuilder(_itemSet).UpdateInteractionResponse(e.Interaction);
                    return true;
                }
            }

            await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
            return false;
        }
    }
}
