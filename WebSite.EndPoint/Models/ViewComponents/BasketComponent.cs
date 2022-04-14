using Application.Baskets;
using Common;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebSite.EndPoint.Utility;

namespace WebSite.EndPoint.Models.ViewComponents
{
    public class BasketComponent : ViewComponent
    {
        private readonly IBasketService basketService;
        private ClaimsPrincipal userClaimsPrincipal => ViewContext?.HttpContext?.User;
        public BasketComponent(IBasketService basketService)
        {
            this.basketService = basketService;
        }

        public IViewComponentResult Invoke()
        {
            BasketDto basket = null;
            if (User.Identity.IsAuthenticated)
            {
                basket = basketService.GetBasketForUser(ClaimUtility.GetUserId(userClaimsPrincipal));
            }
            else
            {
                if (Request.Cookies.ContainsKey(CookiesName.BasketId))
                {
                    var buyerId = Request.Cookies[CookiesName.BasketId];
                    basket = basketService.GetBasketForUser(buyerId);
                }
            }

            return View(viewName: "BasketComponent", model: basket);
        }
    }
}
