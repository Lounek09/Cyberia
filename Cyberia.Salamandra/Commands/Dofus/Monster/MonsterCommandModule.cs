using Cyberia.Api;

using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Commands.Processors.SlashCommands.Metadata;
using DSharpPlus.Entities;

using System.ComponentModel;

namespace Cyberia.Salamandra.Commands.Dofus.Monster;

public sealed class MonsterCommandModule
{
    [Command("monstre"), Description("Retourne les informations d'un monstre à partir de son nom")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall, DiscordApplicationIntegrationType.UserInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild, DiscordInteractionContextType.PrivateChannel)]
    public static async Task ExecuteAsync(SlashCommandContext ctx,
        [Parameter("nom"), Description("Nom du monstre")]
        [SlashAutoCompleteProvider<MonsterAutocompleteProvider>]
        [SlashMinMaxLength(MinLength = 1, MaxLength = 70)]
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
        await ctx.RespondAsync(response);
    }
}
