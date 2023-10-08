using Cyberia.Api.DatacenterNS;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class IncarnationCommandModule : ApplicationCommandModule
    {
        [SlashCommand("incarnation", "Retourne les informations d'une incarnation à partir de son nom")]
        public async Task Command(InteractionContext ctx,
            [Option("nom", "Nom de l'incarnation", true)]
            [Autocomplete(typeof(IncarnationAutocompleteProvider))]
            string value)
        {
            DiscordInteractionResponseBuilder? response = null;

            if (int.TryParse(value, out int id))
            {
                IncarnationData? incarnationData = Bot.Instance.Api.Datacenter.IncarnationsData.GetIncarnationDataById(id);
                if (incarnationData is not null)
                    response = await new IncarnationMessageBuilder(incarnationData).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }
            else
            {
                List<IncarnationData> incarnationsData = Bot.Instance.Api.Datacenter.IncarnationsData.GetIncarnationsDataByName(value);
                if (incarnationsData.Count == 1)
                    response = await new IncarnationMessageBuilder(incarnationsData[0]).GetMessageAsync<DiscordInteractionResponseBuilder>();
                else if (incarnationsData.Count > 1)
                    response = await new PaginatedIncarnationMessageBuilder(incarnationsData, value).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }

            response ??= new DiscordInteractionResponseBuilder().WithContent("Incarnation introuvable");
            await ctx.CreateResponseAsync(response);
        }
    }
}
