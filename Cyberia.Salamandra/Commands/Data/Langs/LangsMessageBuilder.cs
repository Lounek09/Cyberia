using Cyberia.Langzilla;
using Cyberia.Langzilla.Enums;
using Cyberia.Langzilla.Models;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Formatters;
using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.Entities;

using Microsoft.Extensions.DependencyInjection;

using System.Text;

namespace Cyberia.Salamandra.Commands.Data.Langs;

public sealed class LangsMessageBuilder : ICustomMessageBuilder
{
    public const string PacketHeader = "LANG";
    public const int PacketVersion = 1;

    private readonly EmbedBuilderService _embedBuilderService;
    private readonly LangsRepository _repository;

    public LangsMessageBuilder(EmbedBuilderService embedBuilderService, LangsRepository repository)
    {
        _embedBuilderService = embedBuilderService;
        _repository = repository;
    }

    public static LangsMessageBuilder? Create(IServiceProvider provider, int version, string[] parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 1 &&
            Enum.TryParse(parameters[0], out LangType langType) &&
            Enum.TryParse(parameters[1], out LangLanguage language))
        {
            var embedBuilderService = provider.GetRequiredService<EmbedBuilderService>();
            var langsWatcher = provider.GetRequiredService<LangsWatcher>();
            var repository = langsWatcher.GetRepository(langType, language);

            return new LangsMessageBuilder(embedBuilderService, repository);
        }

        return null;
    }

    public static string GetPacket(LangType langType, LangLanguage language)
    {
        return PacketFormatter.Action(PacketHeader, PacketVersion, langType, language);
    }

    public async Task<T> BuildAsync<T>() where T : IDiscordMessageBuilder, new()
    {
        var message = new T()
            .AddEmbed(await EmbedBuilder())
            .AddComponents(TypeSelectBuilder())
            .AddComponents(LanguageSelectBuilder());

        return (T)message; 
    }

    private Task<DiscordEmbedBuilder> EmbedBuilder()
    {
        var type = _repository.Type.ToStringFast();
        var language = _repository.Language.ToStringFast();

        var embed = _embedBuilderService.CreateEmbedBuilder(EmbedCategory.Tools, "Langs")
            .WithTitle($"Langs {type} in {language}");

        if (_repository.Langs.Count > 0)
        {
            StringBuilder descriptionBuilder = new();

            descriptionBuilder.Append("Last modification : ");
            descriptionBuilder.Append(_repository.LastChange.ToLocalTime().ToString("yyyy-MM-dd HH:mmzzz"));
            descriptionBuilder.Append('\n');
            descriptionBuilder.Append(Formatter.MaskedUrl(Formatter.Bold(_repository.VersionFileName), new Uri(LangsWatcher.BaseUrl + _repository.VersionFileRoute)));
            descriptionBuilder.Append('\n');

            foreach (var langData in _repository.Langs)
            {
                descriptionBuilder.Append("- ");
                descriptionBuilder.Append(Formatter.MaskedUrl(langData.Name, new Uri(LangsWatcher.BaseUrl + langData.FileRoute)));
                descriptionBuilder.Append(' ');
                descriptionBuilder.Append(Formatter.InlineCode(langData.Version.ToString()));
                descriptionBuilder.Append('\n');
            }

            embed.WithDescription(descriptionBuilder.ToString());
        }
        else
        {
            embed.WithDescription($"No {Formatter.Bold(type)} lang in {Formatter.Bold(language)} has been found.");
        }

        return Task.FromResult(embed);
    }

    private DiscordSelectComponent TypeSelectBuilder()
    {
        return new DiscordSelectComponent(
            PacketFormatter.Select(0),
            "Select a type to display",
            Enum.GetValues<LangType>()
                .Select(x => new DiscordSelectComponentOption(x.ToStringFast(), GetPacket(x, _repository.Language), isDefault: x == _repository.Type)));
    }

    private DiscordSelectComponent LanguageSelectBuilder()
    {
        return new DiscordSelectComponent(
            PacketFormatter.Select(1),
            "Select a language to display",
            Enum.GetValues<LangLanguage>()
                .Select(x => new DiscordSelectComponentOption(x.ToStringFast(), GetPacket(_repository.Type, x), isDefault: x == _repository.Language)));
    }
}
