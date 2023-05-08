using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Factories.Effects;
using Cyberia.Salamandra.DsharpPlus;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class ItemSetMessageBuilder : CustomMessageBuilder
    {
        private readonly ItemSet _itemSet;
        private readonly List<Item> _items;
        private readonly Breed? _breed;
        private int _nbItemSelected;

        public ItemSetMessageBuilder(ItemSet itemSet) :
            base()
        {
            _itemSet = itemSet;
            _items = itemSet.GetItems();
            _breed = itemSet.GetBreed();
            _nbItemSelected = itemSet.ItemsId.Count;
        }

        protected override Task<DiscordEmbedBuilder> EmbedBuilder()
        {
            DiscordEmbedBuilder embed = EmbedManager.BuildDofusEmbed(DofusEmbedCategory.Inventory, "Panoplies")
                .WithTitle($"{_itemSet.Name} ({_itemSet.Id})")
                .AddField("Niveau :", _itemSet.GetLevel().ToString());

            if (_items.Count > 0)
            {
                HashSet<string> itemsName = new();
                foreach (Item item in _items)
                    itemsName.Add($"- Niv.{item.Level} {Formatter.Bold(item.Name)}");

                embed.AddField("Items :", string.Join('\n', itemsName));
            }

            List<IEffect> effects = _itemSet.GetEffects(_nbItemSelected);
            if (effects.Count > 0)
                embed.AddEffectFields("Effets :", effects);

            return Task.FromResult(embed);
        }

        private HashSet<DiscordButtonComponent> Buttons1Builder()
        {
            HashSet<DiscordButtonComponent> components = new();

            for (int i = 2; i < _itemSet.ItemsId.Count + 1 && i < 7; i++)
                components.Add(new(ButtonStyle.Primary, i.ToString(), $"{i}/{_itemSet.ItemsId.Count}", _nbItemSelected == i));

            return components;
        }

        private HashSet<DiscordButtonComponent> Buttons2Builder()
        {
            HashSet<DiscordButtonComponent> components = new();

            for (int i = 7; i < _itemSet.ItemsId.Count + 1 && i < 10; i++)
                components.Add(new(ButtonStyle.Primary, i.ToString(), $"{i}/{_itemSet.ItemsId.Count}", _nbItemSelected == i));

            if (_breed is not null)
                components.Add(new(ButtonStyle.Success, "breed", _breed.Name));

            return components;
        }

        private DiscordSelectComponent SelectBuilder()
        {
            HashSet<DiscordSelectComponentOption> options = new();
            foreach (Item item in _items)
                options.Add(new(item.Name.WithMaxLength(100), item.Id.ToString(), item.Id.ToString()));

            return new("select", "Sélectionne un item pour l'afficher", options);
        }

        protected override async Task<DiscordInteractionResponseBuilder> InteractionResponseBuilder()
        {
            DiscordInteractionResponseBuilder response = await base.InteractionResponseBuilder();

            HashSet<DiscordButtonComponent> nbItemButtons = Buttons1Builder();
            if (nbItemButtons.Count > 0)
                response.AddComponents(nbItemButtons);

            nbItemButtons = Buttons2Builder();
            if (nbItemButtons.Count > 0)
                response.AddComponents(nbItemButtons);

            DiscordSelectComponent select = SelectBuilder();
            if (select.Options.Count > 1)
                response.AddComponents(select);

            return response;
        }

        protected override async Task<DiscordFollowupMessageBuilder> FollowupMessageBuilder()
        {
            DiscordFollowupMessageBuilder followupMessage = await base.FollowupMessageBuilder();

            HashSet<DiscordButtonComponent> nbItemButtons = Buttons1Builder();
            if (nbItemButtons.Count > 0)
                followupMessage.AddComponents(nbItemButtons);

            nbItemButtons = Buttons2Builder();
            if (nbItemButtons.Count > 0)
                followupMessage.AddComponents(nbItemButtons);

            DiscordSelectComponent select = SelectBuilder();
            if (select.Options.Count > 1)
                followupMessage.AddComponents(select);

            return followupMessage;
        }

        protected override async Task<bool> InteractionTreatment(ComponentInteractionCreateEventArgs e)
        {
            if (int.TryParse(e.Id, out int nbItemSelected))
            {
                _nbItemSelected = nbItemSelected;

                await UpdateInteractionResponse(e.Interaction);
                return true;
            }

            if (e.Id.Equals("select"))
            {
                int id = Convert.ToInt32(e.Interaction.Data.Values.First());

                Item? item = Bot.Instance.Api.Datacenter.ItemsData.GetItemById(id);
                if (item is not null)
                {
                    await new ItemMessageBuilder(item).UpdateInteractionResponse(e.Interaction);
                    return true;
                }
            }
            else if (e.Id.Equals("breed"))
            {
                if (_breed is not null)
                {
                    await new BreedMessageBuilder(_breed).UpdateInteractionResponse(e.Interaction);
                    return true;
                }
            }

            await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
            return false;
        }
    }
}
