using Cyberia.Api;
using Cyberia.Api.Data.Maps;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus;

public sealed class PaginatedMapSubAreaMessageBuilder : PaginatedMessageBuilder<MapSubAreaData>
{
    public const string PacketHeader = "PMA.SA";
    public const int PacketVersion = 1;

    private readonly string _search;

    public PaginatedMapSubAreaMessageBuilder(List<MapSubAreaData> mapSubAreasData, string search, int selectedPageIndex = 0)
        : base(EmbedCategory.Map, "Carte du monde", "Plusieurs sous-zones trouvées :", mapSubAreasData, selectedPageIndex)
    {
        _search = search;
    }

    public static PaginatedMapSubAreaMessageBuilder? Create(int version, string[] parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 2 &&
            int.TryParse(parameters[1], out var selectedPageIndex))
        {
            var mapSubAreasData = DofusApi.Datacenter.MapsData.GetMapSubAreasDataByName(parameters[2]).ToList();
            if (mapSubAreasData.Count > 0)
            {
                return new(mapSubAreasData, parameters[2], selectedPageIndex);
            }
        }

        return null;
    }

    public static string GetPacket(string search, int selectedPageIndex = 0, PaginatedAction action = PaginatedAction.None)
    {
        return InteractionManager.ComponentPacketBuilder(PacketHeader, PacketVersion, (int)action, selectedPageIndex, search);
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
