using AutoMapper;
using Catalog.Host.Data;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Response;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Services;

public class CatalogService : BaseDataService<ApplicationDbContext>, ICatalogService
{
    private readonly ICatalogItemRepository _catalogItemRepository;
    private readonly IMapper _mapper;

    public CatalogService(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BaseDataService<ApplicationDbContext>> logger,
        ICatalogItemRepository catalogItemRepository,
        IMapper mapper)
        : base(dbContextWrapper, logger)
    {
        _catalogItemRepository = catalogItemRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedItemsResponse<CatalogItemDto>> GetCatalogItemsAsync(int pageSize, int pageIndex)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var result = await _catalogItemRepository.GetByPageAsync(pageIndex, pageSize);
            return new PaginatedItemsResponse<CatalogItemDto>()
            {
                Count = result.TotalCount,
                Data = result.Data.Select(s => _mapper.Map<CatalogItemDto>(s)).ToList(),
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        });
    }

    public async Task<CatalogItemDto> GetCatalogByIdAsync(int id)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var item = await _catalogItemRepository.GetByIdAsync(id);
            var response = _mapper.Map<CatalogItemDto>(item);
            return response;
        });
    }

    public async Task<ItemsResponse<CatalogItemDto>> GetCatalogByBrandAsync(string brand)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var items = await _catalogItemRepository.GetByBrandAsync(brand);
            var response = new ItemsResponse<CatalogItemDto>()
            {
                Items = items.Select(_mapper.Map<CatalogItemDto>)
            };

            return response!;
        });
    }

    public async Task<ItemsResponse<CatalogItemDto>> GetCatalogByTypeAsync(string type)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var items = await _catalogItemRepository.GetByTypeAsync(type);
            var response = new ItemsResponse<CatalogItemDto>()
            {
                Items = items.Select(_mapper.Map<CatalogItemDto>)
            };

            return response!;
        });
    }
}