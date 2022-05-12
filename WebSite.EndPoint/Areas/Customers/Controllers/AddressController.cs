using Application.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebSite.EndPoint.Utility;

namespace WebSite.EndPoint.Areas.Customers.Controllers
{
    [Authorize]
    [Area("Customers")]
    public class AddressController : Controller
    {
        private readonly IUserAddressService addressService;
    

        public AddressController(IUserAddressService addressService )
        {
            this.addressService = addressService;
        }
        public IActionResult Index()
        {
            var data = addressService.GetAddress(ClaimUtility.GetUserId(User));
            return View(data);
        }


        public IActionResult AddNewAddress()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddNewAddress(AddUserAddressDto addressDto)
        {
            addressDto.UserId = ClaimUtility.GetUserId(User);
            addressService.AddnewAddress(addressDto);            
            return RedirectToAction("index");
        }
    }
}
