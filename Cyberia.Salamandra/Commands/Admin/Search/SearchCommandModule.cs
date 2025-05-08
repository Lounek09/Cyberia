using Cyberia.Api.Data;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.ArgumentModifiers;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.Metadata;
using DSharpPlus.Entities;

using System.ComponentModel;
using System.Text;

namespace Cyberia.Salamandra.Commands.Admin.Search;

[Command("search")]
[InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall)]
[InteractionAllowedContexts(DiscordInteractionContextType.Guild)]
public sealed class SearchCommandModule
{
    private readonly ICultureService _cultureService;
    private readonly DofusDatacenter _dofusDatacenter;
    private readonly IEmbedBuilderService _embedBuilderService;

    public SearchCommandModule(ICultureService cultureService, DofusDatacenter dofusDatacenter, IEmbedBuilderService embedBuilderService)
    {
        _cultureService = cultureService;
        _dofusDatacenter = dofusDatacenter;
        _embedBuilderService = embedBuilderService;
    }

    [Command("effect"), Description("Search where the effect is used")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public async Task EffectExecuteAsync(SlashCommandContext ctx,
        [Parameter("where"), Description("Where to look for the effect")]
        SearchLocation location,
        [Parameter("id"), Description("Effect id")]
        [MinMaxValue(-1, 9999)]
        int effectId)
    {
        var culture = await _cultureService.GetCultureAsync(ctx.Interaction);

        StringBuilder descriptionBuilder = new();

        switch (location)
        {
            case SearchLocation.Item:
                foreach (var itemData in _dofusDatacenter.ItemsRepository.GetItemsDataWithEffectId(effectId))
                {
                    descriptionBuilder.Append("- ");
                    descriptionBuilder.Append(itemData.Name.ToString(culture));
                    descriptionBuilder.Append(" (");
                    descriptionBuilder.Append(itemData.Id);
                    descriptionBuilder.Append(")\n");
                }
                break;
            case SearchLocation.Spell:
                foreach (var spellData in _dofusDatacenter.SpellsRepository.GetSpellsDataWithEffectId(effectId))
                {
                    descriptionBuilder.Append("- ");
                    descriptionBuilder.Append(spellData.Name.ToString(culture));
                    descriptionBuilder.Append(" (");
                    descriptionBuilder.Append(spellData.Id);
                    descriptionBuilder.Append(")\n");
                }
                break;
            default:
                await ctx.RespondAsync($"Unknown {Formatter.Bold(location.ToString())}");
                return;
        }

        var embed = _embedBuilderService.CreateEmbedBuilder(EmbedCategory.Tools, "Tools", culture)
            .WithTitle($"Search effect {effectId} in {location}")
            .WithDescription(descriptionBuilder.ToString().WithMaxLength(4000));

        await ctx.RespondAsync(embed);
    }

    [Command("criterion"), Description("Search where the criterion is used")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public async Task CriterionExecuteAsync(SlashCommandContext ctx,
        [Parameter("where"), Description("Where to look for the criterion")]
        SearchLocation location,
        [Parameter("id"), Description("Criterion id")]
        [MinMaxLength(2, 2)]
        string criterionId)
    {
        var culture = await _cultureService.GetCultureAsync(ctx.Interaction);

        StringBuilder descriptionBuilder = new();

        switch (location)
        {
            case SearchLocation.Item:
                foreach (var itemData in _dofusDatacenter.ItemsRepository.GetItemsDataWithCriterionId(criterionId))
                {
                    descriptionBuilder.Append("- ");
                    descriptionBuilder.Append(itemData.Name.ToString(culture));
                    descriptionBuilder.Append(" (");
                    descriptionBuilder.Append(itemData.Id);
                    descriptionBuilder.Append(")\n");
                }
                break;
            case SearchLocation.Spell:
                foreach (var spellData in _dofusDatacenter.SpellsRepository.GetSpellsDataWithCriterionId(criterionId))
                {
                    descriptionBuilder.Append("- ");
                    descriptionBuilder.Append(spellData.Name.ToString(culture));
                    descriptionBuilder.Append(" (");
                    descriptionBuilder.Append(spellData.Id);
                    descriptionBuilder.Append(")\n");
                }
                break;
            default:
                await ctx.RespondAsync($"Unknown {Formatter.Bold(location.ToString())}");
                return;
        }

        await ctx.RespondAsync(_embedBuilderService.CreateEmbedBuilder(EmbedCategory.Tools, $"Search criterion {criterionId} in {location}", culture)
            .WithDescription(descriptionBuilder.ToString().WithMaxLength(4000)));
    }
}
