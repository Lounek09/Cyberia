﻿using Cyberia.Api.DatacenterNS;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class PaginatedQuestMessageBuilder : PaginatedMessageBuilder<Quest>
    {
        public const string PACKET_HEADER = "PQ";
        public const int PACKET_VERSION = 1;

        private readonly string _search;

        public PaginatedQuestMessageBuilder(List<Quest> quests, string search, int selectedPageIndex = 0) :
            base(DofusEmbedCategory.Quests, "Livre de quêtes", "Plusieurs quêtes trouvées :", quests, selectedPageIndex)
        {
            _search = search
                ;
        }

        public static PaginatedQuestMessageBuilder? Create(int version, string[] parameters)
        {
            if (version == PACKET_VERSION &&
                parameters.Length > 2 &&
                int.TryParse(parameters[1], out int selectedPageIndex))
            {
                List<Quest> quests = Bot.Instance.Api.Datacenter.QuestsData.GetQuestsByName(parameters[2]);
                if (quests.Count > 0)
                    return new(quests, parameters[2], selectedPageIndex);
            }

            return null;
        }

        public static string GetPacket(string search, int selectedPageIndex = 0, PaginatedAction action = PaginatedAction.Nothing)
        {
            return InteractionManager.ComponentPacketBuilder(PACKET_HEADER, PACKET_VERSION, (int)action, selectedPageIndex, search);
        }

        protected override IEnumerable<string> GetContent()
        {
            return _data.Select(x => $"- {Formatter.Bold(x.Name)} ({x.Id}) {Emojis.Quest(x.Repeatable, x.Account)}{(x.HasDungeon ? Emojis.DUNGEON : "")}");
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
}