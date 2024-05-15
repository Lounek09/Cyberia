using Cyberia.Api.Data.Emotes;

namespace Cyberia.Api.Factories.Criteria;

public sealed record EmoteCriterion : Criterion, ICriterion
{
    public int EmoteId { get; init; }

    public EmoteCriterion(string id, char @operator, int emoteId)
        : base(id, @operator)
    {
        EmoteId = emoteId;
    }

    internal static EmoteCriterion? Create(string id, char @operator, params string[] parameters)
    {
        if (parameters.Length > 0 && int.TryParse(parameters[0], out var emoteId))
        {
            return new(id, @operator, emoteId);
        }

        return null;
    }

    public EmoteData? GetEmoteData()
    {
        return DofusApi.Datacenter.EmotesRepository.GetEmoteById(EmoteId);
    }

    protected override string GetDescriptionName()
    {
        return $"Criterion.Emote.{GetOperatorDescriptionName()}";
    }

    public Description GetDescription()
    {
        var emoteName = DofusApi.Datacenter.EmotesRepository.GetEmoteNameById(EmoteId);

        return GetDescription(emoteName);
    }
}
