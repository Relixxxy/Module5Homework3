using System.Net;
using Catalog.Host.Models.Requests;
using Catalog.Host.Models.Response;
using Catalog.Host.Services.Interfaces;
using Infrastructure;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Host.Controllers;

[ApiController]
[Route(ComponentDefaults.DefaultRoute)]
public class CatalogBrandController : ControllerBase
{
    private readonly ILogger<CatalogBrandController> _logger;
    private readonly ICatalogBrandService _catalogBrandService;

    public CatalogBrandController(ILogger<CatalogBrandController> logger, ICatalogBrandService catalogBrandService)
    {
        _logger = logger;
        _catalogBrandService = catalogBrandService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(AddItemResponse<int?>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Add(CreateBrandRequest request)
    {
        var response = await _catalogBrandService.Add(request.Brand);
        return Ok(new AddItemResponse<int?>() { Id = response });
    }

    [HttpPost]
    [ProducesResponseType(typeof(UpdateItemResponse<int?>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Update(UpdateBrandRequest request)
    {
        try
        {
            var response = await _catalogBrandService.Update(request.Id, request.Brand);
            return Ok(new UpdateItemResponse<int?>() { Id = response });
        }
        catch (NotFoundException ex)
        {
            return BadRequest(new ErrorResponse() { Message = ex.Message });
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(DeleteItemResponse<int?>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Delete(IdRequest request)
    {
        try
        {
            var response = await _catalogBrandService.Delete(request.Id);
            return Ok(new DeleteItemResponse<int?>() { Id = response });
        }
        catch (NotFoundException ex)
        {
            return BadRequest(new ErrorResponse() { Message = ex.Message });
        }
    }
}