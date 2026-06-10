using DSharpPlus.Commands.Processors.SlashCommands.Localization;

using System.Collections.ObjectModel;

namespace Cyberia.Salamandra.Commands.Dofus.Map;

public sealed class MapInteractionLocalizer : InteractionLocalizer
{
    public const string CommandName = "map";
    public const string CommandDescription = "Return the information of a map.";

    public const string Id_CommandName = "id";
    public const string Id_CommandDescription = "Return the information of a map from its id.";

    public const string Id_Id_ParameterName = "id";
    public const string Id_Id_ParameterDescription = "Id of the map.";

    public const string Coordinates_CommandName = "coordinates";
    public const string Coordinates_CommandDescription = "Return a list of maps from their coordinates.";

    public const string Coordinates_X_ParameterName = "x";
    public const string Coordinates_X_ParameterDescription = "X coordinate of the map.";

    public const string Coordinates_Y_ParameterName = "y";
    public const string Coordinates_Y_ParameterDescription = "Y coordinate of the map.";

    public const string SubArea_CommandName = "sub-area";
    public const string SubArea_CommandDescription = "Return a list of maps from their sub-area.";

    public const string SubArea_Value_ParameterName = "name";
    public const string SubArea_Value_ParameterDescription = "Name of the sub-area.";

    public const string Area_CommandName = "area";
    public const string Area_CommandDescription = "Return a list of maps from their area.";

    public const string Area_Value_ParameterName = "name";
    public const string Area_Value_ParameterDescription = "Name of the area.";

    protected override IReadOnlyDictionary<DiscordLocale, string> InternalTranslate(string fullSymbolName)
    {
        if (fullSymbolName.Equals(CommandName + c_name))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, CommandName },
                { DiscordLocale.en_GB, CommandName },
                { DiscordLocale.fr, "map" }
            };
        }

        if (fullSymbolName.Equals(CommandName + c_description))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, CommandDescription },
                { DiscordLocale.en_GB, CommandDescription },
                { DiscordLocale.fr, "Retourne les informations d'une map." }
            };
        }

        if (fullSymbolName.Equals(CommandName + "." + Id_CommandName + c_name))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, Id_CommandName },
                { DiscordLocale.en_GB, Id_CommandName },
                { DiscordLocale.fr, "nom" }
            };
        }

        if (fullSymbolName.Equals(CommandName + "." + Id_CommandName + c_description))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, Id_CommandDescription },
                { DiscordLocale.en_GB, Id_CommandDescription },
                { DiscordLocale.fr, "Retourne les informations d'une map à partir de son id." }
            };
        }

        if (fullSymbolName.Equals(CommandName + "." + Id_CommandName + c_parameters + Id_Id_ParameterName + c_name))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, Id_Id_ParameterName },
                { DiscordLocale.en_GB, Id_Id_ParameterName },
                { DiscordLocale.fr, "id" }
            };
        }

        if (fullSymbolName.Equals(CommandName + "." + Id_CommandName + c_parameters + Id_Id_ParameterName + c_description))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, Id_Id_ParameterDescription },
                { DiscordLocale.en_GB, Id_Id_ParameterDescription },
                { DiscordLocale.fr, "Id de la map." }
            };
        }

        if (fullSymbolName.Equals(CommandName + "." + Coordinates_CommandName + c_name))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, Coordinates_CommandName },
                { DiscordLocale.en_GB, Coordinates_CommandName },
                { DiscordLocale.fr, "coordonnees" }
            };
        }

        if (fullSymbolName.Equals(CommandName + "." + Coordinates_CommandName + c_description))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, Coordinates_CommandDescription },
                { DiscordLocale.en_GB, Coordinates_CommandDescription },
                { DiscordLocale.fr, "Retourne une liste de maps à partir de leurs coordonnées." }
            };
        }

        if (fullSymbolName.Equals(CommandName + "." + Coordinates_CommandName + c_parameters + Coordinates_X_ParameterName + c_name))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, Coordinates_X_ParameterName },
                { DiscordLocale.en_GB, Coordinates_X_ParameterName },
                { DiscordLocale.fr, "x" }
            };
        }

        if (fullSymbolName.Equals(CommandName + "." + Coordinates_CommandName + c_parameters + Coordinates_X_ParameterName + c_description))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, Coordinates_X_ParameterDescription },
                { DiscordLocale.en_GB, Coordinates_X_ParameterDescription },
                { DiscordLocale.fr, "Coordonnée X de la map." }
            };
        }

        if (fullSymbolName.Equals(CommandName + "." + Coordinates_CommandName + c_parameters + Coordinates_Y_ParameterName + c_name))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, Coordinates_Y_ParameterName },
                { DiscordLocale.en_GB, Coordinates_Y_ParameterName },
                { DiscordLocale.fr, "y" }
            };
        }

        if (fullSymbolName.Equals(CommandName + "." + Coordinates_CommandName + c_parameters + Coordinates_Y_ParameterName + c_description))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, Coordinates_Y_ParameterDescription },
                { DiscordLocale.en_GB, Coordinates_Y_ParameterDescription },
                { DiscordLocale.fr, "Coordonnée Y de la map." }
            };
        }

        if (fullSymbolName.Equals(CommandName + "." + SubArea_CommandName + c_name))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, SubArea_CommandName },
                { DiscordLocale.en_GB, SubArea_CommandName },
                { DiscordLocale.fr, "sous-zone" }
            };
        }

        if (fullSymbolName.Equals(CommandName + "." + SubArea_CommandName + c_description))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, SubArea_CommandDescription },
                { DiscordLocale.en_GB, SubArea_CommandDescription },
                { DiscordLocale.fr, "Retourne une liste de maps à partir de leur sous-zone." }
            };
        }

        if (fullSymbolName.Equals(CommandName + "." + SubArea_CommandName + c_parameters + SubArea_Value_ParameterName + c_name))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, SubArea_Value_ParameterName },
                { DiscordLocale.en_GB, SubArea_Value_ParameterName },
                { DiscordLocale.fr, "nom" }
            };
        }

        if (fullSymbolName.Equals(CommandName + "." + SubArea_CommandName + c_parameters + SubArea_Value_ParameterName + c_description))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, SubArea_Value_ParameterDescription },
                { DiscordLocale.en_GB, SubArea_Value_ParameterDescription },
                { DiscordLocale.fr, "Nom de la sous-zone." }
            };
        }

        if (fullSymbolName.Equals(CommandName + "." + Area_CommandName + c_name))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, Area_CommandName },
                { DiscordLocale.en_GB, Area_CommandName },
                { DiscordLocale.fr, "zone" }
            };
        }

        if (fullSymbolName.Equals(CommandName + "." + Area_CommandName + c_description))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, Area_CommandDescription },
                { DiscordLocale.en_GB, Area_CommandDescription },
                { DiscordLocale.fr, "Retourne une liste de maps à partir de leur zone." }
            };
        }

        if (fullSymbolName.Equals(CommandName + "." + Area_CommandName + c_parameters + Area_Value_ParameterName + c_name))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, Area_Value_ParameterName },
                { DiscordLocale.en_GB, Area_Value_ParameterName },
                { DiscordLocale.fr, "nom" }
            };
        }

        if (fullSymbolName.Equals(CommandName + "." + Area_CommandName + c_parameters + Area_Value_ParameterName + c_description))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, Area_Value_ParameterDescription },
                { DiscordLocale.en_GB, Area_Value_ParameterDescription },
                { DiscordLocale.fr, "Nom de la zone." }
            };
        }

        return ReadOnlyDictionary<DiscordLocale, string>.Empty;
    }
}
