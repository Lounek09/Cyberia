using Cyberia.Api;
using Cyberia.Api.Data.Incarnations;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus;

public sealed class PaginatedIncarnationMessageBuilder : PaginatedMessageBuilder<IncarnationData>
{
    public const string PACKET_HEADER = "PINCA";
    public const int PACKET_VERSION = 1;

    private readonly string _search;

    public PaginatedIncarnationMessageBuilder(List<IncarnationData> incarnationsData, string search, int selectedPageIndex = 0)
        : base(DofusEmbedCategory.Inventory, "Incarnations", "Plusieurs incarnations trouvés :", incarnationsData, selectedPageIndex)
    {
        _search = search;
    }

    public static PaginatedIncarnationMessageBuilder? Create(int version, string[] parameters)
    {
        if (version == PACKET_VERSION &&
            parameters.Length > 2 &&
            int.TryParse(parameters[1], out var selectedPageIndex))
        {
            var incarnationsData = DofusApi.Datacenter.IncarnationsData.GetIncarnationsDataByName(parameters[2]).ToList();
            if (incarnationsData.Count > 0)
            {
                return new(incarnationsData, parameters[2], selectedPageIndex);
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
        return _data.Select(x => $"- {Formatter.Bold(Formatter.Sanitize(x.Name))} ({x.Id})");
    }

    protected override DiscordSelectComponent SelectBuilder()
    {
        return IncarnationComponentsBuilder.IncarnationsSelectBuilder(0, _data);
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
