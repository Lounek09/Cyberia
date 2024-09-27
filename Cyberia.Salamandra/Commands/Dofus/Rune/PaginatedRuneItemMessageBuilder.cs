using Cyberia.Api;
using Cyberia.Api.Data.Items;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Formatters;
using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.Entities;

using Microsoft.Extensions.DependencyInjection;

namespace Cyberia.Salamandra.Commands.Dofus.Rune;

public sealed class PaginatedRuneItemMessageBuilder : PaginatedMessageBuilder<ItemData>
{
    public const string PacketHeader = "PR";
    public const int PacketVersion = 2;

    private readonly int _quantity;

    public PaginatedRuneItemMessageBuilder(EmbedBuilderService embedBuilderService, List<ItemData> itemsData, string search, int quantity = 1, int selectedPageIndex = 0)
        : base(embedBuilderService.CreateEmbedBuilder(EmbedCategory.Inventory, BotTranslations.Embed_Item_Author), BotTranslations.Embed_PaginatedItem_Title, itemsData, search, selectedPageIndex)
    {
        _quantity = quantity;
    }

    public static PaginatedRuneItemMessageBuilder? Create(IServiceProvider provider, int version, string[] parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 2 &&
            int.TryParse(parameters[0], out var selectedPageIndex) &&
            int.TryParse(parameters[2], out var quantity))
        {
            var itemsData = DofusApi.Datacenter.ItemsRepository.GetItemsDataByName(parameters[1]).ToList();
            if (itemsData.Count > 0)
            {
                var embedBuilderService = provider.GetRequiredService<EmbedBuilderService>();

                return new(embedBuilderService, itemsData, parameters[1], quantity, selectedPageIndex);
            }
        }

        return null;
    }

    public static string GetPacket(string search, int quantity, int selectedPageIndex = 0)
    {
        return PacketFormatter.Action(PacketHeader, PacketVersion, selectedPageIndex, search, quantity);
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
