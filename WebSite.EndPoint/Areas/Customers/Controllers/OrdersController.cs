using Application.Services.Orders.CustomerOrdersServices;
using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebSite.EndPoint.Utility;

namespace WebSite.EndPoint.Areas.Customers.Controllers
{
    [Authorize]
    [Area("Customers")]
    public class OrdersController : Controller
    {
        private readonly ICustomerOrdersService customerOrdersService;
        private readonly UserManager<User> userManager;

        public OrdersController(ICustomerOrdersService customerOrdersService
            , UserManager<User> userManager)
        {
            this.customerOrdersService = customerOrdersService;
            this.userManager = userManager;
        }
        public IActionResult Index()
        {
            var user = ClaimUtility.GetUserId(User);
            var orders = customerOrdersService.GetMyOrder(user);
            return View(orders);
        }
    }
}
