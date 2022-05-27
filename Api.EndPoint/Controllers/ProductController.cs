using Application.Catalogs.CatalogItems.GetCatalogIItemPLP;
using Application.Catalogs.CatalogItems.GetCatalogItemPDP;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace Api.EndPoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IGetCatalogIItemPLPService getCatalogIItemPLPService;
        private readonly IGetCatalogItemPDPService getCatalogItemPDPService;


        public ProductController(IGetCatalogIItemPLPService getCatalogIItemPLPService
            , IGetCatalogItemPDPService getCatalogItemPDPService)
        {
            this.getCatalogIItemPLPService = getCatalogIItemPLPService;
            this.getCatalogItemPDPService = getCatalogItemPDPService;

        }

        [HttpGet]
        public IActionResult Get([FromQuery] CatlogPLPRequestDto request)
        {
            return Ok(getCatalogIItemPLPService.Execute(request));
        }

        [HttpGet]
        [Route("PDP")]
        public IActionResult Get([FromQuery] int Id)
        {
            return Ok(getCatalogItemPDPService.Execute(Id));
        }

        
    }
}
