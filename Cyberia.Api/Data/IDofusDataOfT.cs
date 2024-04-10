namespace Cyberia.Api.Data;

public interface IDofusData<T> : IDofusData
    where T : notnull
{
    public T Id { get; init; }
}
