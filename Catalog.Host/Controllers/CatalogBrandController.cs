using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Host.Controllers;

[ApiController]
[Route(ComponentDefaults.DefaultRoute)]
public class CatalogBrandController : ControllerBase
{
    private readonly ILogger<CatalogBrandController> _logger;

    public CatalogBrandController(ILogger<CatalogBrandController> logger)
    {
        _logger = logger;
    }
}