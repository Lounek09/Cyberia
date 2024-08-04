using Cyberia.Api;
using Cyberia.Api.Data.Quests;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus.Quest;

public sealed class PaginatedQuestMessageBuilder : PaginatedMessageBuilder<QuestData>
{
    public const string PacketHeader = "PQ";
    public const int PacketVersion = 2;

    private readonly string _search;

    public PaginatedQuestMessageBuilder(List<QuestData> questsData, string search, int selectedPageIndex = 0)
        : base(EmbedCategory.Quests, BotTranslations.Embed_Quest_Author, BotTranslations.Embed_PaginatedQuest_Title, questsData, selectedPageIndex)
    {
        _search = search;
    }

    public static PaginatedQuestMessageBuilder? Create(int version, string[] parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 2 &&
            int.TryParse(parameters[1], out var selectedPageIndex))
        {
            var questsData = DofusApi.Datacenter.QuestsRepository.GetQuestsDataByName(parameters[2]).ToList();
            if (questsData.Count > 0)
            {
                return new(questsData, parameters[2], selectedPageIndex);
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
        return _data.Select(x => $"- {Formatter.Bold(x.Name)} ({x.Id}) {Emojis.Quest(x.Repeatable, x.Account)}{(x.HasDungeon ? Emojis.Dungeon : string.Empty)}");
    }

    protected override DiscordSelectComponent SelectBuilder()
    {
        return QuestComponentsBuilder.QuestsSelectBuilder(0, _data);
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
