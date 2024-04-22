using Cyberia.Api;

using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Commands.Processors.SlashCommands.Metadata;
using DSharpPlus.Entities;

using System.ComponentModel;

namespace Cyberia.Salamandra.Commands.Dofus;

public sealed class IncarnationCommandModule
{
    [Command("incarnation"), Description("Retourne les informations d'une incarnation à partir de son nom")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall, DiscordApplicationIntegrationType.UserInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild, DiscordInteractionContextType.PrivateChannel)]
    public static async Task ExecuteAsync(SlashCommandContext ctx,
        [Parameter("nom"), Description("Nom de l'incarnation")]
        [SlashAutoCompleteProvider<IncarnationAutocompleteProvider>]
        [SlashMinMaxLength(MinLength = 1, MaxLength = 70)]
        string value)
    {
        DiscordInteractionResponseBuilder? response = null;

        if (int.TryParse(value, out var id))
        {
            var incarnationData = DofusApi.Datacenter.IncarnationsData.GetIncarnationDataByItemId(id);
            if (incarnationData is not null)
            {
                response = await new IncarnationMessageBuilder(incarnationData).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }
        }
        else
        {
            var incarnationsData = DofusApi.Datacenter.IncarnationsData.GetIncarnationsDataByName(value).ToList();
            if (incarnationsData.Count == 1)
            {
                response = await new IncarnationMessageBuilder(incarnationsData[0]).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }
            else if (incarnationsData.Count > 1)
            {
                response = await new PaginatedIncarnationMessageBuilder(incarnationsData, value).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }
        }

        response ??= new DiscordInteractionResponseBuilder().WithContent("Incarnation introuvable");
        await ctx.RespondAsync(response);
    }
}
