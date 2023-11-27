namespace Cyberia.Api;

public readonly record struct Description(string Value, params string[] Parameters)
{
    public override string ToString()
    {
        return PatternDecoder.Description(Value, Parameters);
    }

    public string ToString(Func<string, string> decorator)
    {
        return PatternDecoder.Description(Value, Array.ConvertAll(Parameters, x => string.IsNullOrEmpty(x) ? x : decorator(x)));
    }

    public static implicit operator string(Description description)
    {
        return PatternDecoder.Description(description.Value, description.Parameters);
    }
}
