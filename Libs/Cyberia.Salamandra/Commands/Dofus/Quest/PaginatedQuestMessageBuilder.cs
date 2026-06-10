using Cyberia.Api.Data;
using Cyberia.Api.Data.Quests;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Formatters;
using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.Entities;

using Microsoft.Extensions.DependencyInjection;

using System.Globalization;

namespace Cyberia.Salamandra.Commands.Dofus.Quest;

public sealed class PaginatedQuestMessageBuilder : PaginatedMessageBuilder<QuestData>
{
    public const string PacketHeader = "PQ";
    public const int PacketVersion = 2;

    public PaginatedQuestMessageBuilder(
        IEmbedBuilderService embedBuilderService,
        List<QuestData> questsData,
        string search,
        CultureInfo? culture,
        int selectedPageIndex = 0)
    : base(
        embedBuilderService.CreateBaseEmbedBuilder(EmbedCategory.Quests, Translation.Get<BotTranslations>("Embed.Quest.Author", culture), culture),
        Translation.Get<BotTranslations>("Embed.PaginatedQuest.Title", culture),
        questsData,
        search,
        culture,
        selectedPageIndex)
    { }

    public static PaginatedQuestMessageBuilder? Create(IServiceProvider provider, int version, CultureInfo? culture, params ReadOnlySpan<string> parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 1 &&
            int.TryParse(parameters[0], out var selectedPageIndex))
        {
            var dofusDatacenter = provider.GetRequiredService<DofusDatacenter>();

            var questsData = dofusDatacenter.QuestsRepository.GetQuestsDataByName(parameters[1], culture).ToList();
            if (questsData.Count > 0)
            {
                var embedBuilderService = provider.GetRequiredService<IEmbedBuilderService>();

                return new(embedBuilderService, questsData, parameters[1], culture, selectedPageIndex);
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
        return _data.Select(x => $"- {Formatter.Bold(x.Name.ToString(_culture))} ({x.Id}) {Emojis.Quest(x, _culture)}{(x.HasDungeon ? Emojis.Dungeon(_culture) : string.Empty)}");
    }

    protected override DiscordSelectComponent SelectBuilder()
    {
        return QuestComponentsBuilder.QuestsSelectBuilder(0, _data, _culture);
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
