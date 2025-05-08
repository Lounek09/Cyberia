using Cyberia.Api.Data;
using Cyberia.Api.Data.Crafts;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Formatters;
using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.Entities;

using Microsoft.Extensions.DependencyInjection;

using System.Globalization;

namespace Cyberia.Salamandra.Commands.Dofus.Craft;

public sealed class PaginatedCraftMessageBuilder : PaginatedMessageBuilder<CraftData>
{
    public const string PacketHeader = "PC";
    public const int PacketVersion = 2;

    private readonly int _quantity;

    public PaginatedCraftMessageBuilder(
        EmbedBuilderService embedBuilderService,
        List<CraftData> craftsData,
        string search,
        int quantity,
        CultureInfo? culture,
        int selectedPageIndex = 0)
    : base(
        embedBuilderService.CreateEmbedBuilder(EmbedCategory.Jobs, Translation.Get<BotTranslations>("Embed.Craft.Author", culture), culture),
        Translation.Get<BotTranslations>("Embed.PaginatedCraft.Title", culture),
        craftsData,
        search,
        culture,
        selectedPageIndex)
    {
        _quantity = quantity;
    }

    public static PaginatedCraftMessageBuilder? Create(IServiceProvider provider, int version, CultureInfo? culture, params ReadOnlySpan<string> parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 2 &&
            int.TryParse(parameters[0], out var selectedPageIndex) &&
            int.TryParse(parameters[2], out var quantity))
        {
            var dofusDatacenter = provider.GetRequiredService<DofusDatacenter>();

            var craftsData = dofusDatacenter.CraftsRepository.GetCraftsDataByItemName(parameters[1], culture).ToList();
            if (craftsData.Count > 0)
            {
                var embedBuilderService = provider.GetRequiredService<EmbedBuilderService>();

                return new(embedBuilderService, craftsData, parameters[1], quantity, culture, selectedPageIndex);
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
        foreach (var craftData in _data)
        {
            var itemData = craftData.GetItemData();
            if (itemData is not null)
            {
                yield return $"- {Translation.Get<BotTranslations>("ShortLevel", _culture)}{itemData.Level} {Formatter.Bold(Formatter.Sanitize(itemData.Name.ToString(_culture)))} ({craftData.Id})";
            }
        }
    }

    protected override DiscordSelectComponent SelectBuilder()
    {
        return CraftComponentsBuilder.CraftsSelectBuilder(0, _data, _quantity, _culture);
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
