using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Helpers;
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

    public async Task<int?> Update(int id, string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName)
    {
        var item = await _dbContext.CatalogItems.FindAsync(id);

        CheckForNull(item);

        item!.CatalogBrandId = catalogBrandId;
        item!.CatalogTypeId = catalogTypeId;
        item!.Description = description;
        item!.Name = name;
        item!.PictureFileName = pictureFileName;
        item!.Price = price;
        item!.AvailableStock = availableStock;
        await _dbContext.SaveChangesAsync();

        return id;
    }

    public async Task<int?> Delete(int id)
    {
        var item = await _dbContext.CatalogItems.FindAsync(id);

        CheckForNull(item);

        _dbContext.CatalogItems.Remove(item!);
        await _dbContext.SaveChangesAsync();

        return id;
    }

    private void CheckForNull(CatalogItem? item)
    {
        if (item is null)
        {
            string text = "Item is not found!";
            _logger.LogError(text);
            throw new NotFoundException(text);
        }
    }
}