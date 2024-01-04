namespace Cyberia.Api.Data;

public interface IDofusData
{

}

public interface IDofusData<T>
    : IDofusData
    where T : notnull
{
    public T Id { get; init; }
}
