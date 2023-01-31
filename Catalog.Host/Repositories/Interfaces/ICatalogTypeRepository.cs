using Catalog.Host.Data.Entities;

namespace Catalog.Host.Repositories.Interfaces;

public interface ICatalogTypeRepository
{
    Task<IEnumerable<CatalogType>> GetTypesAsync();
    Task<int?> Add(string type);
    Task<int?> Update(int id, string type);
    Task<int?> Delete(int id);
}
