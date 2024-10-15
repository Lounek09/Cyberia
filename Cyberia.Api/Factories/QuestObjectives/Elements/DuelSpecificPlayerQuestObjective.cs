using Cyberia.Api.Data.Quests;

using System.Globalization;

namespace Cyberia.Api.Factories.QuestObjectives.Elements;

public sealed record DuelSpecificPlayerQuestObjective : QuestObjective
{
    public LocalizedString SpecificPlayer { get; init; }

    private DuelSpecificPlayerQuestObjective(QuestObjectiveData questObjectiveData, LocalizedString specificPlayer)
        : base(questObjectiveData)
    {
        SpecificPlayer = specificPlayer;
    }

    internal static DuelSpecificPlayerQuestObjective? Create(QuestObjectiveData questObjectiveData)
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
        return GetDescription(culture, SpecificPlayer);
    }
}
