using Cyberia.Api.DatacenterNS;

using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus
{
#pragma warning disable CA1822 // Mark members as static
    public sealed class IncarnationCommandModule : ApplicationCommandModule
    {
        [SlashCommand("incarnation", "Retourne les informations d'une incarnation à partir de son nom")]
        public async Task Command(InteractionContext ctx,
            [Option("nom", "Nom de l'incarnation")]
            [Autocomplete(typeof(IncarnationAutocompleteProvider))]
            string sId)
        {
            Incarnation? incarnation = null;

            if (int.TryParse(sId, out int id))
                incarnation = Bot.Instance.Api.Datacenter.IncarnationsData.GetIncarnationById(id);

            if (incarnation is null)
                await ctx.CreateResponseAsync("Incarnation introuvable");
            else
                await new IncarnationMessageBuilder(incarnation).SendInteractionResponse(ctx.Interaction);
        }
    }
}
