﻿using Cyberia.Api.DatacenterNS;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class PaginatedItemMessageBuilder : PaginatedMessageBuilder<Item>
    {
        public const string PACKET_HEADER = "PI";
        public const int PACKET_VERSION = 1;

        private readonly string _search;

        public PaginatedItemMessageBuilder(List<Item> items, string search, int selectedPageIndex = 0) :
            base(DofusEmbedCategory.Inventory, "Items", "Plusieurs objets trouvés :", items, selectedPageIndex)
        {
            _search = search;
        }

        public static PaginatedItemMessageBuilder? Create(int version, string[] parameters)
        {
            if (version == PACKET_VERSION &&
                parameters.Length > 2 &&
                int.TryParse(parameters[1], out int selectedPageIndex))
            {
                List<Item> items = Bot.Instance.Api.Datacenter.ItemsData.GetItemsByName(parameters[2]);
                if (items.Count > 0)
                    return new(items, parameters[2], selectedPageIndex);
            }

            return null;
        }

        public static string GetPacket(string search, int selectedPageIndex = 0, PaginatedAction action = PaginatedAction.Nothing)
        {
            return InteractionManager.ComponentPacketBuilder(PACKET_HEADER, PACKET_VERSION, (int)action, selectedPageIndex, search);
        }

        protected override IEnumerable<string> GetContent()
        {
            return _data.Select(x => $"- Niv.{x.Level} {Formatter.Bold(x.Name.SanitizeMarkDown())} ({x.Id})");
        }

        protected override DiscordSelectComponent SelectBuilder()
        {
            return ItemComponentsBuilder.ItemsSelectBuilder(0, _data);
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