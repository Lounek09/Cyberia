﻿using Cyberia.Api.DatacenterNS;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public enum MapSearchCategory
    {
        Coordinate,
        MapSubArea,
        MapArea
    }

    public sealed class PaginatedMapMessageBuilder : PaginatedMessageBuilder<Map>
    {
        public const string PACKET_HEADER = "PMA";
        public const int PACKET_VERSION = 1;

        private readonly MapSearchCategory _searchCategory;
        private readonly string _search;

        public PaginatedMapMessageBuilder(List<Map> maps, MapSearchCategory searchCategory, string search, int selectedPageIndex = 0) :
            base(DofusEmbedCategory.Map, "Carte du monde", "Plusieurs maps trouvées :", maps.OrderBy(x => x.Id).ToList(), selectedPageIndex)
        {
            _searchCategory = searchCategory;
            _search = search;
        }

        public static PaginatedMapMessageBuilder? Create(int version, string[] parameters)
        {
            if (version == PACKET_VERSION &&
                parameters.Length > 3 &&
                int.TryParse(parameters[1], out int selectedPageIndex) &&
                Enum.TryParse(parameters[2], true, out MapSearchCategory searchCategory))
            {
                List<Map> maps = new();
                string search = "";
                switch (searchCategory)
                {
                    case MapSearchCategory.Coordinate:
                        if (parameters.Length > 4 &&
                            int.TryParse(parameters[3], out int xCoord) &&
                            int.TryParse(parameters[4], out int yCoord))
                        {
                            maps = Bot.Instance.Api.Datacenter.MapsData.GetMapsByCoordinate(xCoord, yCoord);
                            search = $"{parameters[3]}{InteractionManager.PACKET_PARAMETER_SEPARATOR}{parameters[4]}";
                        }
                        break;
                    case MapSearchCategory.MapSubArea:
                        if (int.TryParse(parameters[3], out int mapSubAreaId))
                        {
                            maps = Bot.Instance.Api.Datacenter.MapsData.GetMapsByMapSubAreaId(mapSubAreaId);
                            search = parameters[3];
                        }
                        break;
                    case MapSearchCategory.MapArea:
                        if (int.TryParse(parameters[3], out int mapAreaId))
                        {
                            maps = Bot.Instance.Api.Datacenter.MapsData.GetMapsByMapAreaId(mapAreaId);
                            search = parameters[3];
                        }
                        break;
                }

                if (maps.Count > 0 && !string.IsNullOrEmpty(search))
                    return new(maps, searchCategory, search, selectedPageIndex);
            }

            return null;
        }

        public static string GetPacket(MapSearchCategory searchCategory, string search, int selectedPageIndex = 0, PaginatedAction action = PaginatedAction.Nothing)
        {
            return InteractionManager.ComponentPacketBuilder(PACKET_HEADER, PACKET_VERSION, (int)action, selectedPageIndex, (int)searchCategory, search);
        }

        protected override IEnumerable<string> GetContent()
        {
            return _data.Select(x => $"- {Formatter.Bold(x.GetCoordinate())} {x.GetMapAreaName()} ({x.Id}) {(x.IsHouse() ? Emojis.HOUSE : "")}");
        }

        protected override DiscordSelectComponent SelectBuilder()
        {
            return MapComponentsBuilder.MapsSelectBuilder(0, _data);
        }

        protected override string PreviousPacketBuilder()
        {
            return GetPacket(_searchCategory, _search, PreviousPageIndex());
        }

        protected override string NextPacketBuilder()
        {
            return GetPacket(_searchCategory, _search, NextPageIndex());
        }
    }
}