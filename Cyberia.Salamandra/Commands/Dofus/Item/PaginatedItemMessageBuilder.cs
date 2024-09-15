using Cyberia.Api;
using Cyberia.Api.Data.Items;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus.Item;

public sealed class PaginatedItemMessageBuilder : PaginatedMessageBuilder<ItemData>
{
    public const string PacketHeader = "PI";
    public const int PacketVersion = 2;

    public PaginatedItemMessageBuilder(List<ItemData> itemsData, string search, int selectedPageIndex = 0)
        : base(EmbedCategory.Inventory, BotTranslations.Embed_Item_Author, BotTranslations.Embed_PaginatedItem_Title, itemsData, search, selectedPageIndex)
    {

    }

    public static PaginatedItemMessageBuilder? Create(IServiceProvider _, int version, string[] parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 1 &&
            int.TryParse(parameters[0], out var selectedPageIndex))
        {
            var itemsData = DofusApi.Datacenter.ItemsRepository.GetItemsDataByName(parameters[1]).ToList();
            if (itemsData.Count > 0)
            {
                return new(itemsData, parameters[1], selectedPageIndex);
            }
        }

        return null;
    }

    public static string GetPacket(string search, int selectedPageIndex = 0)
    {
        return PacketManager.ComponentBuilder(PacketHeader, PacketVersion, selectedPageIndex, search);
    }

    protected override IEnumerable<string> GetContent()
    {
        return _data.Select(x => $"- {BotTranslations.ShortLevel}{x.Level} {Formatter.Bold(Formatter.Sanitize(x.Name))} ({x.Id})");
    }

    protected override DiscordSelectComponent SelectBuilder()
    {
        return ItemComponentsBuilder.ItemsSelectBuilder(0, _data);
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
