using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Response;

namespace Catalog.Host.Services.Interfaces;

public interface ICatalogService
{
    Task<PaginatedItemsResponse<CatalogItemDto>> GetCatalogItemsAsync(int pageSize, int pageIndex);
    Task<CatalogItemDto> GetCatalogByIdAsync(int id);
    Task<ItemsResponse<CatalogItemDto>> GetCatalogByBrandAsync(string brand);
    Task<ItemsResponse<CatalogItemDto>> GetCatalogByTypeAsync(string type);
}