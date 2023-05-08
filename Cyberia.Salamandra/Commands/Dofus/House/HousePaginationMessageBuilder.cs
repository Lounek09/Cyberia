using Cyberia.Api.DatacenterNS;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class HousePaginationMessageBuilder : PaginationMessageBuilder
    {
        private readonly List<House> _houses;

        public HousePaginationMessageBuilder(List<House> houses) :
            base(DofusEmbedCategory.Houses, "Agence immobilière", "Plusieurs maisons trouvées :")
        {
            _houses = houses;
            PopulateContent();
        }

        protected override void PopulateContent()
        {
            foreach (House house in _houses)
                _content.Add($"- {Formatter.Bold(house.Name)}{(string.IsNullOrEmpty(house.GetCoordinate()) ? "" : $" {house.GetCoordinate()}")} ({house.Id})");
        }

        protected override DiscordSelectComponent SelectBuilder()
        {
            HashSet<DiscordSelectComponentOption> options = new();
            foreach (House house in _houses.GetRange(GetStartPageIndex(), GetEndPageIndex()))
                options.Add(new($"{house.Name}{(string.IsNullOrEmpty(house.GetCoordinate()) ? "" : $" {house.GetCoordinate()}")}".WithMaxLength(100), house.Id.ToString(), house.Id.ToString()));

            return new("select", "Sélectionne une maison pour l'afficher", options);
        }

        protected override async Task<bool> InteractionTreatment(ComponentInteractionCreateEventArgs e)
        {
            if (await base.InteractionTreatment(e))
                return true;

            if (e.Id.Equals("select"))
            {
                int id = Convert.ToInt32(e.Interaction.Data.Values.First());

                House? house = Bot.Instance.Api.Datacenter.HousesData.GetHouseById(id);
                if (house is not null)
                {
                    await new HouseMessageBuilder(house).UpdateInteractionResponse(e.Interaction);
                    return true;
                }
            }

            await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
            return false;
        }
    }
}
