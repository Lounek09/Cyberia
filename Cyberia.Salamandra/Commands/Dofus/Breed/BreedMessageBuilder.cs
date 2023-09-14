using Cyberia.Api.DatacenterNS;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class BreedMessageBuilder : ICustomMessageBuilder
    {
        public const string PACKET_HEADER = "G";
        public const int PACKET_VERSION = 1;

        private readonly Breed _breed;
        private readonly List<Spell> _spells;
        private readonly Spell? _specialSpell;
        private readonly ItemSet? _itemSet;

        public BreedMessageBuilder(Breed breed)
        {
            _breed = breed;
            _spells = breed.GetSpells();
            _specialSpell = breed.GetSpecialSpell();
            _itemSet = breed.GetItemSet();
        }

        public static BreedMessageBuilder? Create(int version, string[] parameters)
        {
            if (version == PACKET_VERSION &&
                parameters.Length > 0 &&
                int.TryParse(parameters[0], out int breedId))
            {
                Breed? breed = Bot.Instance.Api.Datacenter.BreedsData.GetBreedById(breedId);
                if (breed is not null)
                    return new(breed);
            }

            return null;
        }

        public static string GetPacket(int breedId)
        {
            return InteractionManager.ComponentPacketBuilder(PACKET_HEADER, PACKET_VERSION, breedId);
        }

        public async Task<T> GetMessageAsync<T>() where T : IDiscordMessageBuilder, new()
        {
            IDiscordMessageBuilder message = new T()
                .AddEmbed(await EmbedBuilder());

            if (_spells.Count > 0)
                message.AddComponents(SpellComponentsBuilder.SpellsSelectBuilder(0, _spells));

            List<DiscordButtonComponent> buttons = ButtonsBuilder();
            if (buttons.Count > 0)
                message.AddComponents(buttons);

            return (T)message;
        }

        private Task<DiscordEmbedBuilder> EmbedBuilder()
        {
            DiscordEmbedBuilder embed = EmbedManager.BuildDofusEmbed(DofusEmbedCategory.Breeds, "Classes")
                .WithTitle($"{_breed.LongName} ({_breed.Id})")
                .WithDescription(Formatter.Italic(_breed.Description))
                .WithThumbnail(_breed.GetIconImagePath())
                .WithImageUrl(_breed.GetPreferenceWeaponsImagePath())
                .AddField("Caractérisques :", _breed.GetCaracteristics());

            if (_spells.Count > 0)
                embed.AddField("Sorts :", string.Join('\n', _spells.Select(x => $"- Niv.{x.GetNeededLevel()} {Formatter.Bold(x.Name)}")));

            if (Bot.Instance.Api.Config.Temporis)
                embed.AddField("Temporis :", $"{Formatter.Bold(_breed.TemporisPassiveName)} :\n{_breed.TemporisPassiveDescription}");

            return Task.FromResult(embed);
        }

        private List<DiscordButtonComponent> ButtonsBuilder()
        {
            List<DiscordButtonComponent> buttons = new();

            if (_specialSpell is not null)
                buttons.Add(SpellComponentsBuilder.SpellButtonBuilder(_specialSpell));

            if (_itemSet is not null)
                buttons.Add(ItemSetComponentsBuilder.ItemSetButtonBuilder(_itemSet));

            return buttons;
        }
    }
}
