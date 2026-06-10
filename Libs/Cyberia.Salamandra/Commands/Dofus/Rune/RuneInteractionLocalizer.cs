using DSharpPlus.Commands.Processors.SlashCommands.Localization;

using System.Collections.ObjectModel;

namespace Cyberia.Salamandra.Commands.Dofus.Rune;

public sealed class RuneInteractionLocalizer : InteractionLocalizer
{
    public const string CommandName = "rune";
    public const string CommandDescription = "Allow you to calculate the runes obtained during a breakage.";

    public const string Item_CommandName = "item";
    public const string Item_CommandDescription = "Allows you to calculate the number of runes obtainable from an item.";

    public const string Item_Value_ParameterName = "name";
    public const string Item_Value_ParameterDescription = "Name of the item to break.";

    public const string Item_Quantity_ParameterName = "quantity";
    public const string Item_Quantity_ParameterDescription = "Quantity of item to break.";

    public const string Stat_CommandName = "stat";
    public const string Stat_CommandDescription = "Allows you to calculate the number of runes obtainable from a stat on an item.";

    public const string Stat_ItemLvl_ParameterName = "level";
    public const string Stat_ItemLvl_ParameterDescription = "Level of the item.";

    public const string Stat_StatAmount_ParameterName = "amount";
    public const string Stat_StatAmount_ParameterDescription = "Amount of stat.";

    public const string Stat_runeName_ParameterName = "rune";
    public const string Stat_runeName_ParameterDescription = "Name of the rune.";

    protected override IReadOnlyDictionary<DiscordLocale, string> InternalTranslate(string fullSymbolName)
    {
        if (fullSymbolName.Equals(CommandName + c_name))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, CommandName },
                { DiscordLocale.en_GB, CommandName },
                { DiscordLocale.fr, "rune" }
            };
        }

        if (fullSymbolName.Equals(CommandName + c_description))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, CommandDescription },
                { DiscordLocale.en_GB, CommandDescription },
                { DiscordLocale.fr, "Permet de calculer les runes obtenues lors d'un brisage." }
            };
        }

        if (fullSymbolName.Equals(CommandName + "." + Item_CommandName + c_name))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, Item_CommandName },
                { DiscordLocale.en_GB, Item_CommandName },
                { DiscordLocale.fr, "item" }
            };
        }

        if (fullSymbolName.Equals(CommandName + "." + Item_CommandName + c_description))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, Item_CommandDescription },
                { DiscordLocale.en_GB, Item_CommandDescription },
                { DiscordLocale.fr, "Permet de calculer le nombre de rune obtenable depuis un item." }
            };
        }

        if (fullSymbolName.Equals(CommandName + "." + Item_CommandName + c_parameters + Item_Value_ParameterName + c_name))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, Item_Value_ParameterName },
                { DiscordLocale.en_GB, Item_Value_ParameterName },
                { DiscordLocale.fr, "nom" }
            };
        }

        if (fullSymbolName.Equals(CommandName + "." + Item_CommandName + c_parameters + Item_Value_ParameterName + c_description))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, Item_Value_ParameterDescription },
                { DiscordLocale.en_GB, Item_Value_ParameterDescription },
                { DiscordLocale.fr, "Nom de l'item à briser" }
            };
        }

        if (fullSymbolName.Equals(CommandName + "." + Item_CommandName + c_parameters + Item_Quantity_ParameterName + c_name))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, Item_Quantity_ParameterName },
                { DiscordLocale.en_GB, Item_Quantity_ParameterName },
                { DiscordLocale.fr, "quantite" }
            };
        }

        if (fullSymbolName.Equals(CommandName + "." + Item_CommandName + c_parameters + Item_Quantity_ParameterName + c_description))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, Item_Quantity_ParameterDescription },
                { DiscordLocale.en_GB, Item_Quantity_ParameterDescription },
                { DiscordLocale.fr, "Quantité d'item à briser" }
            };
        }

        if (fullSymbolName.Equals(CommandName + "." + Stat_CommandName + c_name))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, Stat_CommandName },
                { DiscordLocale.en_GB, Stat_CommandName },
                { DiscordLocale.fr, "stat" }
            };
        }

        if (fullSymbolName.Equals(CommandName + "." + Stat_CommandName + c_description))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, Stat_CommandDescription },
                { DiscordLocale.en_GB, Stat_CommandDescription },
                { DiscordLocale.fr, "Permet de calculer le nombre de rune obtenable d'une stat sur un item." }
            };
        }

        if (fullSymbolName.Equals(CommandName + "." + Stat_CommandName + c_parameters + Stat_ItemLvl_ParameterName + c_name))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, Stat_ItemLvl_ParameterName },
                { DiscordLocale.en_GB, Stat_ItemLvl_ParameterName },
                { DiscordLocale.fr, "niveau" }
            };
        }

        if (fullSymbolName.Equals(CommandName + "." + Stat_CommandName + c_parameters + Stat_ItemLvl_ParameterName + c_description))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, Stat_ItemLvl_ParameterDescription },
                { DiscordLocale.en_GB, Stat_ItemLvl_ParameterDescription },
                { DiscordLocale.fr, "Niveau de l'item" }
            };
        }

        if (fullSymbolName.Equals(CommandName + "." + Stat_CommandName + c_parameters + Stat_StatAmount_ParameterName + c_name))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, Stat_StatAmount_ParameterName },
                { DiscordLocale.en_GB, Stat_StatAmount_ParameterName },
                { DiscordLocale.fr, "montant" }
            };
        }

        if (fullSymbolName.Equals(CommandName + "." + Stat_CommandName + c_parameters + Stat_StatAmount_ParameterName + c_description))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, Stat_StatAmount_ParameterDescription },
                { DiscordLocale.en_GB, Stat_StatAmount_ParameterDescription },
                { DiscordLocale.fr, "Montant de la stat" }
            };
        }

        if (fullSymbolName.Equals(CommandName + "." + Stat_CommandName + c_parameters + Stat_runeName_ParameterName + c_name))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, Stat_runeName_ParameterName },
                { DiscordLocale.en_GB, Stat_runeName_ParameterName },
                { DiscordLocale.fr, "rune" }
            };
        }

        if (fullSymbolName.Equals(CommandName + "." + Stat_CommandName + c_parameters + Stat_runeName_ParameterName + c_description))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, Stat_runeName_ParameterDescription },
                { DiscordLocale.en_GB, Stat_runeName_ParameterDescription },
                { DiscordLocale.fr, "Nom de la rune" }
            };
        }

        return ReadOnlyDictionary<DiscordLocale, string>.Empty;
    }
}
