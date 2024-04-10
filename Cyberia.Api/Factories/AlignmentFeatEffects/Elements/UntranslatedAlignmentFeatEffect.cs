namespace Cyberia.Api.Factories.AlignmentFeatEffects;

public sealed record UntranslatedAlignmentFeatEffect : AlignmentFeatEffect, IAlignmentFeatEffect
{
    public IReadOnlyList<int> Parameters { get; init; }

    private UntranslatedAlignmentFeatEffect(int id, IReadOnlyList<int> parameters)
        : base(id)
    {
        Parameters = parameters;
    }

    internal static UntranslatedAlignmentFeatEffect Create(int id, params int[] parameters)
    {
        return new(id, parameters);
    }

    public Description GetDescription()
    {
        return GetDescription(Parameters.ToArray());
    }
}
