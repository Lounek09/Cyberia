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

        private readonly BreedData _breedData;
        private readonly List<SpellData> _spellsData;
        private readonly SpellData? _specialSpellData;
        private readonly ItemSetData? _itemSetData;

        public BreedMessageBuilder(BreedData breedData)
        {
            _breedData = breedData;
            _spellsData = breedData.GetSpellsData();
            _specialSpellData = breedData.GetSpecialSpellData();
            _itemSetData = breedData.GetItemSetData();
        }

        public static BreedMessageBuilder? Create(int version, string[] parameters)
        {
            if (version == PACKET_VERSION &&
                parameters.Length > 0 &&
                int.TryParse(parameters[0], out int breedId))
            {
                BreedData? breedData = Bot.Instance.Api.Datacenter.BreedsData.GetBreedDataById(breedId);
                if (breedData is not null)
                    return new(breedData);
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

            if (_spellsData.Count > 0)
                message.AddComponents(SpellComponentsBuilder.SpellsSelectBuilder(0, _spellsData));

            List<DiscordButtonComponent> buttons = ButtonsBuilder();
            if (buttons.Count > 0)
                message.AddComponents(buttons);

            return (T)message;
        }

        private Task<DiscordEmbedBuilder> EmbedBuilder()
        {
            DiscordEmbedBuilder embed = EmbedManager.BuildDofusEmbed(DofusEmbedCategory.Breeds, "Classes")
                .WithTitle($"{_breedData.LongName} ({_breedData.Id})")
                .WithDescription(Formatter.Italic(_breedData.Description))
                .WithThumbnail(_breedData.GetIconImagePath())
                .WithImageUrl(_breedData.GetPreferenceWeaponsImagePath())
                .AddField("Caractérisques :", _breedData.GetCaracteristics());

            if (_spellsData.Count > 0)
                embed.AddField("Sorts :", string.Join('\n', _spellsData.Select(x => $"- Niv.{x.GetNeededLevel()} {Formatter.Bold(x.Name)}")));

            if (Bot.Instance.Api.Config.Temporis)
                embed.AddField("Temporis :", $"{Formatter.Bold(_breedData.TemporisPassiveName)} :\n{_breedData.TemporisPassiveDescription}");

            return Task.FromResult(embed);
        }

        private List<DiscordButtonComponent> ButtonsBuilder()
        {
            List<DiscordButtonComponent> buttons = new();

            if (_specialSpellData is not null)
                buttons.Add(SpellComponentsBuilder.SpellButtonBuilder(_specialSpellData));

            if (_itemSetData is not null)
                buttons.Add(ItemSetComponentsBuilder.ItemSetButtonBuilder(_itemSetData));

            return buttons;
        }
    }
}
