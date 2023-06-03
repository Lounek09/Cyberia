using Cyberia.Api.DatacenterNS;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class PaginatedCraftMessageBuilder : PaginatedMessageBuilder<Craft>
    {
        public const string PACKET_HEADER = "PC";
        public const int PACKET_VERSION = 1;

        private readonly string _search;
        private readonly int _qte;

        public PaginatedCraftMessageBuilder(List<Craft> crafts, string search, int qte = 1, int selectedPageIndex = 0) :
            base(DofusEmbedCategory.Jobs, "Calculateur de crafts", "Plusieurs crafts trouvés :", crafts, selectedPageIndex)
        {
            _search = search;
            _qte = qte;
        }

        public static PaginatedCraftMessageBuilder? Create(int version, string[] parameters)
        {
            if (version == PACKET_VERSION &&
                parameters.Length > 3 &&
                int.TryParse(parameters[1], out int selectedPageIndex) &&
                int.TryParse(parameters[3], out int qte))
            {
                List<Craft> crafts = Bot.Instance.Api.Datacenter.CraftsData.GetCraftsByItemName(parameters[2]);
                if (crafts.Count > 0)
                    return new(crafts, parameters[2], qte, selectedPageIndex);
            }

            return null;
        }

        public static string GetPacket(string search, int qte, int selectedPageIndex = 0, PaginatedAction action = PaginatedAction.Nothing)
        {
            return InteractionManager.ComponentPacketBuilder(PACKET_HEADER, PACKET_VERSION, (int)action, selectedPageIndex, search, qte);
        }

        protected override IEnumerable<string> GetContent()
        {
            foreach (Craft craft in _data)
            {
                Item? item = craft.GetItem();
                if (item is not null)
                    yield return $"- Niv.{item.Level} {Formatter.Bold(item.Name.SanitizeMarkDown())} ({craft.Id})";
            }
        }

        protected override DiscordSelectComponent SelectBuilder()
        {
            return CraftComponentsBuilder.CraftsSelectBuilder(0, _data);
        }

        protected override string PreviousPacketBuilder()
        {
            return GetPacket(_search, _qte, PreviousPageIndex());
        }

        protected override string NextPacketBuilder()
        {
            return GetPacket(_search, _qte, NextPageIndex());
        }
    }
}
