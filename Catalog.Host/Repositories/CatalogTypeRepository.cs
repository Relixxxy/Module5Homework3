using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Helpers;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Host.Repositories;

public class CatalogTypeRepository : ICatalogTypeRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CatalogBrandRepository> _logger;

    public CatalogTypeRepository(IDbContextWrapper<ApplicationDbContext> dbContextWrapper, ILogger<CatalogBrandRepository> logger)
    {
        _context = dbContextWrapper.DbContext;
        _logger = logger;
    }

    public async Task<int?> Add(string type)
    {
        var item = await _context.CatalogTypes.AddAsync(new CatalogType { Type = type });
        await _context.SaveChangesAsync();

        return item.Entity.Id;
    }

    public async Task<int?> Delete(int id)
    {
        var item = await _context.CatalogTypes.FindAsync(id);

        CheckForNull(item);

        _context.CatalogTypes.Remove(item!);
        await _context.SaveChangesAsync();

        return id;
    }

    public async Task<IEnumerable<CatalogType>> GetTypesAsync()
    {
        var types = await _context.CatalogTypes.ToListAsync();
        return types;
    }

    public async Task<int?> Update(int id, string type)
    {
        var item = await _context.CatalogTypes.FindAsync(id);

        CheckForNull(item);

        item!.Type = type;

        _context.CatalogTypes.Update(item!);
        await _context.SaveChangesAsync();

        return id;
    }

    private void CheckForNull(CatalogType? type)
    {
        if (type is null)
        {
            string text = "Type is not found!";
            _logger.LogError(text);
            throw new NotFoundException(text);
        }
    }
}
