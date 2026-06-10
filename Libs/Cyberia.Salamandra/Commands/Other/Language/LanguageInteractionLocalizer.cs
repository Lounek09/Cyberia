using DSharpPlus.Commands.Processors.SlashCommands.Localization;

using System.Collections.ObjectModel;

namespace Cyberia.Salamandra.Commands.Other.Language;

public sealed class LanguageInteractionLocalizer : InteractionLocalizer
{
    public const string CommandName = "language";
    public const string CommandDescription = "Allows you to set the language with which the bot will interact with you.";

    public const string Value_ParameterName = "name";
    public const string Value_ParameterDescription = "Name of the language.";

    protected override IReadOnlyDictionary<DiscordLocale, string> InternalTranslate(string fullSymbolName)
    {
        if (fullSymbolName.Equals(CommandName + c_name))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, CommandName },
                { DiscordLocale.en_GB, CommandName },
                { DiscordLocale.fr, "langue" }
            };
        }

        if (fullSymbolName.Equals(CommandName + c_description))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, CommandDescription },
                { DiscordLocale.en_GB, CommandDescription },
                { DiscordLocale.fr, "Permet de définir la langue avec laquelle le bot interagira avec vous." }
            };
        }

        if (fullSymbolName.Equals(CommandName + c_parameters + Value_ParameterName + c_name))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, Value_ParameterName },
                { DiscordLocale.en_GB, Value_ParameterName },
                { DiscordLocale.fr, "nom" }
            };
        }

        if (fullSymbolName.Equals(CommandName + c_parameters + Value_ParameterName + c_description))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, Value_ParameterDescription },
                { DiscordLocale.en_GB, Value_ParameterDescription },
                { DiscordLocale.fr, "Nom de la langue." }
            };
        }

        return ReadOnlyDictionary<DiscordLocale, string>.Empty;
    }
}
