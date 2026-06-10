using Cyberia.Api.Data;
using Cyberia.Api.Data.Houses;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Formatters;
using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.Entities;

using Microsoft.Extensions.DependencyInjection;

using System.Globalization;

namespace Cyberia.Salamandra.Commands.Dofus.House;

public sealed class PaginatedHouseMessageBuilder : PaginatedMessageBuilder<HouseData>
{
    public const string PacketHeader = "PH";
    public const int PacketVersion = 2;

    private readonly HouseSearchCategory _searchCategory;

    public PaginatedHouseMessageBuilder(
        IEmbedBuilderService embedBuilderService,
        List<HouseData> housesData,
        HouseSearchCategory searchCategory,
        string search,
        CultureInfo? culture,
        int selectedPageIndex = 0)
    : base(
        embedBuilderService.CreateBaseEmbedBuilder(EmbedCategory.Houses, Translation.Get<BotTranslations>("Embed.House.Author", culture), culture),
        Translation.Get<BotTranslations>("Embed.PaginatedHouse.Title", culture),
        housesData,
        search,
        culture,
        selectedPageIndex)
    {
        _searchCategory = searchCategory;
    }

    public static PaginatedHouseMessageBuilder? Create(IServiceProvider provider, int version, CultureInfo? culture, params ReadOnlySpan<string> parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 2 &&
            int.TryParse(parameters[0], out var selectedPageIndex) &&
            Enum.TryParse(parameters[1], true, out HouseSearchCategory searchCategory))
        {
            var dofusDatacenter = provider.GetRequiredService<DofusDatacenter>();

            List<HouseData> housesData = [];
            var search = string.Empty;
            switch (searchCategory)
            {
                case HouseSearchCategory.Name:
                    housesData = dofusDatacenter.HousesRepository.GetHousesDataByName(parameters[2], culture).ToList();
                    search = parameters[2];
                    break;
                case HouseSearchCategory.Coordinate:
                    if (parameters.Length > 3 &&
                        int.TryParse(parameters[2], out var x) &&
                        int.TryParse(parameters[3], out var y))
                    {
                        housesData = dofusDatacenter.HousesRepository.GetHousesDataByCoordinate(x, y).ToList();
                        search = $"{parameters[2]}{PacketFormatter.Separator}{parameters[3]}";
                    }
                    break;
                case HouseSearchCategory.MapSubArea:
                    if (int.TryParse(parameters[2], out var mapSubAreaId))
                    {
                        housesData = dofusDatacenter.HousesRepository.GetHousesDataByMapSubAreaId(mapSubAreaId).ToList();
                        search = parameters[2];
                    }
                    break;
                case HouseSearchCategory.MapArea:
                    if (int.TryParse(parameters[2], out var mapAreaId))
                    {
                        housesData = dofusDatacenter.HousesRepository.GetHousesDataByMapAreaId(mapAreaId).ToList();
                        search = parameters[2];
                    }
                    break;
            }

            if (housesData.Count > 0 && !string.IsNullOrEmpty(search))
            {
                var embedBuilderService = provider.GetRequiredService<IEmbedBuilderService>();

                return new(embedBuilderService, housesData, searchCategory, search, culture, selectedPageIndex);
            }
        }

        return null;
    }

    public static string GetPacket(HouseSearchCategory searchCategory, string search, int selectedPageIndex = 0)
    {
        return PacketFormatter.Action(PacketHeader, PacketVersion, selectedPageIndex, (int)searchCategory, search);
    }

    protected override IEnumerable<string> GetContent()
    {
        return _data.Select(x => $"- {Formatter.Bold(x.Name.ToString(_culture))} {x.GetCoordinate()} ({x.Id})");
    }

    protected override DiscordSelectComponent SelectBuilder()
    {
        return HouseComponentsBuilder.HousesSelectBuilder(0, _data, _culture);
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
