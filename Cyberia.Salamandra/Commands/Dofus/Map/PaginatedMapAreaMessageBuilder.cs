﻿using Cyberia.Api.DatacenterNS;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class PaginatedMapAreaMessageBuilder : PaginatedMessageBuilder<MapArea>
    {
        public const string PACKET_HEADER = "PMA.A";
        public const int PACKET_VERSION = 1;

        private readonly string _search;

        public PaginatedMapAreaMessageBuilder(List<MapArea> mapAreas, string search, int selectedPageIndex = 0) :
            base(DofusEmbedCategory.Map, "Carte du monde", "Plusieurs zones trouvées :", mapAreas, selectedPageIndex)
        {
            _search = search;
        }

        public static PaginatedMapAreaMessageBuilder? Create(int version, string[] parameters)
        {
            if (version == PACKET_VERSION &&
                parameters.Length > 2 &&
                int.TryParse(parameters[1], out int selectedPageIndex))
            {
                List<MapArea> mapAreas = Bot.Instance.Api.Datacenter.MapsData.GetMapAreasByName(parameters[2]);
                if (mapAreas.Count > 0)
                    return new(mapAreas, parameters[2], selectedPageIndex);
            }

            return null;
        }

        public static string GetPacket(string search, int selectedPageIndex = 0, PaginatedAction action = PaginatedAction.Nothing)
        {
            return InteractionManager.ComponentPacketBuilder(PACKET_HEADER, PACKET_VERSION, (int)action, selectedPageIndex, search);
        }

        protected override IEnumerable<string> GetContent()
        {
            return _data.Select(x => $"- {Formatter.Bold(x.Name)} ({x.Id})");
        }

        protected override DiscordSelectComponent SelectBuilder()
        {
            return MapComponentsBuilder.MapAreasSelectBuilder(0, _data);
        }

        protected override string PreviousPacketBuilder()
        {
            return GetPacket(_search, PreviousPageIndex());
        }

        protected override string NextPacketBuilder()
        {
            return GetPacket(_search, NextPageIndex());
        }
    }
}