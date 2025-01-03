﻿using DSharpPlus.Commands.Processors.SlashCommands.Localization;

using System.Collections.ObjectModel;

namespace Cyberia.Salamandra.Commands.Dofus.Breed;

public sealed class BreedInteractionLocalizer : InteractionLocalizer
{
    public const string CommandName = "class";
    public const string CommandDescription = "Return the information of a class from its name.";

    public const string BreedId_ParameterName = "name";
    public const string BreedId_ParameterDescription = "Name of the class.";

    public const string Gladiatrool_ParameterName = "gladiatrool";
    public const string Gladiatrool_ParameterDescription = "Display the gladiatrool information.";

    protected override IReadOnlyDictionary<DiscordLocale, string> InternalTranslate(string fullSymbolName)
    {
        if (fullSymbolName.Equals(CommandName + c_name))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, CommandName },
                { DiscordLocale.en_GB, CommandName },
                { DiscordLocale.fr, "classe" }
            };
        }

        if (fullSymbolName.Equals(CommandName + c_description))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, CommandDescription },
                { DiscordLocale.en_GB, CommandDescription },
                { DiscordLocale.fr, "Retourne les informations d'une classe à partir de son nom." }
            };
        }

        if (fullSymbolName.Equals(CommandName + c_parameters + BreedId_ParameterName + c_name))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, BreedId_ParameterName },
                { DiscordLocale.en_GB, BreedId_ParameterName },
                { DiscordLocale.fr, "nom" }
            };
        }

        if (fullSymbolName.Equals(CommandName + c_parameters + BreedId_ParameterName + c_description))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, BreedId_ParameterDescription },
                { DiscordLocale.en_GB, BreedId_ParameterDescription },
                { DiscordLocale.fr, "Nom de la classe." }
            };
        }

        if (fullSymbolName.Equals(CommandName + c_parameters + Gladiatrool_ParameterName + c_name))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, Gladiatrool_ParameterName },
                { DiscordLocale.en_GB, Gladiatrool_ParameterName },
                { DiscordLocale.fr, "gladiatrool" }
            };
        }

        if (fullSymbolName.Equals(CommandName + c_parameters + Gladiatrool_ParameterName + c_description))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, Gladiatrool_ParameterDescription },
                { DiscordLocale.en_GB, Gladiatrool_ParameterDescription },
                { DiscordLocale.fr, "Affiche les informations du gladiatrool." }
            };
        }

        return ReadOnlyDictionary<DiscordLocale, string>.Empty;
    }
}
