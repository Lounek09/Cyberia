using Cyberia.Api.DatacenterNS;

using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus
{
#pragma warning disable CA1822 // Mark members as static
    public sealed class MonsterCommandModule : ApplicationCommandModule
    {
        [SlashCommand("monstre", "Retourne les informations d'un monstre à partir de son nom")]
        public async Task Command(InteractionContext ctx,
            [Option("nom", "Nom du monstre")]
            [Autocomplete(typeof(MonsterAutocompleteProvider))]
            string sId)
        {
            Monster? monster = null;

            if (int.TryParse(sId, out int id))
                monster = Bot.Instance.Api.Datacenter.MonstersData.GetMonsterById(id);

            if (monster is null)
                await ctx.CreateResponseAsync("Monstre introuvable");
            else
                await new MonsterMessageBuilder(monster).SendInteractionResponse(ctx.Interaction);

        }
    }
}
