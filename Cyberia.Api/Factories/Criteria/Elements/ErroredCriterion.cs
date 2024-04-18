namespace Cyberia.Api.Factories.Criteria;

public sealed record ErroredCriterion : Criterion, ICriterion
{
    public string CompressedCriterion { get; init; }

    private ErroredCriterion(string compressedCriterion)
        : base(string.Empty, char.MinValue)
    {
        CompressedCriterion = compressedCriterion;
    }

    internal static ErroredCriterion Create(string compressedCriterion)
    {
        return new(compressedCriterion);
    }

    protected override string GetDescriptionName()
    {
        return "Criterion.Errored";
    }

    public Description GetDescription()
    {
        return GetDescription(CompressedCriterion);
    }
}
