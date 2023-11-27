using Cyberia.Api;
using Cyberia.Api.Data;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus;

public sealed class PaginatedCraftMessageBuilder : PaginatedMessageBuilder<CraftData>
{
    public const string PACKET_HEADER = "PC";
    public const int PACKET_VERSION = 1;

    private readonly string _search;
    private readonly int _qte;

    public PaginatedCraftMessageBuilder(List<CraftData> craftsData, string search, int qte = 1, int selectedPageIndex = 0)
        : base(DofusEmbedCategory.Jobs, "Calculateur de crafts", "Plusieurs crafts trouvés :", craftsData, selectedPageIndex)
    {
        _search = search;
        _qte = qte;
    }

    public static PaginatedCraftMessageBuilder? Create(int version, string[] parameters)
    {
        if (version == PACKET_VERSION &&
            parameters.Length > 3 &&
            int.TryParse(parameters[1], out var selectedPageIndex) &&
            int.TryParse(parameters[3], out var qte))
        {
            var craftsData = DofusApi.Datacenter.CraftsData.GetCraftsDataByItemName(parameters[2]).ToList();
            if (craftsData.Count > 0)
            {
                return new(craftsData, parameters[2], qte, selectedPageIndex);
            }
        }

        return null;
    }

    public static string GetPacket(string search, int qte, int selectedPageIndex = 0, PaginatedAction action = PaginatedAction.Nothing)
    {
        return InteractionManager.ComponentPacketBuilder(PACKET_HEADER, PACKET_VERSION, (int)action, selectedPageIndex, search, qte);
    }

    protected override IEnumerable<string> GetContent()
    {
        foreach (var craftData in _data)
        {
            var itemData = craftData.GetItemData();
            if (itemData is not null)
            {
                yield return $"- Niv.{itemData.Level} {Formatter.Bold(Formatter.Sanitize(itemData.Name))} ({craftData.Id})";
            }
        }
    }

    protected override DiscordSelectComponent SelectBuilder()
    {
        return CraftComponentsBuilder.CraftsSelectBuilder(0, _data, _qte);
    }

    protected override string PreviousPacketBuilder()
    {
        return GetPacket(_search, _qte, PreviousPageIndex());
    }

    protected override string NextPacketBuilder()
    {
        return GetPacket(_search, _qte, NextPageIndex());
    }
}
