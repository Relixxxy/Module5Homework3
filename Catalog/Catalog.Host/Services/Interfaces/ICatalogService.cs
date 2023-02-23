using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Response;
using MVC.Models.Enums;

namespace Catalog.Host.Services.Interfaces;

public interface ICatalogService
{
    Task<PaginatedItemsResponse<CatalogItemDto>> GetCatalogItemsAsync(int pageSize, int pageIndex, Dictionary<CatalogTypeFilter, int>? filters);
    Task<ItemsResponse<CatalogBrandDto>> GetCatalogBrandsAsync();
    Task<ItemsResponse<CatalogTypeDto>> GetCatalogTypesAsync();
    Task<CatalogItemDto> GetCatalogByIdAsync(int id);
    Task<ItemsResponse<CatalogItemDto>> GetCatalogByBrandAsync(string brand);
    Task<ItemsResponse<CatalogItemDto>> GetCatalogByTypeAsync(string type);
}