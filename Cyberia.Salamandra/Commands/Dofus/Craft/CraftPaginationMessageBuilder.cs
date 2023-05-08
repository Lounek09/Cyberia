using Cyberia.Api.DatacenterNS;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class CraftPaginationMessageBuilder : PaginationMessageBuilder
    {
        private readonly List<Craft> _crafts;
        private readonly int _qte;

        public CraftPaginationMessageBuilder(List<Craft> crafts, int qte) :
            base(DofusEmbedCategory.Jobs, "Calculateur de crafts", "Plusieurs crafts trouvés :")
        {
            _crafts = crafts;
            _qte = qte;
            PopulateContent();
        }

        protected override void PopulateContent()
        {
            foreach (Craft craft in _crafts)
            {
                Item? item = craft.GetItem();
                if (item is not null)
                    _content.Add($"- Niv.{item.Level} {Formatter.Bold(item.Name.SanitizeMarkDown())} ({craft.Id})");
            }
        }

        protected override DiscordSelectComponent SelectBuilder()
        {
            HashSet<DiscordSelectComponentOption> options = new();
            foreach (Craft craft in _crafts.GetRange(GetStartPageIndex(), GetEndPageIndex()))
            {
                Item? item = craft.GetItem();
                if (item is not null)
                    options.Add(new(item.Name.WithMaxLength(100), craft.Id.ToString(), craft.Id.ToString()));
            }

            return new("select", "Sélectionne un item pour calculer son craft", options);
        }

        protected override async Task<bool> InteractionTreatment(ComponentInteractionCreateEventArgs e)
        {
            if (await base.InteractionTreatment(e))
                return true;

            if (e.Id.Equals("select"))
            {
                int id = Convert.ToInt32(e.Interaction.Data.Values.First());

                Craft? craft = Bot.Instance.Api.Datacenter.CraftsData.GetCraftById(id);
                if (craft is not null)
                {
                    await new CraftMessageBuilder(craft, _qte).UpdateInteractionResponse(e.Interaction);
                    return true;
                }
            }

            await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
            return false;
        }
    }
}
