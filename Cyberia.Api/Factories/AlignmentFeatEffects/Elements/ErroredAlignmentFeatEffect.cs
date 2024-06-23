namespace Cyberia.Api.Factories.AlignmentFeatEffects;

public sealed record ErroredAlignmentFeatEffect : AlignmentFeatEffect
{
    IReadOnlyList<int> Parameters { get; init; }

    internal ErroredAlignmentFeatEffect(int id, IReadOnlyList<int> parameter)
        : base(id)
    {
        Parameters = parameter;
    }

    public override Description GetDescription()
    {
        return new(ApiTranslations.AlignmentFeatEffect_Errored,
            Id.ToString(),
            string.Join(", ", Parameters));
    }
}
