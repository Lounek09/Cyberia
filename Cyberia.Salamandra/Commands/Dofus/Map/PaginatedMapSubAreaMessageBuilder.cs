using Cyberia.Api.Data;
using Cyberia.Api.Data.Maps;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Formatters;
using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.Entities;

using Microsoft.Extensions.DependencyInjection;

using System.Globalization;

namespace Cyberia.Salamandra.Commands.Dofus.Map;

public sealed class PaginatedMapSubAreaMessageBuilder : PaginatedMessageBuilder<MapSubAreaData>
{
    public const string PacketHeader = "PMA.SA";
    public const int PacketVersion = 2;

    public PaginatedMapSubAreaMessageBuilder(
        IEmbedBuilderService embedBuilderService,
        List<MapSubAreaData> mapSubAreasData,
        string search,
        CultureInfo? culture,
        int selectedPageIndex = 0)
    : base(
        embedBuilderService.CreateEmbedBuilder(EmbedCategory.Map, Translation.Get<BotTranslations>("Embed.Map.Author", culture), culture),
        Translation.Get<BotTranslations>("Embed.PaginatedMapSubArea.Title", culture),
        mapSubAreasData,
        search,
        culture,
        selectedPageIndex)
    {

    }

    public static PaginatedMapSubAreaMessageBuilder? Create(IServiceProvider provider, int version, CultureInfo? culture, params ReadOnlySpan<string> parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 1 &&
            int.TryParse(parameters[0], out var selectedPageIndex))
        {
            var dofusDatacenter = provider.GetRequiredService<DofusDatacenter>();

            var mapSubAreasData = dofusDatacenter.MapsRepository.GetMapSubAreasDataByName(parameters[1], culture).ToList();
            if (mapSubAreasData.Count > 0)
            {
                var embedBuilderService = provider.GetRequiredService<IEmbedBuilderService>();

                return new(embedBuilderService, mapSubAreasData, parameters[1], culture, selectedPageIndex);
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
        return _data.Select(x => $"- {Formatter.Bold(x.Name.ToString(_culture))} ({x.Id})");
    }

    protected override DiscordSelectComponent SelectBuilder()
    {
        return MapComponentsBuilder.MapSubAreasSelectBuilder(0, _data, _culture);
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
