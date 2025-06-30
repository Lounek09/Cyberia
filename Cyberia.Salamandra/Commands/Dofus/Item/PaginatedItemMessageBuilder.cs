using Cyberia.Api.Data;
using Cyberia.Api.Data.Items;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Formatters;
using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.Entities;

using Microsoft.Extensions.DependencyInjection;

using System.Globalization;

namespace Cyberia.Salamandra.Commands.Dofus.Item;

public sealed class PaginatedItemMessageBuilder : PaginatedMessageBuilder<ItemData>
{
    public const string PacketHeader = "PI";
    public const int PacketVersion = 2;

    public PaginatedItemMessageBuilder(
        IEmbedBuilderService embedBuilderService,
        List<ItemData> itemsData,
        string search,
        CultureInfo? culture,
        int selectedPageIndex = 0)
    : base(
        embedBuilderService.CreateBaseEmbedBuilder(EmbedCategory.Inventory, Translation.Get<BotTranslations>("Embed.Item.Author", culture), culture),
        Translation.Get<BotTranslations>("Embed.PaginatedItem.Title", culture),
        itemsData,
        search,
        culture,
        selectedPageIndex)
    {

    }

    public static PaginatedItemMessageBuilder? Create(IServiceProvider provider, int version, CultureInfo? culture, params ReadOnlySpan<string> parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 1 &&
            int.TryParse(parameters[0], out var selectedPageIndex))
        {
            var dofusDatacenter = provider.GetRequiredService<DofusDatacenter>();

            var itemsData = dofusDatacenter.ItemsRepository.GetItemsDataByName(parameters[1], culture).ToList();
            if (itemsData.Count > 0)
            {
                var embedBuilderService = provider.GetRequiredService<IEmbedBuilderService>();

                return new(embedBuilderService, itemsData, parameters[1], culture, selectedPageIndex);
            }
        }

        return null;
    }

    public static string GetPacket(string search, int selectedPageIndex = 0)
    {
        return PacketFormatter.Action(PacketHeader, PacketVersion, selectedPageIndex, search);
    }

    protected override IEnumerable<string> GetContent()
    {
        return _data.Select(x => $"- {Translation.Get<BotTranslations>("ShortLevel", _culture)}{x.Level} {Formatter.Bold(Formatter.Sanitize(x.Name.ToString(_culture)))} ({x.Id})");
    }

    protected override DiscordSelectComponent SelectBuilder()
    {
        return ItemComponentsBuilder.ItemsSelectBuilder(0, _data, _culture);
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
