using Application.Catalogs.CatalogItems.GetCatalogIItemPLP;
using Application.Catalogs.CatalogItems.GetCatalogItemPDP;
using Microsoft.AspNetCore.Mvc;

namespace WebSite.EndPoint.Controllers
{
    public class ProductController : Controller
    {
        private readonly IGetCatalogIItemPLPService getCatalogIItemPLPService;
        private readonly IGetCatalogItemPDPService getCatalogItemPDPService;
        public ProductController(IGetCatalogIItemPLPService
            getCatalogIItemPLPService, IGetCatalogItemPDPService getCatalogItemPDPService)
        {
            this.getCatalogIItemPLPService = getCatalogIItemPLPService;
            this.getCatalogItemPDPService = getCatalogItemPDPService;
        }
        public IActionResult Index(CatlogPLPRequestDto catlogPLPRequestDto)
        {
            var data = getCatalogIItemPLPService.Execute(catlogPLPRequestDto);
            return View(data);
        }

        public IActionResult Details(int Id)
        {
            var data = getCatalogItemPDPService.Execute(Id);
            return View(data);
        }
    }
}
