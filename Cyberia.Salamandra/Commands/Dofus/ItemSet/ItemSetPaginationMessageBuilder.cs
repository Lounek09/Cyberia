using Cyberia.Api.DatacenterNS;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class ItemSetPaginationMessageBuilder : PaginationMessageBuilder
    {
        private readonly List<ItemSet> _itemSets;

        public ItemSetPaginationMessageBuilder(List<ItemSet> itemSets) :
            base(DofusEmbedCategory.Inventory, "Items", "Plusieurs panoplies trouvées :")
        {
            _itemSets = itemSets;
            PopulateContent();
        }

        protected override void PopulateContent()
        {
            foreach (ItemSet itemSet in _itemSets)
                _content.Add($"- Niv.{itemSet.GetLevel()} {Formatter.Bold(itemSet.Name)} ({itemSet.Id})");
        }

        protected override DiscordSelectComponent SelectBuilder()
        {
            HashSet<DiscordSelectComponentOption> options = new();
            foreach (ItemSet itemSet in _itemSets.GetRange(GetStartPageIndex(), GetEndPageIndex()))
                options.Add(new(itemSet.Name.WithMaxLength(100), itemSet.Id.ToString(), itemSet.Id.ToString()));

            return new("select", "Sélectionne une panoplie pour l'afficher", options);
        }

        protected override async Task<bool> InteractionTreatment(ComponentInteractionCreateEventArgs e)
        {
            if (await base.InteractionTreatment(e))
                return true;

            if (e.Id.Equals("select"))
            {
                int id = Convert.ToInt32(e.Interaction.Data.Values.First());

                ItemSet? itemSet = Bot.Instance.Api.Datacenter.ItemSetsData.GetItemSetById(id);
                if (itemSet is not null)
                {
                    await new ItemSetMessageBuilder(itemSet).UpdateInteractionResponse(e.Interaction);
                    return true;
                }
            }

            await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
            return false;
        }
    }
}
