using DSharpPlus.Commands.Processors.SlashCommands.Localization;

using System.Collections.ObjectModel;

namespace Cyberia.Salamandra.Commands.Dofus.ItemSet;

public sealed class ItemSetInteractionLocalizer : InteractionLocalizer
{
    public const string CommandName = "set";
    public const string CommandDescription = "Return the information of a set from its name.";

    public const string Value_ParameterName = "name";
    public const string Value_ParameterDescription = "Name of the set.";

    protected override IReadOnlyDictionary<DiscordLocale, string> InternalTranslate(string fullSymbolName)
    {
        if (fullSymbolName.Equals(CommandName + c_name))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, CommandName },
                { DiscordLocale.en_GB, CommandName },
                { DiscordLocale.fr, "panoplie" }
            };
        }

        if (fullSymbolName.Equals(CommandName + c_description))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, CommandDescription },
                { DiscordLocale.en_GB, CommandDescription },
                { DiscordLocale.fr, "Retourne les informations d'une panoplie à partir de son nom." }
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
                { DiscordLocale.fr, "Nom de la panoplie." }
            };
        }

        return ReadOnlyDictionary<DiscordLocale, string>.Empty;
    }
}
