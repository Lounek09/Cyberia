using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Factories.Effects;
using Cyberia.Api.Managers;
using Cyberia.Salamandra.DsharpPlus;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class IncarnationMessageBuilder : CustomMessageBuilder
    {
        private readonly Incarnation _incarnation;
        private readonly Item? _item;
        private readonly List<Spell> _spells;

        public IncarnationMessageBuilder(Incarnation incarnation) :
            base()
        {
            _incarnation = incarnation;
            _item = incarnation.GetItem();
            _spells = incarnation.GetSpells();
        }

        protected override async Task<DiscordEmbedBuilder> EmbedBuilder()
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
                    ItemType? itemType = _item.GetItemType();

                    string caracteristics = $"PA : {_item.WeaponInfos.ActionPointCost}\n";
                    caracteristics += $"Portée : {_item.WeaponInfos.MinRange}{(_item.WeaponInfos.MinRange == _item.WeaponInfos.MaxRange ? "" : $" à {_item.WeaponInfos.MaxRange}")}\n";
                    caracteristics += _item.WeaponInfos.CriticalBonus == 0 ? "" : $"Bonus coups critique : {_item.WeaponInfos.CriticalBonus}\n";
                    caracteristics += $"{(_item.WeaponInfos.CriticalHitRate == 0 ? "" : $"Critique : 1/{_item.WeaponInfos.CriticalHitRate}{(_item.WeaponInfos.CriticalFailureRate == 0 ? "\n" : "")}")}{(_item.WeaponInfos.CriticalHitRate != 0 && _item.WeaponInfos.CriticalFailureRate != 0 ? " - " : "")}{(_item.WeaponInfos.CriticalFailureRate == 0 ? "" : $"Échec : 1/{_item.WeaponInfos.CriticalFailureRate}\n")}";
                    caracteristics += _item.WeaponInfos.LineOnly ? "Lancer en ligne uniquement\n" : "";
                    caracteristics += _item.WeaponInfos.LineOfSight || _item.WeaponInfos.MaxRange == 1 ? "" : "Ne possède pas de ligne de vue\n";
                    caracteristics += _item.TwoHanded ? "Arme à deux mains" : "Arme à une main";
                    caracteristics += itemType is null || itemType.Area.Id == EffectAreaManager.BaseArea.Id ? "" : $"\nZone : {Emojis.Area(itemType.Area.Id)} {itemType.Area.GetDescription()}";
                    embed.AddField("Caractéristiques :", caracteristics);
                }
            }
            else
            {
                embed.WithDescription(Formatter.Italic("Incarnation non existante dans les langs du jeu"))
                    .WithThumbnail($"{Bot.Instance.Api.CdnUrl}/images/items/unknown.png");
            }

            if (_spells.Count > 0)
            {
                HashSet<string> spellsName = new();
                foreach (Spell spell in _spells)
                    spellsName.Add($"- {Formatter.Bold(spell.Name)} ({spell.Id})");

                embed.AddField("Sorts :", string.Join('\n', spellsName));
            }

            return embed;
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

            if (_item is not null)
                buttons.Add(new(ButtonStyle.Primary, "item", "Item"));

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
            else if (e.Id.Equals("item"))
            {
                Item? item = _incarnation.GetItem();
                if (item is not null)
                {
                    await new ItemMessageBuilder(item).UpdateInteractionResponse(e.Interaction);
                    return true;
                }
            }

            await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
            return false;
        }
    }
}
