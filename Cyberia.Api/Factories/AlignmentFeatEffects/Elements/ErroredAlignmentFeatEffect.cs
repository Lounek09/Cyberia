namespace Cyberia.Api.Factories.AlignmentFeatEffects;

public sealed record ErroredAlignmentFeatEffect : AlignmentFeatEffect, IAlignmentFeatEffect
{
    IReadOnlyList<int> Parameters { get; init; }

    private ErroredAlignmentFeatEffect(int id, IReadOnlyList<int> parameter)
        : base(id)
    {
        Parameters = parameter;
    }

    internal static ErroredAlignmentFeatEffect Create(int id, params int[] parameters)
    {
        return new(id, parameters);
    }

    public Description GetDescription()
    {
        return new(ApiTranslations.AlignmentFeatEffect_Errored,
            Id.ToString(),
            string.Join(", ", Parameters));
    }
}
