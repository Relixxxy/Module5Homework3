namespace Catalog.Host.Data;

public class PaginatedItems<T>
{
    public long TotalCount { get; init; }

    public IEnumerable<T> Data { get; init; } = null!;
}