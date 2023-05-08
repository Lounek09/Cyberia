namespace Cyberia.Api.Factories.Criteria.PlayerCriteria
{
    public static class EmoteCriterion
    {
        public static string? GetValue(char @operator, string[] values)
        {
            if (values.Length > 0 && int.TryParse(values[0], out int emoteId))
            {
                string emoteName = DofusApi.Instance.Datacenter.EmotesData.GetEmoteNameById(emoteId);

                return $"Attitude {@operator} {emoteName.Bold()}";
            }

            return null;
        }
    }
}
