using Cyberia.Api;
using Cyberia.Api.Data.Maps;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus;

public enum MapSearchCategory
{
    Coordinate,
    MapSubArea,
    MapArea
}

public sealed class PaginatedMapMessageBuilder : PaginatedMessageBuilder<MapData>
{
    public const string PacketHeader = "PMA";
    public const int PacketVersion = 1;

    private readonly MapSearchCategory _searchCategory;
    private readonly string _search;

    public PaginatedMapMessageBuilder(List<MapData> mapsData, MapSearchCategory searchCategory, string search, int selectedPageIndex = 0)
        : base(EmbedCategory.Map, "Carte du monde", "Plusieurs maps trouvées :", mapsData.OrderBy(x => x.Id).ToList(), selectedPageIndex)
    {
        _searchCategory = searchCategory;
        _search = search;
    }

    public static PaginatedMapMessageBuilder? Create(int version, string[] parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 3 &&
            int.TryParse(parameters[1], out var selectedPageIndex) &&
            Enum.TryParse(parameters[2], true, out MapSearchCategory searchCategory))
        {
            List<MapData> mapsData = [];
            var search = string.Empty;
            switch (searchCategory)
            {
                case MapSearchCategory.Coordinate:
                    if (parameters.Length > 4 &&
                        int.TryParse(parameters[3], out var xCoord) &&
                        int.TryParse(parameters[4], out var yCoord))
                    {
                        mapsData = DofusApi.Datacenter.MapsData.GetMapsDataByCoordinate(xCoord, yCoord).ToList();
                        search = $"{parameters[3]}{InteractionManager.PacketParameterSeparator}{parameters[4]}";
                    }
                    break;
                case MapSearchCategory.MapSubArea:
                    if (int.TryParse(parameters[3], out var mapSubAreaId))
                    {
                        mapsData = DofusApi.Datacenter.MapsData.GetMapsDataByMapSubAreaId(mapSubAreaId).ToList();
                        search = parameters[3];
                    }
                    break;
                case MapSearchCategory.MapArea:
                    if (int.TryParse(parameters[3], out var mapAreaId))
                    {
                        mapsData = DofusApi.Datacenter.MapsData.GetMapsDataByMapAreaId(mapAreaId).ToList();
                        search = parameters[3];
                    }
                    break;
            }

            if (mapsData.Count > 0 && !string.IsNullOrEmpty(search))
            {
                return new(mapsData, searchCategory, search, selectedPageIndex);
            }
        }

        return null;
    }

    public static string GetPacket(MapSearchCategory searchCategory, string search, int selectedPageIndex = 0, PaginatedAction action = PaginatedAction.None)
    {
        return InteractionManager.ComponentPacketBuilder(PacketHeader, PacketVersion, (int)action, selectedPageIndex, (int)searchCategory, search);
    }

    protected override IEnumerable<string> GetContent()
    {
        return _data.Select(x => $"- {Formatter.Bold(x.GetCoordinate())} {x.GetMapAreaName()} ({x.Id}) {(x.IsHouse() ? Emojis.House : string.Empty)}");
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
