using Cyberia.Api;

using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Commands.Processors.SlashCommands.Metadata;
using DSharpPlus.Entities;

using System.ComponentModel;

namespace Cyberia.Salamandra.Commands.Dofus.Quest;

public sealed class QuestCommandModule
{
    [Command("quete"), Description("Retourne les informations d'une quête à partir de son nom")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall, DiscordApplicationIntegrationType.UserInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild, DiscordInteractionContextType.PrivateChannel)]
    public static async Task ExecuteAsync(SlashCommandContext ctx,
        [Parameter("nom"), Description("Nom de la quête")]
        [SlashAutoCompleteProvider<QuestAutocompleteProvider>]
        [SlashMinMaxLength(MinLength = 1, MaxLength = 70)]
        string value)
    {
        DiscordInteractionResponseBuilder? response = null;

        if (int.TryParse(value, out var id))
        {
            var questData = DofusApi.Datacenter.QuestsData.GetQuestDataById(id);
            if (questData is not null)
            {
                response = await new QuestMessageBuilder(questData).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }
        }
        else
        {
            var questsData = DofusApi.Datacenter.QuestsData.GetQuestsDataByName(value).ToList();
            if (questsData.Count == 1)
            {
                response = await new QuestMessageBuilder(questsData[0]).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }
            else if (questsData.Count > 1)
            {
                response = await new PaginatedQuestMessageBuilder(questsData, value).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }
        }

        response ??= new DiscordInteractionResponseBuilder().WithContent("Quête introuvable");
        await ctx.RespondAsync(response);
    }
}
