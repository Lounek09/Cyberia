using DSharpPlus.Commands.Processors.SlashCommands.Localization;

using System.Collections.ObjectModel;

namespace Cyberia.Salamandra.Commands.Dofus.Craft;

public sealed class CraftInteractionLocalizer : InteractionLocalizer
{
    public const string CommandName = "craft";
    public const string CommandDescription = "Allows you to calculate the resources needed to craft an item.";

    public const string Value_ParameterName = "name";
    public const string Value_ParameterDescription = "Name of the item to craft.";

    public const string Quantity_ParameterName = "quantity";
    public const string Quantity_ParameterDescription = "quantity to craft.";

    protected override IReadOnlyDictionary<DiscordLocale, string> InternalTranslate(string fullSymbolName)
    {
        if (fullSymbolName.Equals(CommandName + c_name))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, CommandName },
                { DiscordLocale.en_GB, CommandName },
                { DiscordLocale.fr, CommandName }
            };
        }

        if (fullSymbolName.Equals(CommandName + c_description))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, CommandDescription },
                { DiscordLocale.en_GB, CommandDescription },
                { DiscordLocale.fr, "Permet de calculer les ressources nécessaires pour craft un item." }
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
                { DiscordLocale.fr, "Nom de l'item à craft." }
            };
        }

        if (fullSymbolName.Equals(CommandName + c_parameters + Quantity_ParameterName + c_name))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, Quantity_ParameterName },
                { DiscordLocale.en_GB, Quantity_ParameterName },
                { DiscordLocale.fr, "quantite" }
            };
        }

        if (fullSymbolName.Equals(CommandName + c_parameters + Quantity_ParameterName + c_description))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, Quantity_ParameterDescription },
                { DiscordLocale.en_GB, Quantity_ParameterDescription },
                { DiscordLocale.fr, "Quantité à craft." }
            };
        }

        return ReadOnlyDictionary<DiscordLocale, string>.Empty;
    }
}
