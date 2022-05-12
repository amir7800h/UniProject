using Application.Baskets;
using Application.Discounts;
using Application.Orders;
using Application.Payments;
using Application.Users;
using Domain.Orders;
using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebSite.EndPoint.Models.ViewModels.Baskets;
using WebSite.EndPoint.Utility;

namespace WebSite.EndPoint.Controllers
{
    [Authorize]
    public class BasketController : Controller
    {
        private readonly IBasketService basketService;
        private readonly SignInManager<User> signInManager;
        private readonly IOrderService orderService;
        private readonly IUserAddressService userAddressService;
        private readonly IPaymentService paymentService;
        private readonly IDiscountService discountService;
        private readonly UserManager<User> userManager;
        private string userId = null;
        public BasketController(IBasketService basketService
            , SignInManager<User> signInManager, IOrderService orderService
            , IUserAddressService userAddressService, IPaymentService paymentService
            , IDiscountService discountService, UserManager<User> userManager)
        {
            this.basketService = basketService;
            this.signInManager = signInManager;
            this.orderService = orderService;
            this.userAddressService = userAddressService;
            this.paymentService = paymentService;
            this.discountService = discountService;
            this.userManager = userManager;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var data = GetOrSetBasket();
            return View(data);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Index(int CatalogitemId, int quantity = 1)
        {
            var data = GetOrSetBasket();
            basketService.AddItemToBasket(data.Id, CatalogitemId, quantity);
            return RedirectToAction(nameof(Index)); 
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult setQuantity(int basketItemId, int quantity)
        {            
            var result = basketService.SetQuantities(basketItemId, quantity);
            var basketId = GetOrSetBasket().Id;
            string coponCode = discountService.GetCoponCode(basketId);
            if(coponCode != null)
                discountService.ApplyDiscountInBasket(coponCode, basketId);
            return Json(result);
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult RemoveItemFromBasket(int ItemId)
        {
            basketService.RemoveItemFromBasket(ItemId);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ShippingPayment()
        {
            var model = new ShippingPaymentViewModel();
            string userId = ClaimUtility.GetUserId(User);
            model.Basket = basketService.GetBasketForUser(userId);
            model.UserAddresses = userAddressService.GetAddress(userId);
            return View(model);
        }

        [HttpPost]
        public IActionResult ShippingPayment(int Address, PaymentMethod PaymentMethod)
        {
            string userId = ClaimUtility.GetUserId(User);
            var basket = basketService.GetBasketForUser(userId);
            int orderId = orderService.CreateOrder(basket.Id, Address, PaymentMethod);
            if (PaymentMethod == PaymentMethod.OnlinePaymnt)
            {
                var payment = paymentService.PayForOrder(orderId);
                return RedirectToAction("Index", "Pay", new { PaymentId = payment.PaymentId });
            }

            else
            {
                //برو به صفحه سفارشات من
                return RedirectToAction("Index", "Orders", new { area = "customers" });
            }
        }


        private BasketDto GetOrSetBasket()
        {
            if (signInManager.IsSignedIn(User))
            {
                userId = ClaimUtility.GetUserId(User);
                return basketService.GetOrCreateBasketForUser(userId);
            }
            else
            {
                SetCookiesForBasket();
                return basketService.GetOrCreateBasketForUser(userId);
            }
        }

        private void SetCookiesForBasket()
        {
            string basketCookieName = "BasketId";
            if(Request.Cookies.ContainsKey(basketCookieName))
                userId = Request.Cookies[basketCookieName];

            if (userId != null) return;

            userId = Guid.NewGuid().ToString();
            var cookieOptions = new CookieOptions() { IsEssential = true };
            cookieOptions.Expires = DateTime.Now.AddYears(2);

            Response.Cookies.Append(basketCookieName, userId, cookieOptions);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult ApplyDiscount(string CouponCode, int BasketId)
        {
            var user = userManager.GetUserAsync(User).Result;

            var valisDiscount = discountService.IsDiscountValid(CouponCode, user);

            if (valisDiscount.IsSuccess)
                discountService.ApplyDiscountInBasket(CouponCode, BasketId);

            else
                TempData["InvalidMessage"] = String.Join(Environment.NewLine, valisDiscount.Message.Select(x => String.Join(",", x)));

            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        public IActionResult RemoveDiscount(int id)
        {
            discountService.RemoveDiscountFromBasket(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
