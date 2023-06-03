using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Factories.Effects;
using Cyberia.Api.Managers;
using Cyberia.Salamandra.DsharpPlus;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

using System.Text;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class IncarnationMessageBuilder : ICustomMessageBuilder
    {
        public const string PACKET_HEADER = "INCA";
        public const int PACKET_VERSION = 1;

        private readonly Incarnation _incarnation;
        private readonly Item? _item;
        private readonly List<Spell> _spells;

        public IncarnationMessageBuilder(Incarnation incarnation)
        {
            _incarnation = incarnation;
            _item = incarnation.GetItem();
            _spells = incarnation.GetSpells();
        }

        public static IncarnationMessageBuilder? Create(int version, string[] parameters)
        {
            if (version == PACKET_VERSION &&
                parameters.Length > 0 &&
                int.TryParse(parameters[0], out int incarnationId))
            {
                Incarnation? incarnartion = Bot.Instance.Api.Datacenter.IncarnationsData.GetIncarnationById(incarnationId);
                if (incarnartion is not null)
                    return new(incarnartion);
            }

            return null;
        }

        public static string GetPacket(int incarnationId)
        {
            return InteractionManager.ComponentPacketBuilder(PACKET_HEADER, PACKET_VERSION, incarnationId);
        }

        public async Task<T> GetMessageAsync<T>() where T : IDiscordMessageBuilder, new()
        {
            IDiscordMessageBuilder message = new T()
                .AddEmbed(await EmbedBuilder());

            if (_spells.Count > 0)
                message.AddComponents(SpellComponentsBuilder.SpellsSelectBuilder(0, _spells));

            if (_item is not null)
                message.AddComponents(ItemComponentsBuilder.ItemButtonBuilder(_item));

            return (T)message;
        }

        private async Task<DiscordEmbedBuilder> EmbedBuilder()
        {
            DiscordEmbedBuilder embed = EmbedManager.BuildDofusEmbed(DofusEmbedCategory.Inventory, "Incarnations")
                .WithTitle($"{_incarnation.Name} ({_incarnation.Id})")
                .WithImageUrl(await _incarnation.GetImgPath());

            if (_item is not null)
            {
                embed.WithDescription(string.IsNullOrEmpty(_item.Description) ? "" : Formatter.Italic(_item.Description))
                    .WithThumbnail(await _item.GetImagePath())
                    .AddField("Niveau :", _item.Level.ToString(), true)
                    .AddField("Type :", Bot.Instance.Api.Datacenter.ItemsData.GetItemTypeNameById(_item.ItemTypeId), true)
                    .AddField(Constant.ZERO_WIDTH_SPACE, Constant.ZERO_WIDTH_SPACE, true);

                List<IEffect> effects = _incarnation.GetEffects();
                if (effects.Count > 0)
                    embed.AddEffectFields("Effets :", effects);

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
            }
            else
            {
                embed.WithDescription(Formatter.Italic("Incarnation non existante dans les langs du jeu"))
                    .WithThumbnail($"{Bot.Instance.Api.CdnUrl}/images/items/unknown.png");
            }

            if (_spells.Count > 0)
                embed.AddField("Sorts :", string.Join('\n', _spells.Select(x => $"- {Formatter.Bold(x.Name)}")));

            return embed;
        }
    }
}
