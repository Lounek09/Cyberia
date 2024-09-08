using Cyberia.Api;
using Cyberia.Salamandra.Managers;

using DSharpPlus.Commands;
using DSharpPlus.Commands.ArgumentModifiers;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Commands.Processors.SlashCommands.Metadata;
using DSharpPlus.Entities;

using System.ComponentModel;

namespace Cyberia.Salamandra.Commands.Dofus.Incarnation;

public sealed class IncarnationCommandModule
{
    [Command("incarnation"), Description("Retourne les informations d'une incarnation à partir de son nom")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall, DiscordApplicationIntegrationType.UserInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild, DiscordInteractionContextType.PrivateChannel)]
    public static async Task ExecuteAsync(SlashCommandContext ctx,
        [Parameter("nom"), Description("Nom de l'incarnation")]
        [SlashAutoCompleteProvider<IncarnationAutocompleteProvider>]
        [MinMaxLength(1, 70)]
        string value)
    {
        CultureManager.SetCulture(ctx.Interaction);

        DiscordInteractionResponseBuilder? response = null;

        if (int.TryParse(value, out var id))
        {
            var incarnationData = DofusApi.Datacenter.IncarnationsRepository.GetIncarnationDataByItemId(id);
            if (incarnationData is not null)
            {
                response = await new IncarnationMessageBuilder(incarnationData).BuildAsync<DiscordInteractionResponseBuilder>();
            }
        }
        else
        {
            var incarnationsData = DofusApi.Datacenter.IncarnationsRepository.GetIncarnationsDataByItemName(value).ToList();
            if (incarnationsData.Count == 1)
            {
                response = await new IncarnationMessageBuilder(incarnationsData[0]).BuildAsync<DiscordInteractionResponseBuilder>();
            }
            else if (incarnationsData.Count > 1)
            {
                response = await new PaginatedIncarnationMessageBuilder(incarnationsData, value).BuildAsync<DiscordInteractionResponseBuilder>();
            }
        }

        response ??= new DiscordInteractionResponseBuilder().WithContent(BotTranslations.Incarnation_NotFound);
        await ctx.RespondAsync(response);
    }
}
