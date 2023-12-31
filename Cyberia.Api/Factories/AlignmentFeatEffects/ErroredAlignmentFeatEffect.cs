namespace Cyberia.Api.Factories.AlignmentFeatEffects;

public sealed record ErroredAlignmentFeatEffect : AlignmentFeatEffect, IAlignmentFeatEffect<ErroredAlignmentFeatEffect>
{
    public IReadOnlyCollection<int> Parameters { get; init; }

    private ErroredAlignmentFeatEffect(int id, IReadOnlyCollection<int> parameters)
        : base(id)
    {
        Parameters = parameters;
    }

    public static ErroredAlignmentFeatEffect Create(int id, params int[] parameters)
    {
        return new(id, parameters);
    }

    public Description GetDescription()
    {
        return GetDescription(Parameters.ToArray());
    }
}
