using DSharpPlus.Commands.Processors.SlashCommands.Localization;

using System.Collections.ObjectModel;

namespace Cyberia.Salamandra.Commands.Dofus.Crit;

public sealed class CritInteractionLocalizer : InteractionLocalizer
{
    public const string CommandName = "crit";
    public const string CommandDescription = "Allows you to calculate your critical rate.";

    public const string Number_ParameterName = "number";
    public const string Number_ParameterDescription = "Your number of critical hit.";

    public const string TargetRate_ParameterName = "rate";
    public const string TargetRate_ParameterDescription = "Target critical rate.";

    public const string Agility_ParameterName = "agility";
    public const string Agility_ParameterDescription = "Your agility.";

    protected override IReadOnlyDictionary<DiscordLocale, string> InternalTranslate(string fullSymbolName)
    {
        if (fullSymbolName.Equals(CommandName + c_name))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, CommandName },
                { DiscordLocale.en_GB, CommandName },
                { DiscordLocale.fr, "crit" }
            };
        }

        if (fullSymbolName.Equals(CommandName + c_description))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, CommandDescription },
                { DiscordLocale.en_GB, CommandDescription },
                { DiscordLocale.fr, "Permet de calculer votre taux de coup critique." }
            };
        }

        if (fullSymbolName.Equals(CommandName + c_parameters + Number_ParameterName + c_name))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, Number_ParameterName },
                { DiscordLocale.en_GB, Number_ParameterName },
                { DiscordLocale.fr, "nombre" }
            };
        }

        if (fullSymbolName.Equals(CommandName + c_parameters + Number_ParameterName + c_description))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, Number_ParameterDescription },
                { DiscordLocale.en_GB, Number_ParameterDescription },
                { DiscordLocale.fr, "Votre nombre de coup critique." }
            };
        }

        if (fullSymbolName.Equals(CommandName + c_parameters + TargetRate_ParameterName + c_name))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, TargetRate_ParameterName },
                { DiscordLocale.en_GB, TargetRate_ParameterName },
                { DiscordLocale.fr, "taux" }
            };
        }

        if (fullSymbolName.Equals(CommandName + c_parameters + TargetRate_ParameterName + c_description))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, TargetRate_ParameterDescription },
                { DiscordLocale.en_GB, TargetRate_ParameterDescription },
                { DiscordLocale.fr, "Taux de coup critique cible." }
            };
        }

        if (fullSymbolName.Equals(CommandName + c_parameters + Agility_ParameterName + c_name))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, Agility_ParameterName },
                { DiscordLocale.en_GB, Agility_ParameterName },
                { DiscordLocale.fr, "agilite" }
            };
        }

        if (fullSymbolName.Equals(CommandName + c_parameters + Agility_ParameterName + c_description))
        {
            return new Dictionary<DiscordLocale, string>
            {
                { DiscordLocale.en_US, Agility_ParameterDescription },
                { DiscordLocale.en_GB, Agility_ParameterDescription },
                { DiscordLocale.fr, "Votre agilité." }
            };
        }

        return ReadOnlyDictionary<DiscordLocale, string>.Empty;
    }
}
