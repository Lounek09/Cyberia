using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Templates;

public abstract record ParameterlessEffect : Effect
{
    protected ParameterlessEffect(int id)
        : base(id) { }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, Array.Empty<string>());
    }
}
