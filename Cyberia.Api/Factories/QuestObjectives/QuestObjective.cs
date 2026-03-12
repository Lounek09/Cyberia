using Cyberia.Api.Data.Quests;
using Cyberia.Api.Factories.QuestObjectives.Elements;
using Cyberia.Langzilla.Primitives;

using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace Cyberia.Api.Factories.QuestObjectives;

/// <summary>
/// Represents an objective of a <see cref="QuestStepData"/>.
/// </summary>
public abstract record QuestObjective
{
    /// <summary>
    /// Gets the data of the quest objective.
    /// </summary>
    public QuestObjectiveData Data { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="QuestObjective"/> record.
    /// </summary>
    /// <param name="data">The data of the quest objective.</param>
    protected QuestObjective(QuestObjectiveData data)
    {
        Data = data;
    }

    /// <summary>
    /// Generates a human-readable description of the quest objective for the specified language.
    /// </summary>
    /// <param name="language">The language to generate the description for.</param>
    /// <returns>The <see cref="DescriptionString"/> object containing the description of the quest objective for the specified language.</returns>
    public DescriptionString GetDescription(Language language)
    {
        return GetDescription(language.ToCulture());
    }

    /// <summary>
    /// Generates a human-readable description of the quest objective for the specified culture.
    /// </summary>
    /// <param name="culture">The culture to generate the description for.</param>
    /// <returns>The <see cref="DescriptionString"/> object containing the description of the quest objective for the specified culture.</returns>
    [OverloadResolutionPriority(2)]
    public abstract DescriptionString GetDescription(CultureInfo? culture = null);

    /// <inheritdoc cref="GetDescription(CultureInfo)"/>
    protected DescriptionString GetDescription<T>(CultureInfo? culture, T parameter)
    {
        return GetDescription(culture, parameter?.ToString() ?? string.Empty);
    }

    /// <inheritdoc cref="GetDescription(CultureInfo)"/>
    protected DescriptionString GetDescription<T0, T1>(CultureInfo? culture, T0 parameter0, T1 parameter1)
    {
        return GetDescription(culture,
            parameter0?.ToString() ?? string.Empty,
            parameter1?.ToString() ?? string.Empty);
    }

    /// <inheritdoc cref="GetDescription(CultureInfo)"/>
    protected DescriptionString GetDescription<T0, T1, T2>(CultureInfo? culture, T0 parameter0, T1 parameter1, T2 parameter2)
    {
        return GetDescription(culture,
            parameter0?.ToString() ?? string.Empty,
            parameter1?.ToString() ?? string.Empty,
            parameter2?.ToString() ?? string.Empty);
    }

    /// <inheritdoc cref="GetDescription(CultureInfo)"/>
    protected DescriptionString GetDescription<T0, T1, T2, T3>(CultureInfo? culture, T0 parameter0, T1 parameter1, T2 parameter2, T3 parameter3)
    {
        return GetDescription(culture,
            parameter0?.ToString() ?? string.Empty,
            parameter1?.ToString() ?? string.Empty,
            parameter2?.ToString() ?? string.Empty,
            parameter3?.ToString() ?? string.Empty);
    }

    /// <inheritdoc cref="GetDescription(CultureInfo)"/>
    [OverloadResolutionPriority(1)]
    protected DescriptionString GetDescription(CultureInfo? culture, params IReadOnlyList<string> parameters)
    {
        var questObjectiveTypeData = Data.GetQuestObjectiveTypeData();
        if (questObjectiveTypeData is null)
        {
            if (this is not UntranslatedQuestObjective)
            {
                Log.Warning("Unknown QuestObjectiveTypeData {@QuestObjective}", this);
            }

            return new DescriptionString(Translation.Get<ApiTranslations>("QuestObjectiveType.Unknown", culture),
                Data.QuestObjectiveTypeId.ToString(), string.Join(',', Data.Parameters));
        }

        StringBuilder builder = new(questObjectiveTypeData.Description.ToString(culture));

        var coordinate = Data.GetCoordinate();
        if (!string.IsNullOrEmpty(coordinate))
        {
            builder.Append(" - ");
            builder.Append(coordinate);
        }

        return new DescriptionString(builder.ToString(), parameters.ToArray());
    }
}
