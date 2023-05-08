using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Managers;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class CraftMessageBuilder : CustomMessageBuilder
    {
        private readonly Craft _craft;
        private readonly Item? _item;
        private int _qte;
        private bool _recursive;

        public CraftMessageBuilder(Craft craft, int qte) :
            base()
        {
            _craft = craft;
            _item = craft.GetItem();
            _qte = qte;
            _recursive = false;
        }

        protected override async Task<DiscordEmbedBuilder> EmbedBuilder()
        {
            DiscordEmbedBuilder embed = EmbedManager.BuildDofusEmbed(DofusEmbedCategory.Jobs, "Calculateur de crafts")
                .WithDescription(string.Join('\n', _craft.GetCraftToString(_qte, _recursive, true)));

            if (_item is not null)
            {
                embed.WithTitle($"Craft : {_qte.ToStringThousandSeparator()}x {_item.Name.SanitizeMarkDown()} ({_craft.Id})")
                    .WithThumbnail(await _item.GetImgPath());
            }

            int weight = _recursive ? _craft.GetRecursiveWeight() : _craft.GetWeight();
            embed.AddField("Poids :", $"{Formatter.Bold(weight.ToStringThousandSeparator())} pod{(weight > 1 ? "s" : "")} par craft" + (_qte > 1 ? $", {Formatter.Bold((weight * _qte).ToStringThousandSeparator())} au total" : ""));

            string time = $"{Formatter.Bold((_recursive ? _craft.GetRecursiveTimeForMultipleCraft(1) : _craft.GetTimeForMultipleCraft(1)).ToString(@"mm\mss\sfff"))} par craft";
            if (_qte > 1)
            {
                TimeSpan totalTime = _recursive ? _craft.GetRecursiveTimeForMultipleCraft(_qte) : _craft.GetTimeForMultipleCraft(_qte);
                string format = (totalTime.TotalDays < 1 ? "" : @"%d\d") + (totalTime.TotalHours < 1 ? "" : @"hh\h") + @"mm\mss\sfff";
                time += $"\n{Formatter.Bold(totalTime.ToString(format))} au total";
            }
            embed.AddField("Temps de craft approximatif:", time);

            return embed;
        }

        private HashSet<DiscordButtonComponent> LessButtonsBuilder()
        {
            return new HashSet<DiscordButtonComponent>()
            {
                new(ButtonStyle.Danger, "-100", "-100", (_qte - 100) < 1),
                new(ButtonStyle.Danger, "-10", "-10", (_qte - 10) < 1),
                new(ButtonStyle.Danger, "-1", "-1", (_qte - 1) < 1)
            };
        }

        private static HashSet<DiscordButtonComponent> MoreButtonsBuilder()
        {
            return new HashSet<DiscordButtonComponent>()
            {
                new(ButtonStyle.Success, "100", "+100"),
                new(ButtonStyle.Success, "10", "+10"),
                new(ButtonStyle.Success, "1", "+1")
            };
        }

        private HashSet<DiscordButtonComponent> OtherButtonsBuilder()
        {
            HashSet<DiscordButtonComponent> buttons = new()
            {
                new(ButtonStyle.Primary, "recursive", $"{(_recursive ? "Masquer" : "Afficher")} les sous crafts", !_craft.HasSubCraft())
            };

            if (_item is not null)
                buttons.Add(new(ButtonStyle.Primary, "item", "Item"));

            return buttons;
        }

        protected override async Task<DiscordInteractionResponseBuilder> InteractionResponseBuilder()
        {
            DiscordInteractionResponseBuilder response = (await base.InteractionResponseBuilder())
                .AddComponents(LessButtonsBuilder())
                .AddComponents(MoreButtonsBuilder());

            HashSet<DiscordButtonComponent> buttons = OtherButtonsBuilder();
            if (buttons.Count > 0)
                response.AddComponents(buttons);

            return response;
        }

        protected override async Task<DiscordFollowupMessageBuilder> FollowupMessageBuilder()
        {
            DiscordFollowupMessageBuilder followupMessage = (await base.FollowupMessageBuilder())
                .AddComponents(LessButtonsBuilder())
                .AddComponents(MoreButtonsBuilder());

            HashSet<DiscordButtonComponent> buttons = OtherButtonsBuilder();
            if (buttons.Count > 0)
                followupMessage.AddComponents(buttons);

            return followupMessage;
        }

        protected override async Task<bool> InteractionTreatment(ComponentInteractionCreateEventArgs e)
        {
            if (int.TryParse(e.Id, out int qte))
            {
                _qte += (_qte + qte) > 0 ? qte : 0;

                await UpdateInteractionResponse(e.Interaction);
                return true;
            }

            switch (e.Id)
            {
                case "recursive":
                    _recursive = !_recursive;

                    await UpdateInteractionResponse(e.Interaction);
                    return true;
                case "item":
                    if (_item is not null)
                    {
                        await new ItemMessageBuilder(_item).UpdateInteractionResponse(e.Interaction);
                        return true;
                    }
                    break;
            }

            await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
            return false;
        }
    }
}
