using System.Net;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Requests;
using Catalog.Host.Models.Response;
using Catalog.Host.Services.Interfaces;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Host.Controllers;

[ApiController]
[Route(ComponentDefaults.DefaultRoute)]
public class CatalogBffController : ControllerBase
{
    private readonly ILogger<CatalogBffController> _logger;
    private readonly ICatalogService _catalogService;

    public CatalogBffController(
        ILogger<CatalogBffController> logger,
        ICatalogService catalogService)
    {
        _logger = logger;
        _catalogService = catalogService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(PaginatedItemsResponse<CatalogItemDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Items(PaginatedItemsRequest request)
    {
        var result = await _catalogService.GetCatalogItemsAsync(request.PageSize, request.PageIndex);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CatalogItemDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetById(IntParameterRequest request)
    {
        var result = await _catalogService.GetCatalogByIdAsync(request.Parameter);

        if (result is not null)
        {
            return Ok(result);
        }

        return BadRequest(new ErrorResponse { Message = "Catalog not found!" });
    }

    [HttpPost]
    [ProducesResponseType(typeof(ItemsResponse<CatalogItemDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetByBrand(StringParameterRequest request)
    {
        var result = await _catalogService.GetCatalogByBrandAsync(request.Parameter);

        if (result is not null)
        {
            return Ok(result);
        }

        return BadRequest(new ErrorResponse { Message = "Catalogs not found!" });
    }

    [HttpPost]
    [ProducesResponseType(typeof(ItemsResponse<CatalogItemDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetByType(StringParameterRequest request)
    {
        var result = await _catalogService.GetCatalogByTypeAsync(request.Parameter);

        if (result is not null)
        {
            return Ok(result);
        }

        return BadRequest(new ErrorResponse { Message = "Catalogs not found!" });
    }
}