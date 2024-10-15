using Cyberia.Api;
using Cyberia.Api.Data.Items;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Formatters;
using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.Entities;

using Microsoft.Extensions.DependencyInjection;

using System.Globalization;

namespace Cyberia.Salamandra.Commands.Dofus.Rune;

public sealed class PaginatedRuneItemMessageBuilder : PaginatedMessageBuilder<ItemData>
{
    public const string PacketHeader = "PR";
    public const int PacketVersion = 2;

    private readonly int _quantity;

    public PaginatedRuneItemMessageBuilder(
        EmbedBuilderService embedBuilderService,
        List<ItemData> itemsData,
        string search,
        int quantity,
        CultureInfo? culture,
        int selectedPageIndex = 0)
    : base(
        embedBuilderService.CreateEmbedBuilder(EmbedCategory.Inventory, Translation.Get<BotTranslations>("Embed.Item.Author", culture)),
        Translation.Get<BotTranslations>("Embed.PaginatedItem.Title", culture),
        itemsData,
        search,
        culture,
        selectedPageIndex)
    {
        _quantity = quantity;
    }

    public static PaginatedRuneItemMessageBuilder? Create(IServiceProvider provider, int version, CultureInfo? culture, string[] parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 2 &&
            int.TryParse(parameters[0], out var selectedPageIndex) &&
            int.TryParse(parameters[2], out var quantity))
        {
            var itemsData = DofusApi.Datacenter.ItemsRepository.GetItemsDataByName(parameters[1], culture).ToList();
            if (itemsData.Count > 0)
            {
                var embedBuilderService = provider.GetRequiredService<EmbedBuilderService>();

                return new(embedBuilderService, itemsData, parameters[1], quantity, culture, selectedPageIndex);
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
        return _data.Select(x => $"- {Translation.Get<BotTranslations>("ShortLevel", _culture)}{x.Level} {Formatter.Bold(Formatter.Sanitize(x.Name.ToString(_culture)))} ({x.Id})");
    }

    protected override DiscordSelectComponent SelectBuilder()
    {
        return RuneComponentsBuilder.ItemsSelectBuilder(0, _data, _quantity, _culture);
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
