﻿using Cyberia.Api.DatacenterNS;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class PaginatedSpellMessageBuilder : PaginatedMessageBuilder<Spell>
    {
        public const string PACKET_HEADER = "PS";
        public const int PACKET_VERSION = 1;

        private readonly string _search;

        public PaginatedSpellMessageBuilder(List<Spell> spells, string search, int selectedPageIndex = 0) :
            base(DofusEmbedCategory.Spells, "Livre de sorts", "Plusieurs sorts trouvés :", spells, selectedPageIndex)
        {
            _search = search;
        }

        public static PaginatedSpellMessageBuilder? Create(int version, string[] parameters)
        {
            if (version == PACKET_VERSION &&
                parameters.Length > 2 &&
                int.TryParse(parameters[1], out int selectedPageIndex))
            {
                List<Spell> spells = Bot.Instance.Api.Datacenter.SpellsData.GetSpellsByName(parameters[2]);
                if (spells.Count > 0)
                    return new(spells, parameters[2], selectedPageIndex);
            }

            return null;
        }

        public static string GetPacket(string search, int selectedPageIndex = 0, PaginatedAction action = PaginatedAction.Nothing)
        {
            return InteractionManager.ComponentPacketBuilder(PACKET_HEADER, PACKET_VERSION, (int)action, selectedPageIndex, search);
        }

        protected override IEnumerable<string> GetContent()
        {
            return _data.Select(x => $"- {(x.GetNeededLevel() == 0 ? "" : $"Niv.{x.GetNeededLevel()}")} {Formatter.Bold(x.Name)} ({x.Id})");
        }

        protected override DiscordSelectComponent SelectBuilder()
        {
            return SpellComponentsBuilder.SpellsSelectBuilder(0, _data);
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