using Application.Catalogs.CatalogItems.CatalogItemSeviceForWebSite;
using Microsoft.AspNetCore.Mvc;

namespace WebSite.EndPoint.Models.ViewComponents
{
    public class BrandFilter : ViewComponent
    {
        private readonly ICatalogItemSeviceForWebSite catalogItemSevice;

        public BrandFilter(ICatalogItemSeviceForWebSite catalogItemSevice)
        {
            this.catalogItemSevice = catalogItemSevice;
        }

        public IViewComponentResult Invoke()
        {
            var brands = catalogItemSevice.GetBrand();
            return View(viewName: "BrandFilter", model: brands);
        }
    }
}
