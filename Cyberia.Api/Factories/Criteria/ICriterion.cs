namespace Cyberia.Api.Factories.Criteria;

public interface ICriterion
    : ICriteriaElement
{
    string Id { get; init; }
    char Operator { get; init; }

    Description GetDescription();
}
