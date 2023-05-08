using Cyberia.Api.DatacenterNS;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class MapPaginationMessageBuilder : PaginationMessageBuilder
    {
        private readonly List<Map> _maps;

        public MapPaginationMessageBuilder(string title, List<Map> maps) :
            base(DofusEmbedCategory.Map, "Carte du monde", title)
        {
            _maps = maps.OrderBy(x => x.Id).ToList();
            PopulateContent();
        }

        protected override void PopulateContent()
        {
            foreach (Map map in _maps)
                _content.Add($"- {Formatter.Bold(map.GetCoordinate())} {map.GetMapAreaName()} ({map.Id}) {(map.IsHouse() ? Emojis.HOUSE : "")}");
        }

        protected override DiscordSelectComponent SelectBuilder()
        {
            HashSet<DiscordSelectComponentOption> options = new();
            foreach (Map map in _maps.GetRange(GetStartPageIndex(), GetEndPageIndex()))
                options.Add(new($"{map.GetCoordinate()} ({map.Id}) {(map.IsHouse() ? Emojis.HOUSE : "")}", map.Id.ToString(), map.GetMapAreaName().WithMaxLength(50)));

            return new("select", "Sélectionne une map pour l'afficher", options);
        }

        protected override async Task<bool> InteractionTreatment(ComponentInteractionCreateEventArgs e)
        {
            if (await base.InteractionTreatment(e))
                return true;

            if (e.Id.Equals("select"))
            {
                int id = Convert.ToInt32(e.Interaction.Data.Values.First());

                Map? map = Bot.Instance.Api.Datacenter.MapsData.GetMapById(id);
                if (map is not null)
                {
                    await new MapMessageBuilder(map).UpdateInteractionResponse(e.Interaction);
                    return true;
                }
            }

            await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
            return false;
        }
    }
}
