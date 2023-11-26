using Cyberia.Api;
using Cyberia.Api.Data;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public enum HouseSearchCategory
    {
        Name,
        Coordinate,
        MapSubArea,
        MapArea
    }

    public sealed class PaginatedHouseMessageBuilder : PaginatedMessageBuilder<HouseData>
    {
        public const string PACKET_HEADER = "PH";
        public const int PACKET_VERSION = 1;

        private readonly HouseSearchCategory _searchCategory;
        private readonly string _search;

        public PaginatedHouseMessageBuilder(List<HouseData> housesData, HouseSearchCategory searchCategory, string search, int selectedPageIndex = 0) :
            base(DofusEmbedCategory.Houses, "Agence immobilière", "Plusieurs maisons trouvées :", housesData, selectedPageIndex)
        {
            _searchCategory = searchCategory;
            _search = search;
        }

        public static PaginatedHouseMessageBuilder? Create(int version, string[] parameters)
        {
            if (version == PACKET_VERSION &&
                parameters.Length > 3 &&
                int.TryParse(parameters[1], out int selectedPageIndex) &&
                Enum.TryParse(parameters[2], true, out HouseSearchCategory searchCategory))
            {
                List<HouseData> housesData = [];
                string search = "";
                switch (searchCategory)
                {
                    case HouseSearchCategory.Name:
                        housesData = DofusApi.Datacenter.HousesData.GetHousesDataByName(parameters[3]).ToList();
                        search = parameters[3];
                        break;
                    case HouseSearchCategory.Coordinate:
                        if (parameters.Length > 4 &&
                            int.TryParse(parameters[3], out int xCoord) &&
                            int.TryParse(parameters[4], out int yCoord))
                        {
                            housesData = DofusApi.Datacenter.HousesData.GetHousesDataByCoordinate(xCoord, yCoord).ToList();
                            search = $"{parameters[3]}{InteractionManager.PACKET_PARAMETER_SEPARATOR}{parameters[4]}";
                        }
                        break;
                    case HouseSearchCategory.MapSubArea:
                        if (int.TryParse(parameters[3], out int mapSubAreaId))
                        {
                            housesData = DofusApi.Datacenter.HousesData.GetHousesDataByMapSubAreaId(mapSubAreaId).ToList();
                            search = parameters[3];
                        }
                        break;
                    case HouseSearchCategory.MapArea:
                        if (int.TryParse(parameters[3], out int mapAreaId))
                        {
                            housesData = DofusApi.Datacenter.HousesData.GetHousesDataByMapAreaId(mapAreaId).ToList();
                            search = parameters[3];
                        }
                        break;
                }

                if (housesData.Count > 0 && !string.IsNullOrEmpty(search))
                {
                    return new(housesData, searchCategory, search, selectedPageIndex);
                }
            }

            return null;
        }

        public static string GetPacket(HouseSearchCategory searchCategory, string search, int selectedPageIndex = 0, PaginatedAction action = PaginatedAction.Nothing)
        {
            return InteractionManager.ComponentPacketBuilder(PACKET_HEADER, PACKET_VERSION, (int)action, selectedPageIndex, (int)searchCategory, search);
        }

        protected override IEnumerable<string> GetContent()
        {
            return _data.Select(x => $"- {Formatter.Bold(x.Name)}{(string.IsNullOrEmpty(x.GetCoordinate()) ? "" : $" {x.GetCoordinate()}")} ({x.Id})");
        }

        protected override DiscordSelectComponent SelectBuilder()
        {
            return HouseComponentsBuilder.HousesSelectBuilder(0, _data);
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
