using AutoMapper;
using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Response;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using MVC.Models.Enums;

namespace Catalog.Host.Services;

public class CatalogService : BaseDataService<ApplicationDbContext>, ICatalogService
{
    private readonly ICatalogItemRepository _catalogItemRepository;
    private readonly ICatalogBrandRepository _catalogBrandRepository;
    private readonly ICatalogTypeRepository _catalogTypeRepository;
    private readonly IMapper _mapper;

    public CatalogService(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BaseDataService<ApplicationDbContext>> logger,
        ICatalogItemRepository catalogItemRepository,
        ICatalogBrandRepository catalogBrandRepository,
        ICatalogTypeRepository catalogTypeRepository,
        IMapper mapper)
        : base(dbContextWrapper, logger)
    {
        _catalogItemRepository = catalogItemRepository;
        _catalogBrandRepository = catalogBrandRepository;
        _catalogTypeRepository = catalogTypeRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedItemsResponse<CatalogItemDto>> GetCatalogItemsAsync(int pageSize, int pageIndex, Dictionary<CatalogTypeFilter, int>? filters)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var result = await _catalogItemRepository.GetByPageAsync(pageIndex, pageSize);
            var data = FilterCatalogItems(result.Data, filters);

            return new PaginatedItemsResponse<CatalogItemDto>()
            {
                Count = result.TotalCount,
                Data = data.Select(s => _mapper.Map<CatalogItemDto>(s)).ToList(),
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

    public async Task<ItemsResponse<CatalogBrandDto>> GetCatalogBrandsAsync()
    {
        return await ExecuteSafeAsync(async () =>
        {
            var items = await _catalogBrandRepository.GetBrandsAsync();
            var response = new ItemsResponse<CatalogBrandDto>()
            {
                Items = items.Select(_mapper.Map<CatalogBrandDto>)
            };

            return response!;
        });
    }

    public async Task<ItemsResponse<CatalogTypeDto>> GetCatalogTypesAsync()
    {
        return await ExecuteSafeAsync(async () =>
        {
            var items = await _catalogTypeRepository.GetTypesAsync();
            var response = new ItemsResponse<CatalogTypeDto>()
            {
                Items = items.Select(_mapper.Map<CatalogTypeDto>)
            };

            return response!;
        });
    }

    private IEnumerable<CatalogItem> FilterCatalogItems(IEnumerable<CatalogItem> data, Dictionary<CatalogTypeFilter, int>? filters)
    {
        if (filters is null)
        {
            return data;
        }

        foreach (var filter in filters)
        {
            switch (filter.Key)
            {
                case CatalogTypeFilter.Brand:
                    data = data.Where(i => i.CatalogBrandId == filter.Value);
                    break;
                case CatalogTypeFilter.Type:
                    data = data.Where(i => i.CatalogTypeId == filter.Value);
                    break;
            }
        }

        return data;
    }
}