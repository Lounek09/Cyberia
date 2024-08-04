using Cyberia.Api;
using Cyberia.Api.Data.Maps;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus.Map;

public sealed class PaginatedMapSubAreaMessageBuilder : PaginatedMessageBuilder<MapSubAreaData>
{
    public const string PacketHeader = "PMA.SA";
    public const int PacketVersion = 2;

    public PaginatedMapSubAreaMessageBuilder(List<MapSubAreaData> mapSubAreasData, string search, int selectedPageIndex = 0)
        : base(EmbedCategory.Map, BotTranslations.Embed_Map_Author, BotTranslations.Embed_PaginatedMapSubArea_Title, mapSubAreasData, search, selectedPageIndex)
    {

    }

    public static PaginatedMapSubAreaMessageBuilder? Create(int version, string[] parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 1 &&
            int.TryParse(parameters[0], out var selectedPageIndex))
        {
            var mapSubAreasData = DofusApi.Datacenter.MapsRepository.GetMapSubAreasDataByName(parameters[1]).ToList();
            if (mapSubAreasData.Count > 0)
            {
                return new(mapSubAreasData, parameters[1], selectedPageIndex);
            }
        }

        return null;
    }

    public static string GetPacket(string search, int selectedPageIndex = 0)
    {
        return InteractionManager.ComponentPacketBuilder(PacketHeader, PacketVersion, selectedPageIndex, search);
    }

    protected override IEnumerable<string> GetContent()
    {
        return _data.Select(x => $"- {Formatter.Bold(x.Name)} ({x.Id})");
    }

    protected override DiscordSelectComponent SelectBuilder()
    {
        return MapComponentsBuilder.MapSubAreasSelectBuilder(0, _data);
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
