using Cyberia.Api;
using Cyberia.Api.Data.Items;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus.Rune;

public sealed class PaginatedRuneItemMessageBuilder : PaginatedMessageBuilder<ItemData>
{
    public const string PacketHeader = "PR";
    public const int PacketVersion = 1;

    private readonly string _search;
    private readonly int _qte;

    public PaginatedRuneItemMessageBuilder(List<ItemData> itemsData, string search, int qte = 1, int selectedPageIndex = 0)
        : base(EmbedCategory.Inventory, "Items", "Plusieurs objets trouvés :", itemsData, selectedPageIndex)
    {
        _search = search;
        _qte = qte;
    }

    public static PaginatedRuneItemMessageBuilder? Create(int version, string[] parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 3 &&
            int.TryParse(parameters[1], out var selectedPageIndex) &&
            int.TryParse(parameters[3], out var qte))
        {
            var itemsData = DofusApi.Datacenter.ItemsData.GetItemsDataByName(parameters[2]).ToList();
            if (itemsData.Count > 0)
            {
                return new(itemsData, parameters[2], qte, selectedPageIndex);
            }
        }

        return null;
    }

    public static string GetPacket(string search, int qte, int selectedPageIndex = 0, PaginatedAction action = PaginatedAction.None)
    {
        return InteractionManager.ComponentPacketBuilder(PacketHeader, PacketVersion, (int)action, selectedPageIndex, search, qte);
    }

    protected override IEnumerable<string> GetContent()
    {
        return _data.Select(x => $"- Niv.{x.Level} {Formatter.Bold(Formatter.Sanitize(x.Name))} ({x.Id})");
    }

    protected override DiscordSelectComponent SelectBuilder()
    {
        return RuneComponentsBuilder.ItemsSelectBuilder(0, _data, _qte);
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
