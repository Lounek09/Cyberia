using Cyberia.Api.Data;
using Cyberia.Api.Data.Spells;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Formatters;
using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.Entities;

using Microsoft.Extensions.DependencyInjection;

using System.Globalization;

namespace Cyberia.Salamandra.Commands.Dofus.Spell;

public sealed class PaginatedSpellMessageBuilder : PaginatedMessageBuilder<SpellData>
{
    public const string PacketHeader = "PS";
    public const int PacketVersion = 2;

    public PaginatedSpellMessageBuilder(
        IEmbedBuilderService embedBuilderService,
        List<SpellData> spellsData,
        string search,
        CultureInfo? culture,
        int selectedPageIndex = 0)
    : base(
        embedBuilderService.CreateEmbedBuilder(EmbedCategory.Spells, Translation.Get<BotTranslations>("Embed.Spell.Author", culture), culture),
        Translation.Get<BotTranslations>("Embed.PaginatedSpell.Title", culture),
        spellsData,
        search,
        culture,
        selectedPageIndex)
    {

    }

    public static PaginatedSpellMessageBuilder? Create(IServiceProvider provider, int version, CultureInfo? culture, params ReadOnlySpan<string> parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 1 &&
            int.TryParse(parameters[0], out var selectedPageIndex))
        {
            var dofusDatacenter = provider.GetRequiredService<DofusDatacenter>();

            var spellsData = dofusDatacenter.SpellsRepository.GetSpellsDataByName(parameters[1], culture).ToList();
            if (spellsData.Count > 0)
            {
                var embedBuilderService = provider.GetRequiredService<IEmbedBuilderService>();

                return new(embedBuilderService, spellsData, parameters[1], culture, selectedPageIndex);
            }
        }

        return null;
    }

    public static string GetPacket(string search, int selectedPageIndex = 0)
    {
        return PacketFormatter.Action(PacketHeader, PacketVersion, selectedPageIndex, search);
    }

    protected override IEnumerable<string> GetContent()
    {
        return _data.Select(x => $"- {(x.GetNeededLevel() == 0 ? string.Empty : $"{Translation.Get<BotTranslations>("ShortLevel", _culture)}{x.GetNeededLevel()}")} {Formatter.Bold(x.Name.ToString(_culture))} ({x.Id})");
    }

    protected override DiscordSelectComponent SelectBuilder()
    {
        return SpellComponentsBuilder.SpellsSelectBuilder(0, _data, _culture);
    }

    protected override string PreviousPacketBuilder()
    {
        return GetPacket(_search, PreviousPageIndex());
    }

    protected override string NextPacketBuilder()
    {
        return GetPacket(_search, NextPageIndex());
    }
}
