using Catalog.Host.Data;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;

namespace Catalog.Host.Services;

public class CatalogBrandService : BaseDataService<ApplicationDbContext>, ICatalogBrandService
{
    private readonly ICatalogBrandRepository _catalogBrandRepository;

    public CatalogBrandService(ICatalogBrandRepository catalogBrandRepository, IDbContextWrapper<ApplicationDbContext> dbContextWrapper, ILogger<BaseDataService<ApplicationDbContext>> logger)
        : base(dbContextWrapper, logger)
    {
        _catalogBrandRepository = catalogBrandRepository;
    }

    public Task<int?> Add(string brand)
    {
        return ExecuteSafeAsync(() => _catalogBrandRepository.Add(brand));
    }

    public Task<int?> Delete(int id)
    {
        return ExecuteSafeAsync(() => _catalogBrandRepository.Delete(id));
    }

    public Task<int?> Update(int id, string brand)
    {
        return ExecuteSafeAsync(() => _catalogBrandRepository.Update(id, brand));
    }
}
