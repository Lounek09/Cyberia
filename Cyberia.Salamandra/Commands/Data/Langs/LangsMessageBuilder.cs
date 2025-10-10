using Cyberia.Langzilla;
using Cyberia.Langzilla.Enums;
using Cyberia.Langzilla.Models;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Formatters;
using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.Entities;

using Microsoft.Extensions.DependencyInjection;

using System.Globalization;
using System.Text;

namespace Cyberia.Salamandra.Commands.Data.Langs;

public sealed class LangsMessageBuilder : ICustomMessageBuilder
{
    public const string PacketHeader = "LANG";
    public const int PacketVersion = 1;

    private readonly IEmbedBuilderService _embedBuilderService;
    private readonly LangsRepository _repository;
    private readonly CultureInfo? _culture;

    public LangsMessageBuilder(IEmbedBuilderService embedBuilderService, LangsRepository repository, CultureInfo? culture)
    {
        _embedBuilderService = embedBuilderService;
        _repository = repository;
        _culture = culture;
    }

    public static LangsMessageBuilder? Create(IServiceProvider provider, int version, CultureInfo? culture, params ReadOnlySpan<string> parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 1 &&
            Enum.TryParse(parameters[0], out LangType type) &&
            Enum.TryParse(parameters[1], out Language language))
        {
            var embedBuilderService = provider.GetRequiredService<IEmbedBuilderService>();
            var langsWatcher = provider.GetRequiredService<ILangsWatcher>();

            LangsIdentifier identifier = new(type, language);
            var repository = langsWatcher.GetRepository(identifier);

            return new LangsMessageBuilder(embedBuilderService, repository, culture);
        }

        return null;
    }

    public static string GetPacket(LangType langType, Language language)
    {
        return PacketFormatter.Action(PacketHeader, PacketVersion, langType, language);
    }

    public async Task<T> BuildAsync<T>() where T : IDiscordMessageBuilder, new()
    {
        var message = new T()
            .AddEmbed(await EmbedBuilder())
            .AddActionRowComponent(TypeSelectBuilder())
            .AddActionRowComponent(LanguageSelectBuilder());

        return (T)message;
    }

    private Task<DiscordEmbedBuilder> EmbedBuilder()
    {
        var type = _repository.Type.ToStringFast();
        var language = _repository.Language.ToStringFast();

        var embed = _embedBuilderService.CreateBaseEmbedBuilder(EmbedCategory.Tools, "Langs", _culture)
            .WithTitle($"{type} langs in {language}");

        if (_repository.Langs.Count > 0)
        {
            //TODO: One other lang will exceed the max description size
            StringBuilder descriptionBuilder = new(Constant.MaxEmbedDescriptionSize);

            descriptionBuilder.Append("Last modification: ");
            descriptionBuilder.Append(_repository.LastChange.ToLocalTime().ToString("yyyy-MM-dd HH:mmzzz"));
            descriptionBuilder.Append('\n');
            descriptionBuilder.Append(Formatter.MaskedUrl(Formatter.Bold(_repository.VersionFileName), new Uri(LangsWatcher.BaseUrl + _repository.VersionFileRoute)));
            descriptionBuilder.Append('\n').Append('\n');

            foreach (var langData in _repository.Langs)
            {
                descriptionBuilder.Append("- ");
                descriptionBuilder.Append(Formatter.MaskedUrl(langData.Name, new Uri(LangsWatcher.BaseUrl + langData.FileRoute)));
                descriptionBuilder.Append(' ');
                descriptionBuilder.Append(langData.Version);
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
            Enum.GetValues<Language>()
                .Select(x => new DiscordSelectComponentOption(x.ToStringFast(), GetPacket(_repository.Type, x), isDefault: x == _repository.Language)));
    }
}
