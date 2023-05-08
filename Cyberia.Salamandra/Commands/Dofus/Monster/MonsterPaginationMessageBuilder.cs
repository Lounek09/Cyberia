using Cyberia.Api.DatacenterNS;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class MonsterPaginationMessageBuilder : PaginationMessageBuilder
    {
        private readonly List<Monster> _monsters;

        public MonsterPaginationMessageBuilder(List<Monster> monsters) :
            base(DofusEmbedCategory.Bestiary, "Bestiaire", "Plusieurs monstres trouvés :")
        {
            _monsters = monsters;
            PopulateContent();
        }

        protected override void PopulateContent()
        {
            foreach (Monster monster in _monsters)
            {
                int minLevel = monster.GetMinLevel();
                int maxLevel = monster.GetMaxLevel();

                _content.Add($"- Niv.{minLevel}{(minLevel == maxLevel ? "" : $"-{maxLevel}")} {Formatter.Bold($"{monster.Name} {(monster.BreedSummon ? "(invocation)" : "")}")} ({monster.Id})");
            }
        }

        protected override DiscordSelectComponent SelectBuilder()
        {
            HashSet<DiscordSelectComponentOption> options = new();
            foreach (Monster monster in _monsters.GetRange(GetStartPageIndex(), GetEndPageIndex()))
                options.Add(new(monster.Name.WithMaxLength(100), monster.Id.ToString(), monster.Id.ToString()));

            return new("select", "Sélectionne un monstre pour l'afficher", options);
        }

        protected override async Task<bool> InteractionTreatment(ComponentInteractionCreateEventArgs e)
        {
            if (await base.InteractionTreatment(e))
                return true;

            if (e.Id.Equals("select"))
            {
                int id = Convert.ToInt32(e.Interaction.Data.Values.First());

                Monster? monster = Bot.Instance.Api.Datacenter.MonstersData.GetMonsterById(id);
                if (monster is not null)
                {
                    await new MonsterMessageBuilder(monster).UpdateInteractionResponse(e.Interaction);
                    return true;
                }
            }

            await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
            return false;
        }
    }
}
