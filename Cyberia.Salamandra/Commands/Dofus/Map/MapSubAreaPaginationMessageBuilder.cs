using Cyberia.Api.DatacenterNS;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class MapSubAreaPaginationMessageBuilder : PaginationMessageBuilder
    {
        private readonly List<MapSubArea> _mapSubAreas;

        public MapSubAreaPaginationMessageBuilder(List<MapSubArea> mapSubAreas) :
            base(DofusEmbedCategory.Map, "Carte du monde",  "Plusieurs sous-zones trouvées :")
        {
            _mapSubAreas = mapSubAreas;
            PopulateContent();
        }

        protected override void PopulateContent()
        {
            foreach (MapSubArea mapSubArea in _mapSubAreas)
                _content.Add($"- {Formatter.Bold(mapSubArea.Name)} ({mapSubArea.Id})");
        }

        protected override DiscordSelectComponent SelectBuilder()
        {
            HashSet<DiscordSelectComponentOption> options = new();
            foreach (MapSubArea mapSubArea in _mapSubAreas.GetRange(GetStartPageIndex(), GetEndPageIndex()))
                options.Add(new(mapSubArea.Name.WithMaxLength(100), mapSubArea.Id.ToString(), mapSubArea.Id.ToString()));

            return new("select", "Sélectionne une sous-zone pour afficher ses maps", options);
        }

        protected override async Task<bool> InteractionTreatment(ComponentInteractionCreateEventArgs e)
        {
            if (await base.InteractionTreatment(e))
                return true;

            if (e.Id.Equals("select"))
            {
                int id = Convert.ToInt32(e.Interaction.Data.Values.First());
                MapSubArea? mapSubArea = Bot.Instance.Api.Datacenter.MapsData.GetMapSubAreaById(id);

                if (mapSubArea is not null)
                {
                    List<Map> maps = mapSubArea.GetMaps();
                    if (maps.Count > 0)
                        await new MapPaginationMessageBuilder($"Liste des maps de la sous-zone '{mapSubArea.Name}' :", maps).UpdateInteractionResponse(e.Interaction);
                    else
                        await e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder().WithContent($"La sous-zone {Formatter.Bold(mapSubArea.Name)} ne contient aucune map"));

                    return true;
                }
            }

            await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
            return false;
        }
    }
}
