using Cyberia.Api;
using Cyberia.Api.Data.Maps;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus.Map;

public sealed class PaginatedMapAreaMessageBuilder : PaginatedMessageBuilder<MapAreaData>
{
    public const string PacketHeader = "PMA.A";
    public const int PacketVersion = 2;

    public PaginatedMapAreaMessageBuilder(List<MapAreaData> mapAreasData, string search, int selectedPageIndex = 0)
        : base(EmbedCategory.Map, BotTranslations.Embed_Map_Author, BotTranslations.Embed_PaginatedMapArea_Title, mapAreasData, search, selectedPageIndex)
    {

    }

    public static PaginatedMapAreaMessageBuilder? Create(int version, string[] parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 1 &&
            int.TryParse(parameters[0], out var selectedPageIndex))
        {
            var mapAreasData = DofusApi.Datacenter.MapsRepository.GetMapAreasDataByName(parameters[1]).ToList();
            if (mapAreasData.Count > 0)
            {
                return new(mapAreasData, parameters[1], selectedPageIndex);
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
