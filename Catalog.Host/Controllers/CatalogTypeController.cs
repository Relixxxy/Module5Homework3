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
public class CatalogTypeController : ControllerBase
{
    private readonly ILogger<CatalogTypeController> _logger;
    private readonly ICatalogTypeService _catalogTypeService;
    public CatalogTypeController(ILogger<CatalogTypeController> logger, ICatalogTypeService catalogTypeService)
    {
        _logger = logger;
        _catalogTypeService = catalogTypeService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(AddItemResponse<int?>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Add(CreateTypeRequest request)
    {
        var response = await _catalogTypeService.Add(request.Type);
        return Ok(new AddItemResponse<int?>() { Id = response });
    }

    [HttpPost]
    [ProducesResponseType(typeof(UpdateItemResponse<int?>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Update(UpdateTypeRequest request)
    {
        try
        {
            var response = await _catalogTypeService.Update(request.Id, request.Type);
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
            var response = await _catalogTypeService.Delete(request.Id);
            return Ok(new DeleteItemResponse<int?>() { Id = response });
        }
        catch (NotFoundException ex)
        {
            return BadRequest(new ErrorResponse() { Message = ex.Message });
        }
    }
}