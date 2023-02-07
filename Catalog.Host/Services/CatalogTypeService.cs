using Catalog.Host.Data;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Services;

public class CatalogTypeService : BaseDataService<ApplicationDbContext>, ICatalogTypeService
{
    private readonly ICatalogTypeRepository _catalogTypeRepository;

    public CatalogTypeService(ICatalogTypeRepository catalogTypeRepository, IDbContextWrapper<ApplicationDbContext> dbContextWrapper, ILogger<BaseDataService<ApplicationDbContext>> logger)
        : base(dbContextWrapper, logger)
    {
        _catalogTypeRepository = catalogTypeRepository;
    }

    public Task<int?> Add(string type)
    {
        return ExecuteSafeAsync(() => _catalogTypeRepository.Add(type));
    }

    public Task<int?> Delete(int id)
    {
        return ExecuteSafeAsync(() => _catalogTypeRepository.Delete(id));
    }

    public Task<int?> Update(int id, string type)
    {
        return ExecuteSafeAsync(() => _catalogTypeRepository.Update(id, type));
    }
}
