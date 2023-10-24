using Cyberia.Api.DatacenterNS;
using Cyberia.Salamandra.DsharpPlus;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

using System.Text;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class ItemMessageBuilder : ICustomMessageBuilder
    {
        public const string PACKET_HEADER = "I";
        public const int PACKET_VERSION = 2;

        private readonly ItemData _itemData;
        private readonly ItemTypeData? _itemTypeData;
        private readonly ItemSetData? _itemSetData;
        private readonly ItemStatsData? _itemStatsData;
        private readonly PetData? _petData;
        private readonly CraftData? _craftData;
        private readonly int _craftQte;
        private readonly IncarnationData? _incarnationData;

        public ItemMessageBuilder(ItemData itemData, int craftQte = 1)
        {
            _itemData = itemData;
            _itemTypeData = itemData.GetItemTypeData();
            _itemSetData = itemData.GetItemSetData();
            _itemStatsData = itemData.GetItemStatData();
            _petData = _itemData.ItemTypeId == ItemTypeData.TYPE_PET ? Bot.Instance.Api.Datacenter.PetsData.GetPetDataByItemId(_itemData.Id) : null;
            _craftData = itemData.GetCraftData();
            _craftQte = craftQte;
            _incarnationData = _itemData.IsWeapon() ? Bot.Instance.Api.Datacenter.IncarnationsData.GetIncarnationDataByItemId(_itemData.Id) : null;
        }

        public static ItemMessageBuilder? Create(int version, string[] parameters)
        {
            if (version == PACKET_VERSION &&
                parameters.Length > 1 &&
                int.TryParse(parameters[0], out int itemId) &&
                int.TryParse(parameters[1], out int craftQte))
            {
                ItemData? itemData = Bot.Instance.Api.Datacenter.ItemsData.GetItemDataById(itemId);
                if (itemData is not null)
                {
                    return new(itemData, craftQte);
                }
            }

            return null;
        }

        public static string GetPacket(int itemId, int craftQte = 1)
        {
            return InteractionManager.ComponentPacketBuilder(PACKET_HEADER, PACKET_VERSION, itemId, craftQte);
        }

        public async Task<T> GetMessageAsync<T>() where T : IDiscordMessageBuilder, new()
        {
            IDiscordMessageBuilder message = new T()
                .AddEmbed(await EmbedBuilder());

            List<DiscordButtonComponent> buttons = ButtonsBuilder();
            if (buttons.Count > 0)
            {
                message.AddComponents(buttons);
            }

            return (T)message;
        }

        private async Task<DiscordEmbedBuilder> EmbedBuilder()
        {
            DiscordEmbedBuilder embed = EmbedManager.BuildDofusEmbed(DofusEmbedCategory.Inventory, "Items")
                .WithTitle($"{Formatter.Sanitize(_itemData.Name)} ({_itemData.Id})")
                .WithDescription(string.IsNullOrEmpty(_itemData.Description) ? "" : Formatter.Italic(_itemData.Description))
                .WithThumbnail(await _itemData.GetImagePath())
                .AddField("Niveau :", _itemData.Level.ToString(), true)
                .AddField("Type :", Bot.Instance.Api.Datacenter.ItemsData.GetItemTypeNameById(_itemData.ItemTypeId), true);

            if (_itemSetData is not null)
            {
                embed.AddField("Panoplie :", _itemSetData.Name, true);
            }

            if (_itemStatsData is not null)
            {
                embed.AddEffectFields("Effets :", _itemStatsData.Effects);
            }

            if (_itemData.Criteria.Count > 0)
            {
                embed.AddCriteriaFields(_itemData.Criteria);
            }

            if (_itemData.WeaponData is not null)
            {
                embed.AddWeaponInfosField(_itemData.WeaponData, _itemData.TwoHanded, _itemTypeData);
            }

            if (_petData is not null)
            {
                embed.AddPetField(_petData);
            }

            if (_craftData is not null)
            {
                embed.AddCraftField(_craftData, _craftQte);
            }

            StringBuilder miscellaneousBuilder = new();
            miscellaneousBuilder.AppendFormat("{0} pod{1}", _itemData.Weight.ToStringThousandSeparator(), _itemData.Weight > 1 ? "s" : "");
            if (_itemData.Tradeable()) miscellaneousBuilder.AppendFormat(", se vend {0}{1} aux pnj", _itemData.GetNpcRetailPrice().ToStringThousandSeparator(), Emojis.KAMAS);
            if (_itemData.Ceremonial) miscellaneousBuilder.Append(", objet d'apparat");
            if (_itemData.IsReallyEnhanceable()) miscellaneousBuilder.Append(", forgemageable");
            if (_itemData.Ethereal) miscellaneousBuilder.Append(", item éthéré");
            if (_itemData.Usable) miscellaneousBuilder.Append(", est consommable");
            if (_itemData.Targetable) miscellaneousBuilder.Append(", est ciblable");
            if (_itemData.Cursed) miscellaneousBuilder.Append(", malédiction");
            embed.AddField("Divers :", miscellaneousBuilder.ToString());

            return embed;
        }

        private List<DiscordButtonComponent> ButtonsBuilder()
        {
            List<DiscordButtonComponent> buttons = new();

            if (_itemSetData is not null)
            {
                buttons.Add(ItemSetComponentsBuilder.ItemSetButtonBuilder(_itemSetData));
            }

            if (_craftData is not null)
            {
                buttons.Add(CraftComponentsBuilder.CraftButtonBuilder(_craftData, _craftQte));
            }

            if (_incarnationData is not null)
            {
                buttons.Add(IncarnationComponentsBuilder.IncarnationButtonBuilder(_incarnationData));
            }

            return buttons;
        }
    }
}
