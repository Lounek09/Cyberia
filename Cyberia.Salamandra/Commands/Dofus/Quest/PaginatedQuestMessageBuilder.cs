using Cyberia.Api;
using Cyberia.Api.Data.Quests;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Formatters;
using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.Entities;

using Microsoft.Extensions.DependencyInjection;

namespace Cyberia.Salamandra.Commands.Dofus.Quest;

public sealed class PaginatedQuestMessageBuilder : PaginatedMessageBuilder<QuestData>
{
    public const string PacketHeader = "PQ";
    public const int PacketVersion = 2;

    public PaginatedQuestMessageBuilder(EmbedBuilderService embedBuilderService, List<QuestData> questsData, string search, int selectedPageIndex = 0)
        : base(embedBuilderService.CreateEmbedBuilder(EmbedCategory.Quests, BotTranslations.Embed_Quest_Author), BotTranslations.Embed_PaginatedQuest_Title, questsData, search, selectedPageIndex)
    {

    }

    public static PaginatedQuestMessageBuilder? Create(IServiceProvider provider, int version, string[] parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 1 &&
            int.TryParse(parameters[0], out var selectedPageIndex))
        {
            var questsData = DofusApi.Datacenter.QuestsRepository.GetQuestsDataByName(parameters[1]).ToList();
            if (questsData.Count > 0)
            {
                var embedBuilderService = provider.GetRequiredService<EmbedBuilderService>();

                return new(embedBuilderService, questsData, parameters[1], selectedPageIndex);
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
