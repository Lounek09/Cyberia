using Cyberia.Langzilla;
using Cyberia.Langzilla.Enums;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

using System.Text;

namespace Cyberia.Salamandra.Commands.Data.Cytrus;

public sealed class LangsMessageBuilder : ICustomMessageBuilder
{
    public const string PacketHeader = "LANG";
    public const int PacketVersion = 1;

    private readonly LangType _type;
    private readonly LangLanguage _language;
    private readonly LangRepository _langRepository;

    public LangsMessageBuilder(LangType type, LangLanguage language)
    {
        _type = type;
        _language = language;
        _langRepository = LangsWatcher.LangRepositories[(type, language)];
    }

    public static LangsMessageBuilder? Create(int version, string[] parameters)
    {
        if (version == PacketVersion &&
            parameters.Length > 1 &&
            Enum.TryParse(parameters[0], out LangType langType) &&
            Enum.TryParse(parameters[1], out LangLanguage language))
        {
            return new LangsMessageBuilder(langType, language);
        }

        return null;
    }

    public static string GetPacket(LangType langType, LangLanguage language)
    {
        return InteractionManager.ComponentPacketBuilder(PacketHeader, PacketVersion, langType, language);
    }

    public async Task<T> GetMessageAsync<T>() where T : IDiscordMessageBuilder, new()
    {
        var message = new T()
            .AddEmbed(await EmbedBuilder())
            .AddComponents(TypeSelectBuilder())
            .AddComponents(LanguageSelectBuilder());

        return (T)message; 
    }

    private Task<DiscordEmbedBuilder> EmbedBuilder()
    {
        var type = _type.ToStringFast();
        var language = _language.ToStringFast();

        var embed = EmbedManager.CreateEmbedBuilder(EmbedCategory.Tools, "Langs")
            .WithTitle($"Langs {type} in {language}");

        if (_langRepository.Langs.Count > 0)
        {
            StringBuilder descriptionBuilder = new();

            descriptionBuilder.Append("Last modification : ");
            descriptionBuilder.Append(_langRepository.LastChange.ToLocalTime().ToString("yyyy-mm-dd HH:mmzzz"));
            descriptionBuilder.Append('\n');
            descriptionBuilder.Append(Formatter.MaskedUrl(Formatter.Bold(_langRepository.VersionFileName), new Uri(LangsWatcher.BaseUrl + _langRepository.VersionFileRoute)));
            descriptionBuilder.Append('\n');

            foreach (var langData in _langRepository.Langs)
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
            InteractionManager.SelectComponentPacketBuilder(0),
            "Select a type to display",
            Enum.GetValues<LangType>()
                .Select(x => new DiscordSelectComponentOption(x.ToStringFast(), GetPacket(x, _language), isDefault: x == _type)));
    }

    private DiscordSelectComponent LanguageSelectBuilder()
    {
        return new DiscordSelectComponent(
            InteractionManager.SelectComponentPacketBuilder(1),
            "Select a language to display",
            Enum.GetValues<LangLanguage>()
                .Select(x => new DiscordSelectComponentOption(x.ToStringFast(), GetPacket(_type, x), isDefault: x == _language)));
    }
}
