using Cyberia.Api;
using Cyberia.Api.Data;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus;

public sealed class PaginatedMapAreaMessageBuilder : PaginatedMessageBuilder<MapAreaData>
{
    public const string PACKET_HEADER = "PMA.A";
    public const int PACKET_VERSION = 1;

    private readonly string _search;

    public PaginatedMapAreaMessageBuilder(List<MapAreaData> mapAreasData, string search, int selectedPageIndex = 0)
        : base(DofusEmbedCategory.Map, "Carte du monde", "Plusieurs zones trouvées :", mapAreasData, selectedPageIndex)
    {
        _search = search;
    }

    public static PaginatedMapAreaMessageBuilder? Create(int version, string[] parameters)
    {
        if (version == PACKET_VERSION &&
            parameters.Length > 2 &&
            int.TryParse(parameters[1], out var selectedPageIndex))
        {
            var mapAreasData = DofusApi.Datacenter.MapsData.GetMapAreasDataByName(parameters[2]).ToList();
            if (mapAreasData.Count > 0)
            {
                return new(mapAreasData, parameters[2], selectedPageIndex);
            }
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
