using Cyberia.Api;
using Cyberia.Api.Data.Maps;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Formatters;
using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.Entities;

using Microsoft.Extensions.DependencyInjection;

namespace Cyberia.Salamandra.Commands.Dofus.Map;

public sealed class PaginatedMapMessageBuilder : PaginatedMessageBuilder<MapData>
{
    public const string PacketHeader = "PMA";
    public const int PacketVersion = 2;

    private readonly MapSearchCategory _searchCategory;

    public PaginatedMapMessageBuilder(EmbedBuilderService embedBuilderService, List<MapData> mapsData, MapSearchCategory searchCategory, string search, int selectedPageIndex = 0)
        : base(embedBuilderService.CreateEmbedBuilder(EmbedCategory.Map, BotTranslations.Embed_Map_Author), BotTranslations.Embed_PaginatedMap_Title, mapsData, search, selectedPageIndex)
    {
        _searchCategory = searchCategory;
    }

    public static PaginatedMapMessageBuilder? Create(IServiceProvider provider, int version, string[] parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 2 &&
            int.TryParse(parameters[0], out var selectedPageIndex) &&
            Enum.TryParse(parameters[1], true, out MapSearchCategory searchCategory))
        {
            List<MapData> mapsData = [];
            var search = string.Empty;
            switch (searchCategory)
            {
                case MapSearchCategory.Coordinate:
                    if (parameters.Length > 3 &&
                        int.TryParse(parameters[2], out var x) &&
                        int.TryParse(parameters[3], out var y))
                    {
                        mapsData = DofusApi.Datacenter.MapsRepository.GetMapsDataByCoordinate(x, y).ToList();
                        search = $"{parameters[2]}{PacketFormatter.Separator}{parameters[3]}";
                    }
                    break;
                case MapSearchCategory.MapSubArea:
                    if (int.TryParse(parameters[2], out var mapSubAreaId))
                    {
                        mapsData = DofusApi.Datacenter.MapsRepository.GetMapsDataByMapSubAreaId(mapSubAreaId).ToList();
                        search = parameters[2];
                    }
                    break;
                case MapSearchCategory.MapArea:
                    if (int.TryParse(parameters[2], out var mapAreaId))
                    {
                        mapsData = DofusApi.Datacenter.MapsRepository.GetMapsDataByMapAreaId(mapAreaId).ToList();
                        search = parameters[2];
                    }
                    break;
            }

            if (mapsData.Count > 0 && !string.IsNullOrEmpty(search))
            {
                var embedBuilderService = provider.GetRequiredService<EmbedBuilderService>();

                return new(embedBuilderService, mapsData, searchCategory, search, selectedPageIndex);
            }
        }

        return null;
    }

    public static string GetPacket(MapSearchCategory searchCategory, string search, int selectedPageIndex = 0)
    {
        return PacketFormatter.Action(PacketHeader, PacketVersion, selectedPageIndex, (int)searchCategory, search);
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
