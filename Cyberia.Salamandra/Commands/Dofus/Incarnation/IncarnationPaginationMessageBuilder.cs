using Cyberia.Api.DatacenterNS;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class IncarnationPaginationMessageBuilder : PaginationMessageBuilder
    {
        private readonly List<Incarnation> _incarnations;

        public IncarnationPaginationMessageBuilder(List<Incarnation> incarnations) :
            base(DofusEmbedCategory.Inventory, "Incarnations", "Plusieurs incarnations trouvés :")
        {
            _incarnations = incarnations;
            PopulateContent();
        }

        protected override void PopulateContent()
        {
            foreach (Incarnation incarnation in _incarnations)
                _content.Add($"- {Formatter.Bold(incarnation.Name.SanitizeMarkDown())} ({incarnation.Id})");
        }

        protected override DiscordSelectComponent SelectBuilder()
        {
            HashSet<DiscordSelectComponentOption> options = new();
            foreach (Incarnation incarnation in _incarnations.GetRange(GetStartPageIndex(), GetEndPageIndex()))
                options.Add(new(incarnation.Name.WithMaxLength(100), incarnation.Id.ToString(), incarnation.Id.ToString()));

            return new("select", "Sélectionne une incarnation pour l'afficher", options);
        }

        protected override async Task<bool> InteractionTreatment(ComponentInteractionCreateEventArgs e)
        {
            if (await base.InteractionTreatment(e))
                return true;

            if (e.Id.Equals("select"))
            {
                int id = Convert.ToInt32(e.Interaction.Data.Values.First());

                Incarnation? incarnation = Bot.Instance.Api.Datacenter.IncarnationsData.GetIncarnationById(id);
                if (incarnation is not null)
                {
                    await new IncarnationMessageBuilder(incarnation).UpdateInteractionResponse(e.Interaction);
                    return true;
                }
            }

            await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
            return false;
        }
    }
}
