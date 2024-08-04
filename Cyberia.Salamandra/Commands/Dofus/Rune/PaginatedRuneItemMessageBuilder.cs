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
    public const int PacketVersion = 2;

    private readonly string _search;
    private readonly int _quantity;

    public PaginatedRuneItemMessageBuilder(List<ItemData> itemsData, string search, int quantity = 1, int selectedPageIndex = 0)
        : base(EmbedCategory.Inventory, BotTranslations.Embed_Item_Author, BotTranslations.Embed_PaginatedItem_Title, itemsData, selectedPageIndex)
    {
        _search = search;
        _quantity = quantity;
    }

    public static PaginatedRuneItemMessageBuilder? Create(int version, string[] parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 3 &&
            int.TryParse(parameters[1], out var selectedPageIndex) &&
            int.TryParse(parameters[3], out var quantity))
        {
            var itemsData = DofusApi.Datacenter.ItemsRepository.GetItemsDataByName(parameters[2]).ToList();
            if (itemsData.Count > 0)
            {
                return new(itemsData, parameters[2], quantity, selectedPageIndex);
            }
        }

        return null;
    }

    public static string GetPacket(string search, int quantity, int selectedPageIndex = 0)
    {
        return InteractionManager.ComponentPacketBuilder(PacketHeader, PacketVersion, selectedPageIndex, search, quantity);
    }

    protected override IEnumerable<string> GetContent()
    {
        return _data.Select(x => $"- {BotTranslations.ShortLevel}{x.Level} {Formatter.Bold(Formatter.Sanitize(x.Name))} ({x.Id})");
    }

    protected override DiscordSelectComponent SelectBuilder()
    {
        return RuneComponentsBuilder.ItemsSelectBuilder(0, _data, _quantity);
    }

    protected override string PreviousPacketBuilder()
    {
        return GetPacket(_search, _quantity, PreviousPageIndex());
    }

    protected override string NextPacketBuilder()
    {
        return GetPacket(_search, _quantity, NextPageIndex());
    }
}
