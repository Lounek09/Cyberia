using Cyberia.Api.Data.Quests;
using Cyberia.Api.Factories.QuestObjectives.Elements;
using Cyberia.Langzilla.Enums;

using System.Globalization;
using System.Text;

namespace Cyberia.Api.Factories.QuestObjectives;

/// <inheritdoc cref="IQuestObjective"/>
public abstract record QuestObjective : IQuestObjective
{
    public QuestObjectiveData QuestObjectiveData { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="QuestObjective"/> record.
    /// </summary>
    /// <param name="questObjectiveData">The data of the quest objective.</param>
    protected QuestObjective(QuestObjectiveData questObjectiveData)
    {
        QuestObjectiveData = questObjectiveData;
    }

    public DescriptionString GetDescription(Language language)
    {
        return GetDescription(language.ToCulture());
    }

    public abstract DescriptionString GetDescription(CultureInfo? culture = null);

    /// <inheritdoc cref="IQuestObjective.GetDescription"/>
    protected DescriptionString GetDescription<T>(CultureInfo? culture, T parameter)
    {
        return GetDescription(culture, parameter?.ToString() ?? string.Empty);
    }

    /// <inheritdoc cref="IQuestObjective.GetDescription"/>
    protected DescriptionString GetDescription<T0, T1>(CultureInfo? culture, T0 parameter0, T1 parameter1)
    {
        return GetDescription(culture,
            parameter0?.ToString() ?? string.Empty,
            parameter1?.ToString() ?? string.Empty);
    }

    /// <inheritdoc cref="IQuestObjective.GetDescription"/>
    protected DescriptionString GetDescription<T0, T1, T2>(CultureInfo? culture, T0 parameter0, T1 parameter1, T2 parameter2)
    {
        return GetDescription(culture,
            parameter0?.ToString() ?? string.Empty,
            parameter1?.ToString() ?? string.Empty,
            parameter2?.ToString() ?? string.Empty);
    }

    /// <inheritdoc cref="IQuestObjective.GetDescription"/>
    protected DescriptionString GetDescription(CultureInfo? culture, params IReadOnlyList<string> parameters)
    {
        var questObjectiveTypeData = QuestObjectiveData.GetQuestObjectiveTypeData();
        if (questObjectiveTypeData is null)
        {
            if (this is not UntranslatedQuestObjective)
            {
                Log.Warning("Unknown QuestObjectiveTypeData {@QuestObjective}", this);
            }

            return new DescriptionString(Translation.Get<ApiTranslations>("QuestObjectiveType.Unknown", culture),
            QuestObjectiveData.QuestObjectiveTypeId.ToString(),
            string.Join(',', QuestObjectiveData.Parameters));
        }

        StringBuilder builder = new(questObjectiveTypeData.Description.ToString(culture));

        var coordinate = QuestObjectiveData.GetCoordinate();
        if (!string.IsNullOrEmpty(coordinate))
        {
            builder.Append(" - ");
            builder.Append(coordinate);
        }

        return new DescriptionString(builder.ToString(), parameters);
    }
}
