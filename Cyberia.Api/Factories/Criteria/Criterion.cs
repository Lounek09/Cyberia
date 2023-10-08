namespace Cyberia.Api.Factories.Criteria
{
    public abstract record Criterion(string Id, char Operator)
    {
        protected string GetOperatorDescriptionName()
        {
            return Operator switch
            {
                '=' => "Equal",
                '!' => "Different",
                '>' => "Superior",
                '<' => "Inferior",
                '~' => "SoftEqual",
                _ => Operator.ToString(),
            };
        }

        protected abstract string GetDescriptionName();

        protected Description GetDescription(params object[] parameters)
        {
            string descriptionName = GetDescriptionName();
            string[] strParameters = Array.ConvertAll(parameters, x => x.ToString() ?? string.Empty);

            string? descriptionValue = Resources.ResourceManager.GetString(descriptionName);
            if (descriptionValue is null)
            {
                DofusApi.Instance.Log.Warning("No translation for {descriptionName}, {raw}", descriptionName, $"{Id}{Operator}{string.Join(',', strParameters)}");
                return new($"{Id} {Operator} #1", strParameters);
            }

            return new(descriptionValue, strParameters);
        }
    }
}
