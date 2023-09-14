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
                Incarnation? incarnation = Bot.Instance.Api.Datacenter.IncarnationsData.GetIncarnationById(id);
                if (incarnation is not null)
                    response = await new IncarnationMessageBuilder(incarnation).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }
            else
            {
                List<Incarnation> incarnations = Bot.Instance.Api.Datacenter.IncarnationsData.GetIncarnationsByName(value);
                if (incarnations.Count == 1)
                    response = await new IncarnationMessageBuilder(incarnations[0]).GetMessageAsync<DiscordInteractionResponseBuilder>();
                else if (incarnations.Count > 1)
                    response = await new PaginatedIncarnationMessageBuilder(incarnations, value).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }

            response ??= new DiscordInteractionResponseBuilder().WithContent("Incarnation introuvable");
            await ctx.CreateResponseAsync(response);
        }
    }
}
