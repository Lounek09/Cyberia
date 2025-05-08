using Cyberia.Api.Data;
using Cyberia.Api.Data.ItemSets;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Formatters;
using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.Entities;

using Microsoft.Extensions.DependencyInjection;

using System.Globalization;

namespace Cyberia.Salamandra.Commands.Dofus.ItemSet;

public sealed class PaginatedItemSetMessageBuilder : PaginatedMessageBuilder<ItemSetData>
{
    public const string PacketHeader = "PIS";
    public const int PacketVersion = 2;

    public PaginatedItemSetMessageBuilder(
        IEmbedBuilderService embedBuilderService,
        List<ItemSetData> itemSetsData,
        string search,
        CultureInfo? culture,
        int selectedPageIndex = 0)
    : base(
        embedBuilderService.CreateEmbedBuilder(EmbedCategory.Inventory, Translation.Get<BotTranslations>("Embed.ItemSet.Author", culture), culture),
        Translation.Get<BotTranslations>("Embed.PaginatedItemSet.Title", culture),
        itemSetsData,
        search,
        culture,
        selectedPageIndex)
    {

    }

    public static PaginatedItemSetMessageBuilder? Create(IServiceProvider provider, int version, CultureInfo? culture, params ReadOnlySpan<string> parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 1 &&
            int.TryParse(parameters[0], out var selectedPageIndex))
        {
            var dofusDatacenter = provider.GetRequiredService<DofusDatacenter>();

            var itemSetsData = dofusDatacenter.ItemSetsRepository.GetItemSetsDataByName(parameters[1], culture).ToList();
            if (itemSetsData.Count > 0)
            {
                var embedBuilderService = provider.GetRequiredService<IEmbedBuilderService>();

                return new(embedBuilderService, itemSetsData, parameters[1], culture, selectedPageIndex);
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
        return _data.Select(x => $"- {Translation.Get<BotTranslations>("ShortLevel", _culture)}{x.GetLevel()} {Formatter.Bold(x.Name.ToString(_culture))} ({x.Id})");
    }

    protected override DiscordSelectComponent SelectBuilder()
    {
        return ItemSetComponentsBuilder.ItemSetsSelectBuilder(0, _data, _culture);
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
