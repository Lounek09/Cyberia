using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Factories;
using Cyberia.Api.Managers;
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
        public const int PACKET_VERSION = 1;

        private readonly Item _item;
        private readonly ItemSet? _itemSet;
        private readonly ItemStats? _itemStats;
        private readonly Craft? _craft;
        private readonly Incarnation? _incarnation;

        public ItemMessageBuilder(Item item)
        {
            _item = item;
            _itemSet = item.GetItemSet();
            _itemStats = item.GetItemStat();
            _craft = item.GetCraft();
            _incarnation = Bot.Instance.Api.Datacenter.IncarnationsData.GetIncarnationByItemId(_item.Id);
        }

        public static ItemMessageBuilder? Create(int version, string[] parameters)
        {
            if (version == PACKET_VERSION &&
                parameters.Length > 0 &&
                int.TryParse(parameters[0], out int itemId))
            {
                Item? item = Bot.Instance.Api.Datacenter.ItemsData.GetItemById(itemId);
                if (item is not null)
                    return new(item);
            }

            return null;
        }

        public static string GetPacket(int itemId)
        {
            return InteractionManager.ComponentPacketBuilder(PACKET_HEADER, PACKET_VERSION, itemId);
        }

        public async Task<T> GetMessageAsync<T>() where T : IDiscordMessageBuilder, new()
        {
            IDiscordMessageBuilder message = new T()
                .AddEmbed(await EmbedBuilder());

            List<DiscordButtonComponent> buttons = ButtonsBuilder();
            if (buttons.Count > 0)
                message.AddComponents(buttons);

            return (T)message;
        }

        private async Task<DiscordEmbedBuilder> EmbedBuilder()
        {
            DiscordEmbedBuilder embed = EmbedManager.BuildDofusEmbed(DofusEmbedCategory.Inventory, "Items")
                .WithTitle($"{_item.Name.SanitizeMarkDown()} ({_item.Id})")
                .WithDescription(string.IsNullOrEmpty(_item.Description) ? "" : Formatter.Italic(_item.Description))
                .WithThumbnail(await _item.GetImagePath())
                .AddField("Niveau :", _item.Level.ToString(), true)
                .AddField("Type :", Bot.Instance.Api.Datacenter.ItemsData.GetItemTypeNameById(_item.ItemTypeId), true);

            if (_itemSet is not null)
                embed.AddField("Panoplie :", _itemSet.Name, true);

            if (_itemStats is not null)
                embed.AddEffectFields("Effets :", _itemStats.Effects);

            List<string> criteria = CriterionFactory.GetCriteriaParse(_item.Criterion);
            if (criteria.Count > 0)
                embed.AddFields("Conditions :", criteria);

            if (_item.WeaponInfos is not null)
            {
                StringBuilder caracteristicsBuilder = new();
                caracteristicsBuilder.AppendFormat("PA : {0}\n", _item.WeaponInfos.ActionPointCost);
                caracteristicsBuilder.AppendFormat("Portée : {0}{1}\n", _item.WeaponInfos.MinRange, (_item.WeaponInfos.MinRange == _item.WeaponInfos.MaxRange ? "" : $" à {_item.WeaponInfos.MaxRange}"));

                if (_item.WeaponInfos.CriticalBonus != 0)
                    caracteristicsBuilder.AppendFormat("Bonus coups critique : {0}\n", _item.WeaponInfos.CriticalBonus);

                if (_item.WeaponInfos.CriticalHitRate != 0)
                {
                    caracteristicsBuilder.AppendFormat("Critique : 1/{0}", _item.WeaponInfos.CriticalHitRate);
                    caracteristicsBuilder.Append(_item.WeaponInfos.CriticalFailureRate != 0 ? " - " : "\n");
                }

                if (_item.WeaponInfos.CriticalFailureRate != 0)
                    caracteristicsBuilder.AppendFormat("Échec : 1/{0}\n", _item.WeaponInfos.CriticalFailureRate);

                if (_item.WeaponInfos.LineOnly)
                    caracteristicsBuilder.AppendLine("Lancer en ligne uniquement");

                if (!_item.WeaponInfos.LineOfSight && _item.WeaponInfos.MaxRange != 1)
                    caracteristicsBuilder.AppendLine("Ne possède pas de ligne de vue");

                caracteristicsBuilder.Append(_item.TwoHanded ? "Arme à deux mains" : "Arme à une main");

                ItemType? itemType = _item.GetItemType();
                if (itemType is not null && itemType.Area.Id != EffectAreaManager.BaseArea.Id)
                    caracteristicsBuilder.AppendFormat("\nZone : {0} {1}", Emojis.Area(itemType.Area.Id), itemType.Area.GetDescription());

                embed.AddField("Caractéristiques :", caracteristicsBuilder.ToString());
            }

            if (_craft is not null)
            {
                string recipe = string.Join(" + ", _craft.GetCraftToString(1, false, false));
                if (!string.IsNullOrEmpty(recipe))
                    embed.AddField("Craft :", recipe);
            }

            StringBuilder miscellaneousBuilder = new();
            miscellaneousBuilder.AppendFormat("{0} pod{1}", _item.Weight.ToStringThousandSeparator(), _item.Weight > 1 ? "s" : "");
            miscellaneousBuilder.Append(_item.Ceremonial ? ", objet d'apparat" : "");
            miscellaneousBuilder.Append(_item.IsReallyEnhanceable() ? ", forgemageable" : "");
            miscellaneousBuilder.Append(_item.Ethereal ? ", item éthéré" : "");
            miscellaneousBuilder.Append(_item.Usable ? ", est consommable" : "");
            miscellaneousBuilder.Append(_item.Targetable ? ", est ciblable" : "");
            miscellaneousBuilder.Append(_item.Cursed ? ", malédiction" : "");
            embed.AddField("Divers :", miscellaneousBuilder.ToString());

            return embed;
        }

        private List<DiscordButtonComponent> ButtonsBuilder()
        {
            List<DiscordButtonComponent> buttons = new();

            if (_itemSet is not null)
                buttons.Add(ItemSetComponentsBuilder.ItemSetButtonBuilder(_itemSet));

            if (_craft is not null)
                buttons.Add(CraftComponentsBuilder.CraftButtonBuilder(_craft));

            if (_incarnation is not null)
                buttons.Add(IncarnationComponentsBuilder.IncarnationButtonBuilder(_incarnation));

            return buttons;
        }
    }
}
