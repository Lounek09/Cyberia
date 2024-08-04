using Cyberia.Api;
using Cyberia.Api.Data.ItemSets;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus.ItemSet;

public sealed class PaginatedItemSetMessageBuilder : PaginatedMessageBuilder<ItemSetData>
{
    public const string PacketHeader = "PIS";
    public const int PacketVersion = 1;

    private readonly string _search;

    public PaginatedItemSetMessageBuilder(List<ItemSetData> itemSetsData, string search, int selectedPageIndex = 0)
        : base(EmbedCategory.Inventory, BotTranslations.Embed_ItemSet_Author, BotTranslations.Embed_PaginatedItemSet_Title, itemSetsData, selectedPageIndex)
    {
        _search = search;
    }

    public static PaginatedItemSetMessageBuilder? Create(int version, string[] parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 2 &&
            int.TryParse(parameters[1], out var selectedPageIndex))
        {
            var itemSetsData = DofusApi.Datacenter.ItemSetsRepository.GetItemSetsDataByName(parameters[2]).ToList();
            if (itemSetsData.Count > 0)
            {
                return new(itemSetsData, parameters[2], selectedPageIndex);
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
        return _data.Select(x => $"- {BotTranslations.ShortLevel}{x.GetLevel()} {Formatter.Bold(x.Name)} ({x.Id})");
    }

    protected override DiscordSelectComponent SelectBuilder()
    {
        return ItemSetComponentsBuilder.ItemSetsSelectBuilder(0, _data);
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
