using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Templates;

public abstract record MinMaxEffect : Effect
{
    public int Min { get; init; }
    public int Max { get; init; }

    protected MinMaxEffect(int id, int min, int max)
        : base(id)
    {
        Min = min;
        Max = max;
    }

    public int GetRandomValue()
    {
        return Max < Min ? Min : Random.Shared.Next(Min, Max + 1);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, Min, Max == 0 ? string.Empty : Max.ToString());
    }
}
