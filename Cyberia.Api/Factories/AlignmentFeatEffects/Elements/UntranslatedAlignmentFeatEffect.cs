namespace Cyberia.Api.Factories.AlignmentFeatEffects;

public sealed record UntranslatedAlignmentFeatEffect : AlignmentFeatEffect
{
    public IReadOnlyList<int> Parameters { get; init; }

    internal UntranslatedAlignmentFeatEffect(int id, IReadOnlyList<int> parameters)
        : base(id)
    {
        Parameters = parameters;
    }

    public override DescriptionString GetDescription()
    {
        return GetDescription(Parameters.ToArray());
    }
}
