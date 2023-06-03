using Cyberia.Api.DatacenterNS;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public static class MonsterComponentsBuilder
    {
        public static DiscordButtonComponent MonsterButtonBuilder(Monster monster, bool disable = false)
        {
            return new(ButtonStyle.Success, MonsterMessageBuilder.GetPacket(monster.Id), monster.Name, disable);
        }

        public static DiscordSelectComponent MonstersSelectBuilder(int index, List<Monster> monsters, bool disable = false)
        {
            IEnumerable<DiscordSelectComponentOption> options = monsters.Select(x => new DiscordSelectComponentOption(x.Name.WithMaxLength(100), MonsterMessageBuilder.GetPacket(x.Id), x.Id.ToString()));

            return new(InteractionManager.SelectComponentPacketBuilder(index), "Sélectionne un monstre pour l'afficher", options, disable);
        }
    }
}
