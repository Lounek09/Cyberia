using Cyberia.Api;
using Cyberia.Api.Data.Crafts;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus.Craft;

public sealed class PaginatedCraftMessageBuilder : PaginatedMessageBuilder<CraftData>
{
    public const string PacketHeader = "PC";
    public const int PacketVersion = 1;

    private readonly string _search;
    private readonly int _quantity;

    public PaginatedCraftMessageBuilder(List<CraftData> craftsData, string search, int quantity = 1, int selectedPageIndex = 0)
        : base(EmbedCategory.Jobs, BotTranslations.Embed_Craft_Author, BotTranslations.Embed_PaginatedCraft_Title, craftsData, selectedPageIndex)
    {
        _search = search;
        _quantity = quantity;
    }

    public static PaginatedCraftMessageBuilder? Create(int version, string[] parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 3 &&
            int.TryParse(parameters[1], out var selectedPageIndex) &&
            int.TryParse(parameters[3], out var quantity))
        {
            var craftsData = DofusApi.Datacenter.CraftsRepository.GetCraftsDataByItemName(parameters[2]).ToList();
            if (craftsData.Count > 0)
            {
                return new(craftsData, parameters[2], quantity, selectedPageIndex);
            }
        }

        return null;
    }

    public static string GetPacket(string search, int quantity, int selectedPageIndex = 0, PaginatedAction action = PaginatedAction.None)
    {
        return InteractionManager.ComponentPacketBuilder(PacketHeader, PacketVersion, (int)action, selectedPageIndex, search, quantity);
    }

    protected override IEnumerable<string> GetContent()
    {
        foreach (var craftData in _data)
        {
            var itemData = craftData.GetItemData();
            if (itemData is not null)
            {
                yield return $"- {BotTranslations.ShortLevel}{itemData.Level} {Formatter.Bold(Formatter.Sanitize(itemData.Name))} ({craftData.Id})";
            }
        }
    }

    protected override DiscordSelectComponent SelectBuilder()
    {
        return CraftComponentsBuilder.CraftsSelectBuilder(0, _data, _quantity);
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
