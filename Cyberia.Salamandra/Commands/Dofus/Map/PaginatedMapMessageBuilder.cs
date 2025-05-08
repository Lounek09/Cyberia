using Cyberia.Api.Data;
using Cyberia.Api.Data.Maps;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Formatters;
using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.Entities;

using Microsoft.Extensions.DependencyInjection;

using System.Globalization;

namespace Cyberia.Salamandra.Commands.Dofus.Map;

public sealed class PaginatedMapMessageBuilder : PaginatedMessageBuilder<MapData>
{
    public const string PacketHeader = "PMA";
    public const int PacketVersion = 2;

    private readonly MapSearchCategory _searchCategory;

    public PaginatedMapMessageBuilder(
        IEmbedBuilderService embedBuilderService,
        List<MapData> mapsData,
        MapSearchCategory searchCategory,
        string search,
        CultureInfo? culture,
        int selectedPageIndex = 0)
    : base(
        embedBuilderService.CreateEmbedBuilder(EmbedCategory.Map, Translation.Get<BotTranslations>("Embed.Map.Author", culture), culture),
        Translation.Get<BotTranslations>("Embed.PaginatedMap.Title", culture),
        mapsData,
        search,
        culture,
        selectedPageIndex)
    {
        _searchCategory = searchCategory;
    }

    public static PaginatedMapMessageBuilder? Create(IServiceProvider provider, int version, CultureInfo? culture, params ReadOnlySpan<string> parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 2 &&
            int.TryParse(parameters[0], out var selectedPageIndex) &&
            Enum.TryParse(parameters[1], true, out MapSearchCategory searchCategory))
        {
            var dofusDatacenter = provider.GetRequiredService<DofusDatacenter>();

            List<MapData> mapsData = [];
            var search = string.Empty;
            switch (searchCategory)
            {
                case MapSearchCategory.Coordinate:
                    if (parameters.Length > 3 &&
                        int.TryParse(parameters[2], out var x) &&
                        int.TryParse(parameters[3], out var y))
                    {
                        mapsData = dofusDatacenter.MapsRepository.GetMapsDataByCoordinate(x, y).ToList();
                        search = $"{parameters[2]}{PacketFormatter.Separator}{parameters[3]}";
                    }
                    break;
                case MapSearchCategory.MapSubArea:
                    if (int.TryParse(parameters[2], out var mapSubAreaId))
                    {
                        mapsData = dofusDatacenter.MapsRepository.GetMapsDataByMapSubAreaId(mapSubAreaId).ToList();
                        search = parameters[2];
                    }
                    break;
                case MapSearchCategory.MapArea:
                    if (int.TryParse(parameters[2], out var mapAreaId))
                    {
                        mapsData = dofusDatacenter.MapsRepository.GetMapsDataByMapAreaId(mapAreaId).ToList();
                        search = parameters[2];
                    }
                    break;
            }

            if (mapsData.Count > 0 && !string.IsNullOrEmpty(search))
            {
                var embedBuilderService = provider.GetRequiredService<IEmbedBuilderService>();

                return new(embedBuilderService, mapsData, searchCategory, search, culture, selectedPageIndex);
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
        return _data.Select(x => $"- {Formatter.Bold(x.GetCoordinate())} {x.GetMapAreaName(_culture)} ({x.Id}) {(x.IsHouse() ? Emojis.House(_culture) : string.Empty)}");
    }

    protected override DiscordSelectComponent SelectBuilder()
    {
        return MapComponentsBuilder.MapsSelectBuilder(0, _data, _culture);
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
