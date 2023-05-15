using Cyberia.Api.DatacenterNS;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class MapMessageBuilder : CustomMessageBuilder
    {
        private readonly Map _map;
        private readonly MapSubArea? _mapSubArea;
        private readonly MapArea? _mapArea;
        private readonly House? _house;

        public MapMessageBuilder(Map map) :
            base()
        {
            _map = map;
            _mapSubArea = _map.GetMapSubArea();
            _mapArea = _mapSubArea?.GetMapArea();
            _house = _map.GetHouse();
        }

        protected override Task<DiscordEmbedBuilder> EmbedBuilder()
        {
            DiscordEmbedBuilder embed = EmbedManager.BuildDofusEmbed(DofusEmbedCategory.Map, "Carte du monde")
                .WithTitle($"{_map.GetCoordinate()} ({_map.Id})")
                .WithDescription(_map.GetMapAreaName())
                .WithImageUrl(_map.GetImagePath());

            return Task.FromResult(embed);
        }

        private HashSet<DiscordButtonComponent> ButtonsBuilder()
        {
            HashSet<DiscordButtonComponent> components = new();

            List<Map> maps = Bot.Instance.Api.Datacenter.MapsData.GetMapsByCoordinate(_map.XCoord, _map.YCoord);
            if (maps.Count > 1)
                components.Add(new(ButtonStyle.Primary, "position", "Position"));

            if (_mapSubArea is not null)
                components.Add(new(ButtonStyle.Primary, "mapSubArea", "Sous-zone"));

            if (_mapArea is not null)
                components.Add(new(ButtonStyle.Primary, "mapArea", "Zone"));

            if (_house is not null)
                components.Add(new(ButtonStyle.Primary, "house", "Maison"));

            return components;
        }

        protected override async Task<DiscordInteractionResponseBuilder> InteractionResponseBuilder()
        {
            DiscordInteractionResponseBuilder response = await base.InteractionResponseBuilder();

            HashSet<DiscordButtonComponent> components = ButtonsBuilder();
            if (components.Count > 0)
                response.AddComponents(components);

            return response;
        }

        protected override async Task<DiscordFollowupMessageBuilder> FollowupMessageBuilder()
        {
            DiscordFollowupMessageBuilder followupMessage = await base.FollowupMessageBuilder();

            HashSet<DiscordButtonComponent> components = ButtonsBuilder();
            if (components.Count > 0)
                followupMessage.AddComponents(components);

            return followupMessage;
        }

        protected override async Task<bool> InteractionTreatment(ComponentInteractionCreateEventArgs e)
        {
            switch (e.Id)
            {
                case "position":
                    List<Map> maps = Bot.Instance.Api.Datacenter.MapsData.GetMapsByCoordinate(_map.XCoord, _map.YCoord);
                    if (maps.Count > 0)
                    {
                        await new MapPaginationMessageBuilder($"Plusieurs maps trouvés en [{_map.XCoord}, {_map.YCoord}] :", maps).UpdateInteractionResponse(e.Interaction);
                        return true;
                    }
                    break;
                case "mapSubArea":
                    if (_mapSubArea is not null)
                    {
                        await new MapPaginationMessageBuilder($"Liste des maps de la sous-zone '{_mapSubArea.Name}' :", _mapSubArea.GetMaps()).UpdateInteractionResponse(e.Interaction);
                        return true;
                    }
                    break;
                case "mapArea":
                    if (_mapArea is not null)
                    {
                        await new MapPaginationMessageBuilder($"Liste des maps de la zone '{_mapArea.Name}' :", _mapArea.GetMaps()).UpdateInteractionResponse(e.Interaction);
                        return true;
                    }
                    break;
                case "house":
                    if (_house is not null)
                    {
                        await new HouseMessageBuilder(_house).UpdateInteractionResponse(e.Interaction);
                        return true;
                    }
                    break;
            }

            await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
            return false;
        }
    }
}
