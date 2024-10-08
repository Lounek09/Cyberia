﻿using Cyberia.Api;
using Cyberia.Api.Data.ItemSets;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Formatters;
using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.Entities;

using Microsoft.Extensions.DependencyInjection;

namespace Cyberia.Salamandra.Commands.Dofus.ItemSet;

public sealed class PaginatedItemSetMessageBuilder : PaginatedMessageBuilder<ItemSetData>
{
    public const string PacketHeader = "PIS";
    public const int PacketVersion = 2;

    public PaginatedItemSetMessageBuilder(EmbedBuilderService embedBuilderService, List<ItemSetData> itemSetsData, string search, int selectedPageIndex = 0)
        : base(embedBuilderService.CreateEmbedBuilder(EmbedCategory.Inventory, BotTranslations.Embed_ItemSet_Author), BotTranslations.Embed_PaginatedItemSet_Title, itemSetsData, search, selectedPageIndex)
    {
    }

    public static PaginatedItemSetMessageBuilder? Create(IServiceProvider provider, int version, string[] parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 1 &&
            int.TryParse(parameters[0], out var selectedPageIndex))
        {
            var itemSetsData = DofusApi.Datacenter.ItemSetsRepository.GetItemSetsDataByName(parameters[1]).ToList();
            if (itemSetsData.Count > 0)
            {
                var embedBuilderService = provider.GetRequiredService<EmbedBuilderService>();

                return new(embedBuilderService, itemSetsData, parameters[1], selectedPageIndex);
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
