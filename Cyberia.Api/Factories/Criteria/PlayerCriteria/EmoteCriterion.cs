using Cyberia.Api.Data;

namespace Cyberia.Api.Factories.Criteria.PlayerCriteria
{
    public sealed record EmoteCriterion : Criterion, ICriterion<EmoteCriterion>
    {
        public int EmoteId { get; init; }

        public EmoteCriterion(string id, char @operator, int emoteId) :
            base(id, @operator)
        {
            EmoteId = emoteId;
        }

        public static EmoteCriterion? Create(string id, char @operator, params string[] parameters)
        {
            if (parameters.Length > 0 && int.TryParse(parameters[0], out int emoteId))
            {
                return new(id, @operator, emoteId);
            }

            return null;
        }

        public EmoteData? GetEmoteData()
        {
            return DofusApi.Datacenter.EmotesData.GetEmoteById(EmoteId);
        }

        protected override string GetDescriptionName()
        {
            return $"Criterion.Emote.{GetOperatorDescriptionName()}";
        }

        public Description GetDescription()
        {
            string emoteName = DofusApi.Datacenter.EmotesData.GetEmoteNameById(EmoteId);

            return GetDescription(emoteName);
        }
    }
}
