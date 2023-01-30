using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Host.Repositories;

public class CatalogItemRepository : ICatalogItemRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<CatalogItemRepository> _logger;

    public CatalogItemRepository(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<CatalogItemRepository> logger)
    {
        _dbContext = dbContextWrapper.DbContext;
        _logger = logger;
    }

    public async Task<PaginatedItems<CatalogItem>> GetByPageAsync(int pageIndex, int pageSize)
    {
        var totalItems = await _dbContext.CatalogItems
            .LongCountAsync();

        var itemsOnPage = await _dbContext.CatalogItems
            .Include(i => i.CatalogBrand)
            .Include(i => i.CatalogType)
            .OrderBy(c => c.Name)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedItems<CatalogItem>() { TotalCount = totalItems, Data = itemsOnPage };
    }

    public async Task<int?> Add(string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName)
    {
        var item = await _dbContext.AddAsync(new CatalogItem
        {
            CatalogBrandId = catalogBrandId,
            CatalogTypeId = catalogTypeId,
            Description = description,
            Name = name,
            PictureFileName = pictureFileName,
            Price = price
        });

        await _dbContext.SaveChangesAsync();

        return item.Entity.Id;
    }

    public async Task<CatalogItem> GetByIdAsync(int id)
    {
        var item = await _dbContext.CatalogItems.FindAsync(id);
        return item!;
    }

    public async Task<IEnumerable<CatalogItem>> GetByBrandAsync(string brand)
    {
        brand = brand.ToLower();

        var items = await _dbContext.CatalogItems
            .Where(c => c.CatalogBrand.Brand.ToLower().Equals(brand))
            .Include(i => i.CatalogBrand)
            .Include(i => i.CatalogType)
            .ToListAsync();

        return items!;
    }

    public async Task<IEnumerable<CatalogItem>> GetByTypeAsync(string type)
    {
        type = type.ToLower();

        var items = await _dbContext.CatalogItems
            .Where(c => c.CatalogType.Type.ToLower().Equals(type))
            .Include(i => i.CatalogBrand)
            .Include(i => i.CatalogType)
            .ToListAsync();

        return items!;
    }
}