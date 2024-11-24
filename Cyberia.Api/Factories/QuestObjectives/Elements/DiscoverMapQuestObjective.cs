using Cyberia.Api.Data.Quests;

using System.Globalization;

namespace Cyberia.Api.Factories.QuestObjectives.Elements;

public sealed record DiscoverMapQuestObjective : QuestObjective
{
    public LocalizedString MapDescription { get; init; }

    private DiscoverMapQuestObjective(QuestObjectiveData questObjectiveData, LocalizedString mapDescription)
        : base(questObjectiveData)
    {
        MapDescription = mapDescription;
    }

    internal static DiscoverMapQuestObjective? Create(QuestObjectiveData questObjectiveData)
    {
        var parameters = questObjectiveData.Parameters;
        if (parameters.Count > 0)
        {
            return new(questObjectiveData, parameters[0]);
        }

        return null;
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, MapDescription.ToString(culture));
    }
}
