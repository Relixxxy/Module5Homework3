using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Helpers;
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

    public async Task<int?> Add(string brand)
    {
        var item = await _context.AddAsync(new CatalogBrand { Brand = brand });
        await _context.SaveChangesAsync();

        return item.Entity.Id;
    }

    public async Task<int?> Delete(int id)
    {
        var item = await _context.CatalogBrands.FindAsync(id);

        CheckForNull(item);

        _context.CatalogBrands.Remove(item!);
        await _context.SaveChangesAsync();

        return id;
    }

    public async Task<IEnumerable<CatalogBrand>> GetBrandsAsync()
    {
        var brands = await _context.CatalogBrands.ToListAsync();
        return brands;
    }

    public async Task<int?> Update(int id, string brand)
    {
        var item = await _context.CatalogBrands.FindAsync(id);

        CheckForNull(item);

        item!.Brand = brand;

        _context.CatalogBrands.Update(item!);
        await _context.SaveChangesAsync();

        return id;
    }

    private void CheckForNull(CatalogBrand? brand)
    {
        if (brand is null)
        {
            string text = "Brand is not found!";
            _logger.LogError(text);
            throw new NotFoundException(text);
        }
    }
}
