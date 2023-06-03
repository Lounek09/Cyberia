using Cyberia.Api.DatacenterNS;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class PaginatedMonsterMessageBuilder : PaginatedMessageBuilder<Monster>
    {
        public const string PACKET_HEADER = "PM";
        public const int PACKET_VERSION = 1;

        private readonly string _search;

        public PaginatedMonsterMessageBuilder(List<Monster> monsters, string search, int selectedPageIndex = 0) :
            base(DofusEmbedCategory.Bestiary, "Bestiaire", "Plusieurs monstres trouvés :", monsters, selectedPageIndex)
        {
            _search = search;
        }

        public static PaginatedMonsterMessageBuilder? Create(int version, string[] parameters)
        {
            if (version == PACKET_VERSION &&
                parameters.Length > 2 &&
                int.TryParse(parameters[1], out int selectedPageIndex))
            {
                List<Monster> monsters = Bot.Instance.Api.Datacenter.MonstersData.GetMonstersByName(parameters[2]);
                if (monsters.Count > 0)
                    return new PaginatedMonsterMessageBuilder(monsters, parameters[2], selectedPageIndex);
            }

            return null;
        }

        public static string GetPacket(string search, int selectedPageIndex = 0, PaginatedAction action = PaginatedAction.Nothing)
        {
            return InteractionManager.ComponentPacketBuilder(PACKET_HEADER, PACKET_VERSION, (int)action, selectedPageIndex, search);
        }

        protected override IEnumerable<string> GetContent()
        {
            foreach (Monster monster in _data)
            {
                int minLevel = monster.GetMinLevel();
                int maxLevel = monster.GetMaxLevel();

                yield return $"- Niv.{minLevel}{(minLevel == maxLevel ? "" : $"-{maxLevel}")} {Formatter.Bold($"{monster.Name} {(monster.BreedSummon ? "(invocation)" : "")}")} ({monster.Id})";
            }
        }

        protected override DiscordSelectComponent SelectBuilder()
        {
            return MonsterComponentsBuilder.MonstersSelectBuilder(0, _data);
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
