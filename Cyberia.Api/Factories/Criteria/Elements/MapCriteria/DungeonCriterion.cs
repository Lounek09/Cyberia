namespace Cyberia.Api.Factories.Criteria;

public sealed record DungeonCriterion : Criterion
{
    public bool InDungeon { get; init; }

    private DungeonCriterion(string id, char @operator, bool inDungeon)
        : base(id, @operator)
    {
        InDungeon = inDungeon;
    }

    internal static DungeonCriterion? Create(string id, char @operator, params string[] parameters)
    {
        if (parameters.Length > 0)
        {
            return new(id, @operator, parameters[0].Equals("1"));
        }

        return null;
    }

    protected override string GetDescriptionKey()
    {
        return $"Criterion.Dungeon.{GetOperatorDescriptionKey()}.{InDungeon}";
    }

    public override Description GetDescription()
    {
        return GetDescription([]);
    }
}
