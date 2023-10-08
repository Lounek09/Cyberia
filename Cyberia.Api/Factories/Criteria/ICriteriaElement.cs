namespace Cyberia.Api.Factories.Criteria
{
    public interface ICriteriaElement
    {

    }

    public interface ICriterion : ICriteriaElement
    {
        string Id { get; init; }

        char Operator { get; init; }

        Description GetDescription();
    }

    public interface ICriterion<T> : ICriterion where T : ICriterion<T>
    {
        public static abstract T? Create(string id, char @operator, params string[] parameters);
    }
}
