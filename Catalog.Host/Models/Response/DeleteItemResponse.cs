namespace Catalog.Host.Models.Response;

public class DeleteItemResponse<T>
{
    public T Id { get; set; } = default!;
}
