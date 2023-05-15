using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Factories;
using Cyberia.Api.Managers;
using Cyberia.Salamandra.DsharpPlus;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class ItemMessageBuilder : CustomMessageBuilder
    {
        private readonly Item _item;
        private readonly ItemSet? _itemSet;
        private readonly ItemStats? _itemStats;
        private readonly Craft? _craft;
        private readonly Incarnation? _incarnation;

        public ItemMessageBuilder(Item item) :
            base()
        {
            _item = item;
            _itemSet = item.GetItemSet();
            _itemStats = item.GetItemStat();
            _craft = item.GetCraft();
            _incarnation = Bot.Instance.Api.Datacenter.IncarnationsData.GetIncarnationByItemId(_item.Id);
        }

        protected override async Task<DiscordEmbedBuilder> EmbedBuilder()
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
                ItemType? itemType = _item.GetItemType();

                string caracteristics = $"PA : {_item.WeaponInfos.ActionPointCost}\n";
                caracteristics += $"Portée : {_item.WeaponInfos.MinRange}{(_item.WeaponInfos.MinRange == _item.WeaponInfos.MaxRange ? "" : $" à {_item.WeaponInfos.MaxRange}")}\n";
                caracteristics += _item.WeaponInfos.CriticalBonus == 0 ? "" : $"Bonus coups critique : {_item.WeaponInfos.CriticalBonus}\n";
                caracteristics += $"{(_item.WeaponInfos.CriticalHitRate == 0 ? "" : $"Critique : 1/{_item.WeaponInfos.CriticalHitRate}{(_item.WeaponInfos.CriticalFailureRate == 0 ? "\n" : "")}")}{(_item.WeaponInfos.CriticalHitRate != 0 && _item.WeaponInfos.CriticalFailureRate != 0 ? " - " : "")}{(_item.WeaponInfos.CriticalFailureRate == 0 ? "" : $"Échec : 1/{_item.WeaponInfos.CriticalFailureRate}\n")}";
                caracteristics += _item.WeaponInfos.LineOnly ? "Lancer en ligne uniquement\n" : "";
                caracteristics += _item.WeaponInfos.LineOfSight || _item.WeaponInfos.MaxRange == 1 ? "" : "Ne possède pas de ligne de vue\n";
                caracteristics += _item.TwoHanded ? "Arme à deux mains" : "Arme à une main";
                caracteristics += itemType is null || itemType.Area.Symbol == EffectAreaManager.BaseArea.Symbol ? "" : $"\nZone : {Emojis.Area(itemType.Area.Symbol)}{itemType.Area.GetDescription()}";
                embed.AddField("Caractéristiques :", caracteristics);
            }

            if (_craft is not null)
            {
                string recipe = string.Join(" + ", _craft.GetCraftToString(1, false, false));
                if (!string.IsNullOrEmpty(recipe))
                    embed.AddField("Craft :", recipe);
            }

            string miscellaneous = $"{_item.Weight.ToStringThousandSeparator()} pod{(_item.Weight > 1 ? "s" : "")}";
            miscellaneous += _item.Ceremonial ? ", objet d'apparat" : "";
            miscellaneous += _item.IsReallyEnhanceable() ? ", forgemageable" : "";
            miscellaneous += _item.Ethereal ? ", item éthéré" : "";
            miscellaneous += _item.Usable ? ", est consommable" : "";
            miscellaneous += _item.Targetable ? ", est ciblable" : "";
            miscellaneous += _item.Cursed ? ", malédiction" : "";
            embed.AddField("Divers :", miscellaneous);

            return embed;
        }

        private HashSet<DiscordButtonComponent> ButtonsBuilder()
        {
            HashSet<DiscordButtonComponent> buttons = new();

            if (_itemSet is not null)
                buttons.Add(new(ButtonStyle.Primary, "itemset", "Panoplie"));

            if (_craft is not null)
                buttons.Add(new(ButtonStyle.Primary, "craft", "Craft"));

            if (_incarnation is not null)
                buttons.Add(new(ButtonStyle.Primary, "incarnation", "Incarnation"));

            return buttons;
        }

        protected override async Task<DiscordInteractionResponseBuilder> InteractionResponseBuilder()
        {
            DiscordInteractionResponseBuilder response = await base.InteractionResponseBuilder();

            HashSet<DiscordButtonComponent> buttons = ButtonsBuilder();
            if (buttons.Count > 0)
                response.AddComponents(buttons);

            return response;
        }

        protected override async Task<DiscordFollowupMessageBuilder> FollowupMessageBuilder()
        {
            DiscordFollowupMessageBuilder followupMessage = await base.FollowupMessageBuilder();

            HashSet<DiscordButtonComponent> buttons = ButtonsBuilder();
            if (buttons.Count > 0)
                followupMessage.AddComponents(buttons);

            return followupMessage;
        }

        protected override async Task<bool> InteractionTreatment(ComponentInteractionCreateEventArgs e)
        {
            if (e.Id.Equals("itemset"))
            {
                if (_itemSet is not null)
                {
                    await new ItemSetMessageBuilder(_itemSet).UpdateInteractionResponse(e.Interaction);
                    return true;
                }
            }
            else if (e.Id.Equals("craft"))
            {
                if (_craft is not null)
                {
                    await new CraftMessageBuilder(_craft, 1).UpdateInteractionResponse(e.Interaction);
                    return true;
                }
            }
            else if (e.Id.Equals("incarnation"))
            {
                if (_incarnation is not null)
                {
                    await new IncarnationMessageBuilder(_incarnation).UpdateInteractionResponse(e.Interaction);
                    return true;
                }
            }

            await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
            return false;
        }
    }
}
