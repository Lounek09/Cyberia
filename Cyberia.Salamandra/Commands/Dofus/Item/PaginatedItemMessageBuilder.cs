using Cyberia.Api.DatacenterNS;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class PaginatedItemMessageBuilder : PaginatedMessageBuilder<ItemData>
    {
        public const string PACKET_HEADER = "PI";
        public const int PACKET_VERSION = 1;

        private readonly string _search;

        public PaginatedItemMessageBuilder(List<ItemData> itemsData, string search, int selectedPageIndex = 0) :
            base(DofusEmbedCategory.Inventory, "Items", "Plusieurs objets trouvés :", itemsData, selectedPageIndex)
        {
            _search = search;
        }

        public static PaginatedItemMessageBuilder? Create(int version, string[] parameters)
        {
            if (version == PACKET_VERSION &&
                parameters.Length > 2 &&
                int.TryParse(parameters[1], out int selectedPageIndex))
            {
                List<ItemData> itemsData = Bot.Instance.Api.Datacenter.ItemsData.GetItemsDataByName(parameters[2]);
                if (itemsData.Count > 0)
                    return new(itemsData, parameters[2], selectedPageIndex);
            }

            return null;
        }

        public static string GetPacket(string search, int selectedPageIndex = 0, PaginatedAction action = PaginatedAction.Nothing)
        {
            return InteractionManager.ComponentPacketBuilder(PACKET_HEADER, PACKET_VERSION, (int)action, selectedPageIndex, search);
        }

        protected override IEnumerable<string> GetContent()
        {
            return _data.Select(x => $"- Niv.{x.Level} {Formatter.Bold(Formatter.Sanitize(x.Name))} ({x.Id})");
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
