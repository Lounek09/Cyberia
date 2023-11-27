using Cyberia.Api;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus;

public sealed class MonsterCommandModule : ApplicationCommandModule
{
    [SlashCommand("monstre", "Retourne les informations d'un monstre à partir de son nom")]
    public async Task Command(InteractionContext ctx,
        [Option("nom", "Nom du monstre", true)]
        [Autocomplete(typeof(MonsterAutocompleteProvider))]
        string value)
    {
        DiscordInteractionResponseBuilder? response = null;

        if (int.TryParse(value, out var id))
        {
            var monsterData = DofusApi.Datacenter.MonstersData.GetMonsterDataById(id);
            if (monsterData is not null)
            {
                response = await new MonsterMessageBuilder(monsterData).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }
        }
        else
        {
            var monstersData = DofusApi.Datacenter.MonstersData.GetMonstersDataByName(value).ToList();
            if (monstersData.Count == 1)
            {
                response = await new MonsterMessageBuilder(monstersData[0]).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }
            else if (monstersData.Count > 1)
            {
                response = await new PaginatedMonsterMessageBuilder(monstersData, value).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }
        }

        response ??= new DiscordInteractionResponseBuilder().WithContent("Monstre introuvable");
        await ctx.CreateResponseAsync(response);

    }
}
