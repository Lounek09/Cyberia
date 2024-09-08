using Cyberia.Api;
using Cyberia.Api.Data.Incarnations;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus.Incarnation;

public sealed class PaginatedIncarnationMessageBuilder : PaginatedMessageBuilder<IncarnationData>
{
    public const string PacketHeader = "PINCA";
    public const int PacketVersion = 2;

    public PaginatedIncarnationMessageBuilder(List<IncarnationData> incarnationsData, string search, int selectedPageIndex = 0)
        : base(EmbedCategory.Inventory, BotTranslations.Embed_Incarnation_Author, BotTranslations.Embed_PaginatedIncarnation_Title, incarnationsData, search, selectedPageIndex)
    {

    }

    public static PaginatedIncarnationMessageBuilder? Create(int version, string[] parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 1 &&
            int.TryParse(parameters[0], out var selectedPageIndex))
        {
            var incarnationsData = DofusApi.Datacenter.IncarnationsRepository.GetIncarnationsDataByItemName(parameters[1]).ToList();
            if (incarnationsData.Count > 0)
            {
                return new(incarnationsData, parameters[1], selectedPageIndex);
            }
        }

        return null;
    }

    public static string GetPacket(string search, int selectedPageIndex = 0)
    {
        return InteractionManager.ComponentPacketBuilder(PacketHeader, PacketVersion, selectedPageIndex, search);
    }

    protected override IEnumerable<string> GetContent()
    {
        foreach (var incarnationData in _data)
        {
            var itemData = incarnationData.GetItemData();
            if (itemData is not null)
            {
                yield return $"- {Formatter.Bold(Formatter.Sanitize(itemData.Name))} ({incarnationData.Id})";
            }
        }
    }

    protected override DiscordSelectComponent SelectBuilder()
    {
        return IncarnationComponentsBuilder.IncarnationsSelectBuilder(0, _data);
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
