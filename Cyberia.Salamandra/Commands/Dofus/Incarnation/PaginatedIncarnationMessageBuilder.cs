﻿using Cyberia.Api.Data;
using Cyberia.Api.Data.Incarnations;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Formatters;
using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.Entities;

using Microsoft.Extensions.DependencyInjection;

using System.Globalization;

namespace Cyberia.Salamandra.Commands.Dofus.Incarnation;

public sealed class PaginatedIncarnationMessageBuilder : PaginatedMessageBuilder<IncarnationData>
{
    public const string PacketHeader = "PINCA";
    public const int PacketVersion = 2;

    public PaginatedIncarnationMessageBuilder(
        IEmbedBuilderService embedBuilderService,
        List<IncarnationData> incarnationsData,
        string search,
        CultureInfo? culture,
        int selectedPageIndex = 0)
    : base(
        embedBuilderService.CreateBaseEmbedBuilder(EmbedCategory.Inventory, Translation.Get<BotTranslations>("Embed.Incarnation.Author", culture), culture),
        Translation.Get<BotTranslations>("Embed.PaginatedIncarnation.Title", culture),
        incarnationsData,
        search,
        culture,
        selectedPageIndex)
    {

    }

    public static PaginatedIncarnationMessageBuilder? Create(IServiceProvider provider, int version, CultureInfo? culture, params ReadOnlySpan<string> parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 1 &&
            int.TryParse(parameters[0], out var selectedPageIndex))
        {
            var dofusDatacenter = provider.GetRequiredService<DofusDatacenter>();

            var incarnationsData = dofusDatacenter.IncarnationsRepository.GetIncarnationsDataByItemName(parameters[1], culture).ToList();
            if (incarnationsData.Count > 0)
            {
                var embedBuilderService = provider.GetRequiredService<IEmbedBuilderService>();

                return new(embedBuilderService, incarnationsData, parameters[1], culture, selectedPageIndex);
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
        foreach (var incarnationData in _data)
        {
            yield return $"- {Formatter.Bold(Formatter.Sanitize(incarnationData.GetItemName(_culture)))} ({incarnationData.Id})";
        }
    }

    protected override DiscordSelectComponent SelectBuilder()
    {
        return IncarnationComponentsBuilder.IncarnationsSelectBuilder(0, _data, _culture);
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
