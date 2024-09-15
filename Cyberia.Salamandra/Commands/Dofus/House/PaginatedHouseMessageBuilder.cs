using Cyberia.Api;
using Cyberia.Api.Data.Houses;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus.House;

public sealed class PaginatedHouseMessageBuilder : PaginatedMessageBuilder<HouseData>
{
    public const string PacketHeader = "PH";
    public const int PacketVersion = 2;

    private readonly HouseSearchCategory _searchCategory;

    public PaginatedHouseMessageBuilder(List<HouseData> housesData, HouseSearchCategory searchCategory, string search, int selectedPageIndex = 0)
        : base(EmbedCategory.Houses, BotTranslations.Embed_House_Author, BotTranslations.Embed_PaginatedHouse_Title, housesData, search, selectedPageIndex)
    {
        _searchCategory = searchCategory;
    }

    public static PaginatedHouseMessageBuilder? Create(IServiceProvider _, int version, string[] parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 2 &&
            int.TryParse(parameters[0], out var selectedPageIndex) &&
            Enum.TryParse(parameters[1], true, out HouseSearchCategory searchCategory))
        {
            List<HouseData> housesData = [];
            var search = string.Empty;
            switch (searchCategory)
            {
                case HouseSearchCategory.Name:
                    housesData = DofusApi.Datacenter.HousesRepository.GetHousesDataByName(parameters[2]).ToList();
                    search = parameters[2];
                    break;
                case HouseSearchCategory.Coordinate:
                    if (parameters.Length > 3 &&
                        int.TryParse(parameters[2], out var x) &&
                        int.TryParse(parameters[3], out var y))
                    {
                        housesData = DofusApi.Datacenter.HousesRepository.GetHousesDataByCoordinate(x, y).ToList();
                        search = $"{parameters[2]}{PacketManager.ParameterSeparator}{parameters[3]}";
                    }
                    break;
                case HouseSearchCategory.MapSubArea:
                    if (int.TryParse(parameters[2], out var mapSubAreaId))
                    {
                        housesData = DofusApi.Datacenter.HousesRepository.GetHousesDataByMapSubAreaId(mapSubAreaId).ToList();
                        search = parameters[2];
                    }
                    break;
                case HouseSearchCategory.MapArea:
                    if (int.TryParse(parameters[2], out var mapAreaId))
                    {
                        housesData = DofusApi.Datacenter.HousesRepository.GetHousesDataByMapAreaId(mapAreaId).ToList();
                        search = parameters[2];
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

    public static string GetPacket(HouseSearchCategory searchCategory, string search, int selectedPageIndex = 0)
    {
        return PacketManager.ComponentBuilder(PacketHeader, PacketVersion, selectedPageIndex, (int)searchCategory, search);
    }

    protected override IEnumerable<string> GetContent()
    {
        return _data.Select(x => $"- {Formatter.Bold(x.Name)} {x.GetCoordinate()} ({x.Id})");
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
