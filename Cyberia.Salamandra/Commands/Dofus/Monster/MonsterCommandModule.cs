using Cyberia.Api.DatacenterNS;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

using Newtonsoft.Json.Linq;

namespace Cyberia.Salamandra.Commands.Dofus
{
#pragma warning disable CA1822 // Mark members as static
    public sealed class MonsterCommandModule : ApplicationCommandModule
    {
        [SlashCommand("monstre", "Retourne les informations d'un monstre à partir de son nom")]
        public async Task Command(InteractionContext ctx,
            [Option("nom", "Nom du monstre", true)]
            [Autocomplete(typeof(MonsterAutocompleteProvider))]
            string value)
        {
            DiscordInteractionResponseBuilder? response = null;

            if (int.TryParse(value, out int id))
            {
                Monster? monster = Bot.Instance.Api.Datacenter.MonstersData.GetMonsterById(id);
                if (monster is not null)
                    response = await new MonsterMessageBuilder(monster).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }
            else
            {
                List<Monster> monsters = Bot.Instance.Api.Datacenter.MonstersData.GetMonstersByName(value);
                if (monsters.Count == 1)
                    response = await new MonsterMessageBuilder(monsters[0]).GetMessageAsync<DiscordInteractionResponseBuilder>();
                else if (monsters.Count > 1)
                    response = await new PaginatedMonsterMessageBuilder(monsters, value).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }

            response ??= new DiscordInteractionResponseBuilder().WithContent("Monstre introuvable");
            await ctx.CreateResponseAsync(response);

        }
    }
}
