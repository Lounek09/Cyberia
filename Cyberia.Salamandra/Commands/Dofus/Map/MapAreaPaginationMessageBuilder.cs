using Cyberia.Api.DatacenterNS;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class MapAreaPaginationMessageBuilder : PaginationMessageBuilder
    {
        private readonly List<MapArea> _mapAreas;

        public MapAreaPaginationMessageBuilder(List<MapArea> mapAreas) :
            base(DofusEmbedCategory.Map, "Carte du monde", "Plusieurs zones trouvées :")
        {
            _mapAreas = mapAreas;
            PopulateContent();
        }

        protected override void PopulateContent()
        {
            foreach (MapArea mapArea in _mapAreas)
                _content.Add($"- {Formatter.Bold(mapArea.Name)} ({mapArea.Id})");
        }

        protected override DiscordSelectComponent SelectBuilder()
        {
            HashSet<DiscordSelectComponentOption> options = new();
            foreach (MapArea mapArea in _mapAreas.GetRange(GetStartPageIndex(), Math.Min(GetEndPageIndex(), MAX_PER_PAGE)))
                options.Add(new(mapArea.Name.WithMaxLength(100), mapArea.Id.ToString(), mapArea.Id.ToString()));

            return new("select", "Sélectionne une zone pour afficher ses maps", options);
        }

        protected override async Task<bool> InteractionTreatment(ComponentInteractionCreateEventArgs e)
        {
            if (await base.InteractionTreatment(e))
                return true;

            if (e.Id.Equals("select"))
            {
                int id = Convert.ToInt32(e.Interaction.Data.Values.First());

                MapArea? mapArea = Bot.Instance.Api.Datacenter.MapsData.GetMapAreaById(id);
                if (mapArea is not null)
                {
                    List<Map> maps = mapArea.GetMaps();

                    if (maps.Count > 0)
                        await new MapPaginationMessageBuilder($"Liste des maps de la zone '{mapArea.Name}' :", maps).UpdateInteractionResponse(e.Interaction);
                    else
                        await e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder().WithContent($"La zone {Formatter.Bold(mapArea.Name)} ne contient aucune map"));

                    return true;
                }
            }

            await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
            return false;
        }
    }
}
