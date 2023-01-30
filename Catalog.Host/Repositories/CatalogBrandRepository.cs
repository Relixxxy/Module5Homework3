using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Host.Repositories;

public class CatalogBrandRepository : ICatalogBrandRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CatalogBrandRepository> _logger;

    public CatalogBrandRepository(IDbContextWrapper<ApplicationDbContext> dbContextWrapper, ILogger<CatalogBrandRepository> logger)
    {
        _context = dbContextWrapper.DbContext;
        _logger = logger;
    }

    public async Task<IEnumerable<CatalogBrand>> GetBrandsAsync()
    {
        var brands = await _context.CatalogBrands.ToListAsync();
        return brands;
    }
}
