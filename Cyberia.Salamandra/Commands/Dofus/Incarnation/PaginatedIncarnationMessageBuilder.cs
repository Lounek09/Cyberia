﻿using Cyberia.Api.DatacenterNS;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class PaginatedIncarnationMessageBuilder : PaginatedMessageBuilder<Incarnation>
    {
        public const string PACKET_HEADER = "PINCA";
        public const int PACKET_VERSION = 1;

        private readonly string _search;

        public PaginatedIncarnationMessageBuilder(List<Incarnation> incarnations, string search, int selectedPageIndex = 0) :
            base(DofusEmbedCategory.Inventory, "Incarnations", "Plusieurs incarnations trouvés :", incarnations, selectedPageIndex)
        {
            _search = search;
        }

        public static PaginatedIncarnationMessageBuilder? Create(int version, string[] parameters)
        {
            if (version == PACKET_VERSION &&
                parameters.Length > 2 &&
                int.TryParse(parameters[1], out int selectedPageIndex))
            {
                List<Incarnation> incarnations = Bot.Instance.Api.Datacenter.IncarnationsData.GetIncarnationsByName(parameters[2]);
                if (incarnations.Count > 0)
                    return new(incarnations, parameters[2], selectedPageIndex);
            }

            return null;
        }

        public static string GetPacket(string search, int selectedPageIndex = 0, PaginatedAction action = PaginatedAction.Nothing)
        {
            return InteractionManager.ComponentPacketBuilder(PACKET_HEADER, PACKET_VERSION, (int)action, selectedPageIndex, search);
        }

        protected override IEnumerable<string> GetContent()
        {
            return _data.Select(x => $"- {Formatter.Bold(x.Name.SanitizeMarkDown())} ({x.Id})");
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
}