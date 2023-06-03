using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Factories.Effects;
using Cyberia.Salamandra.DsharpPlus;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class ItemSetMessageBuilder : ICustomMessageBuilder
    {
        public const string PACKET_HEADER = "IS";
        public const int PACKET_VERSION = 1;

        private readonly ItemSet _itemSet;
        private readonly int _nbItemSelected;
        private readonly List<Item> _items;
        private readonly Breed? _breed;

        public ItemSetMessageBuilder(ItemSet itemSet, int nbItemSelected = 2)
        {
            _itemSet = itemSet;
            _nbItemSelected = nbItemSelected;
            _items = itemSet.GetItems();
            _breed = itemSet.GetBreed();
        }

        public static ItemSetMessageBuilder? Create(int version, string[] parameters)
        {
            if (version == PACKET_VERSION &&
                parameters.Length > 1 &&
                int.TryParse(parameters[0], out int itemSetId) &&
                int.TryParse(parameters[1], out int nbItemSelected))
            {
                ItemSet? itemSet = Bot.Instance.Api.Datacenter.ItemSetsData.GetItemSetById(itemSetId);
                if (itemSet is not null)
                    return new(itemSet, nbItemSelected);
            }

            return null;
        }

        public static string GetPacket(int itemSetId, int nbItemSelected = 2)
        {
            return InteractionManager.ComponentPacketBuilder(PACKET_HEADER, PACKET_VERSION, itemSetId, nbItemSelected);
        }

        public async Task<T> GetMessageAsync<T>() where T : IDiscordMessageBuilder, new()
        {
            IDiscordMessageBuilder message = new T()
                .AddEmbed(await EmbedBuilder());

            List<DiscordButtonComponent> buttons = Buttons1Builder();
            if (buttons.Count > 0)
                message.AddComponents(buttons);

            buttons = Buttons2Builder();
            if (buttons.Count > 0)
                message.AddComponents(buttons);

            if (_items.Count > 0)
                message.AddComponents(ItemComponentsBuilder.ItemsSelectBuilder(0, _items));

            return (T)message;
        }

        private Task<DiscordEmbedBuilder> EmbedBuilder()
        {
            DiscordEmbedBuilder embed = EmbedManager.BuildDofusEmbed(DofusEmbedCategory.Inventory, "Panoplies")
                .WithTitle($"{_itemSet.Name} ({_itemSet.Id})")
                .AddField("Niveau :", _itemSet.GetLevel().ToString());

            if (_items.Count > 0)
                embed.AddField("Items :", string.Join('\n', _items.Select(x => $"- Niv.{x.Level} {Formatter.Bold(x.Name)}")));

            List<IEffect> effects = _itemSet.GetEffects(_nbItemSelected);
            if (effects.Count > 0)
                embed.AddEffectFields("Effets :", effects);

            return Task.FromResult(embed);
        }

        private List<DiscordButtonComponent> Buttons1Builder()
        {
            List<DiscordButtonComponent> components = new();

            for (int i = 2; i < _itemSet.ItemsId.Count + 1 && i < 7; i++)
                components.Add(new(ButtonStyle.Primary, GetPacket(_itemSet.Id, i), $"{i}/{_itemSet.ItemsId.Count}", _nbItemSelected == i));

            return components;
        }

        private List<DiscordButtonComponent> Buttons2Builder()
        {
            List<DiscordButtonComponent> components = new();

            for (int i = 7; i < _itemSet.ItemsId.Count + 1 && i < 10; i++)
                components.Add(new(ButtonStyle.Primary, GetPacket(_itemSet.Id, i), $"{i}/{_itemSet.ItemsId.Count}", _nbItemSelected == i));

            if (_breed is not null)
                components.Add(BreedComponentsBuilder.BreedButtonBuilder(_breed));

            return components;
        }
    }
}
