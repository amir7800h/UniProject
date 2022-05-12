using Application.Catalogs.CatalogItems.CatalogItemSeviceForWebSite;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebSite.EndPoint.Utility;

namespace WebSite.EndPoint.Areas.Customers.Controllers
{
    [Authorize]
    [Area("Customers")]
    public class MyFavouriteController : Controller
    {
        private readonly ICatalogItemSeviceForWebSite catalogItemSevice;

        public MyFavouriteController(ICatalogItemSeviceForWebSite catalogItemSevice)
        {
            this.catalogItemSevice = catalogItemSevice;
        }

        public IActionResult Index(int page = 1, int pageSize = 20)
        {
            string userId = ClaimUtility.GetUserId(User);
            var data = catalogItemSevice.GetMyFavourite(userId, page, pageSize);
            return View(data);
        }

        public IActionResult AddToMyFavourite(int CatalogItemId, string returnUrl = "/")
        {
            string userId = ClaimUtility.GetUserId(User);
            catalogItemSevice.AddToMyFavourite(CatalogItemId, userId);
            return Redirect(returnUrl ?? "/");
        }

        
    }
}
