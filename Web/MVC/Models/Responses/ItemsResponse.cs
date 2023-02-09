namespace MVC.Models.Response;

public class ItemsResponse<T>
    where T : class
{
    public IEnumerable<T> Items { get; set; } = null!;
}
