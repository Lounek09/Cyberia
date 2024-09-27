using Cyberia.Api;
using Cyberia.Api.Data.Spells;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Formatters;
using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.Entities;

using Microsoft.Extensions.DependencyInjection;

namespace Cyberia.Salamandra.Commands.Dofus.Spell;

public sealed class PaginatedSpellMessageBuilder : PaginatedMessageBuilder<SpellData>
{
    public const string PacketHeader = "PS";
    public const int PacketVersion = 2;

    public PaginatedSpellMessageBuilder(EmbedBuilderService embedBuilderService, List<SpellData> spellsData, string search, int selectedPageIndex = 0)
        : base(embedBuilderService.CreateEmbedBuilder(EmbedCategory.Spells, BotTranslations.Embed_Spell_Author), BotTranslations.Embed_PaginatedSpell_Title, spellsData, search, selectedPageIndex)
    {

    }

    public static PaginatedSpellMessageBuilder? Create(IServiceProvider provider, int version, string[] parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 1 &&
            int.TryParse(parameters[0], out var selectedPageIndex))
        {
            var spellsData = DofusApi.Datacenter.SpellsRepository.GetSpellsDataByName(parameters[1]).ToList();
            if (spellsData.Count > 0)
            {
                var embedBuilderService = provider.GetRequiredService<EmbedBuilderService>();

                return new(embedBuilderService, spellsData, parameters[1], selectedPageIndex);
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
        return _data.Select(x => $"- {(x.GetNeededLevel() == 0 ? string.Empty : $"{BotTranslations.ShortLevel}{x.GetNeededLevel()}")} {Formatter.Bold(x.Name)} ({x.Id})");
    }

    protected override DiscordSelectComponent SelectBuilder()
    {
        return SpellComponentsBuilder.SpellsSelectBuilder(0, _data);
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
