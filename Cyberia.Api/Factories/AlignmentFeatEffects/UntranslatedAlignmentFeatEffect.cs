namespace Cyberia.Api.Factories.AlignmentFeatEffects;

public sealed record UntranslatedAlignmentFeatEffect : AlignmentFeatEffect, IAlignmentFeatEffect<UntranslatedAlignmentFeatEffect>
{
    public IReadOnlyCollection<int> Parameters { get; init; }

    private UntranslatedAlignmentFeatEffect(int id, IReadOnlyCollection<int> parameters)
        : base(id)
    {
        Parameters = parameters;
    }

    public static UntranslatedAlignmentFeatEffect Create(int id, params int[] parameters)
    {
        return new(id, parameters);
    }

    public Description GetDescription()
    {
        return GetDescription(Parameters.ToArray());
    }
}
